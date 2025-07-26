using BusinessObjects.DTOs;
using Microsoft.Extensions.Configuration;
using Services.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System.Net;
using System.Linq;

namespace Services.Services.Implementations
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _configuration;

        public VNPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public VNPayResponseDTO CreatePaymentUrl(VNPayPaymentDTO paymentDto)
        {
            try
            {
                var vnpayConfig = _configuration.GetSection("VNPAY");
                var tmnCode = vnpayConfig["TmnCode"];
                var hashSecret = vnpayConfig["HashSecret"];
                var baseUrl = vnpayConfig["BaseUrl"];
                var returnUrl = paymentDto.ReturnUrl ?? vnpayConfig["ReturnUrl"];

                // Validate configuration
                if (string.IsNullOrEmpty(tmnCode))
                    throw new Exception("TmnCode không được cấu hình");
                if (string.IsNullOrEmpty(hashSecret))
                    throw new Exception("HashSecret không được cấu hình");
                if (string.IsNullOrEmpty(baseUrl))
                    throw new Exception("BaseUrl không được cấu hình");

                var vnpay = new SortedList<string, string>(new VNPayCompare());
                vnpay.Add("vnp_Version", vnpayConfig["Version"]);
                vnpay.Add("vnp_Command", vnpayConfig["Command"]);
                vnpay.Add("vnp_TmnCode", tmnCode);
                vnpay.Add("vnp_Amount", (paymentDto.Amount * 100).ToString()); // VNPAY expects amount in VND cents
                vnpay.Add("vnp_CurrCode", vnpayConfig["CurrCode"]);
                vnpay.Add("vnp_BankCode", "");
                vnpay.Add("vnp_TxnRef", paymentDto.OrderId);
                vnpay.Add("vnp_OrderInfo", paymentDto.OrderInfo);
                vnpay.Add("vnp_OrderType", "other");
                vnpay.Add("vnp_Locale", vnpayConfig["Locale"]);
                vnpay.Add("vnp_ReturnUrl", returnUrl);
                vnpay.Add("vnp_IpAddr", "127.0.0.1");
                vnpay.Add("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

                var paymentUrl = CreateRequestUrl(baseUrl, vnpay, hashSecret);

                return new VNPayResponseDTO
                {
                    OrderId = paymentDto.OrderId,
                    Amount = paymentDto.Amount,
                    PaymentUrl = paymentUrl,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tạo URL thanh toán VNPAY: {ex.Message}");
            }
        }

        public VNPayResponseDTO ProcessPaymentReturn(Dictionary<string, string> queryParams)
        {
            try
            {
                var vnpayConfig = _configuration.GetSection("VNPAY");
                var hashSecret = vnpayConfig["HashSecret"];

                Console.WriteLine($"VNPAY Service - HashSecret: {hashSecret?.Substring(0, 10)}...");

                var vnpay = new SortedList<string, string>(new VNPayCompare());
                foreach (var param in queryParams)
                {
                    if (!string.IsNullOrEmpty(param.Value) && param.Key.StartsWith("vnp_"))
                    {
                        vnpay.Add(param.Key, param.Value);
                    }
                }

                Console.WriteLine($"VNPAY Service - Processed params: {string.Join(", ", vnpay.Select(kv => $"{kv.Key}={kv.Value}"))}");

                var orderId = vnpay.ContainsKey("vnp_TxnRef") ? vnpay["vnp_TxnRef"] : "";
                var transactionId = vnpay.ContainsKey("vnp_TransactionNo") ? vnpay["vnp_TransactionNo"] : "";
                var responseCode = vnpay.ContainsKey("vnp_ResponseCode") ? vnpay["vnp_ResponseCode"] : "";
                var amount = vnpay.ContainsKey("vnp_Amount") ? decimal.Parse(vnpay["vnp_Amount"]) / 100 : 0;

                Console.WriteLine($"VNPAY Service - Extracted: OrderId={orderId}, TransactionId={transactionId}, ResponseCode={responseCode}, Amount={amount}");

                var isValidSignature = ValidateSignature(vnpay, hashSecret, queryParams["vnp_SecureHash"]);
                Console.WriteLine($"VNPAY Service - Signature validation: {isValidSignature}");

                return new VNPayResponseDTO
                {
                    OrderId = orderId,
                    TransactionId = transactionId,
                    Amount = amount,
                    ResponseCode = responseCode,
                    ResponseMessage = GetResponseMessage(responseCode),
                    IsSuccess = isValidSignature && responseCode == "00"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"VNPAY Service Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        private string CreateRequestUrl(string baseUrl, SortedList<string, string> requestData, string secretKey)
        {
            var data = new StringBuilder();
            foreach (var kv in requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
            }

            var querystring = data.ToString();
            var signData = querystring;
            if (signData.Length > 0)
            {
                signData = signData.Remove(signData.Length - 1, 1);
            }

            var vnp_SecureHash = HmacSHA512(secretKey, signData);
            var finalUrl = baseUrl + "?" + querystring + "vnp_SecureHash=" + vnp_SecureHash;

            return finalUrl;
        }

        private bool ValidateSignature(SortedList<string, string> requestData, string secretKey, string signature)
        {
            var data = new StringBuilder();
            foreach (var kv in requestData.Where(kv => !string.IsNullOrEmpty(kv.Value) && kv.Key != "vnp_SecureHash"))
            {
                data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
            }

            var signData = data.ToString();
            if (signData.Length > 0)
            {
                signData = signData.Remove(signData.Length - 1, 1);
            }

            var expectedSignature = HmacSHA512(secretKey, signData);
            return signature.Equals(expectedSignature, StringComparison.OrdinalIgnoreCase);
        }

        private string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }

        private string GetResponseMessage(string responseCode)
        {
            return responseCode switch
            {
                "00" => "Giao dịch thành công",
                "01" => "Giao dịch chưa hoàn tất",
                "02" => "Giao dịch bị lỗi",
                "04" => "Giao dịch đảo (Khách hàng đã bị trừ tiền tại Ngân hàng nhưng GD chưa thành công ở VNPAY)",
                "05" => "VNPAY đang xử lý",
                "06" => "VNPAY đã gửi yêu cầu hoàn tiền sang Ngân hàng",
                "07" => "Giao dịch bị nghi ngờ gian lận",
                "09" => "Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng chưa đăng ký dịch vụ InternetBanking",
                "13" => "Giao dịch không thành công do Quý khách nhập sai mật khẩu xác thực giao dịch (OTP). Xin quý khách vui lòng thực hiện lại giao dịch.",
                "65" => "Giao dịch không thành công do tài khoản của Quý khách đã vượt quá hạn mức giao dịch trong ngày.",
                "75" => "Ngân hàng thanh toán đang bảo trì.",
                "79" => "Giao dịch không thành công do Quý khách nhập sai mật khẩu thanh toán quốc tế. Xin quý khách vui lòng thực hiện lại giao dịch.",
                "99" => "Các lỗi khác (lỗi còn lại, không có trong danh sách mã lỗi đã liệt kê)",
                _ => "Mã lỗi không xác định"
            };
        }
    }

    public class VNPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
} 