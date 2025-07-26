using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVNPayService _vnpayService;
        private readonly ITransactionService _transactionService;
        private readonly IConfiguration _configuration;

        public PaymentController(IVNPayService vnpayService, ITransactionService transactionService, IConfiguration configuration)
        {
            _vnpayService = vnpayService;
            _transactionService = transactionService;
            _configuration = configuration;
        }

        [HttpPost("create-vnpay-payment")]
        [Authorize]
        public IActionResult CreateVNPayPayment([FromBody] VNPayPaymentDTO paymentDto)
        {
            try
            {
                // Validate input
                if (paymentDto == null)
                    return BadRequest(new { message = "Dữ liệu thanh toán không hợp lệ" });
                
                if (string.IsNullOrEmpty(paymentDto.OrderId))
                    return BadRequest(new { message = "OrderId không được để trống" });
                
                if (paymentDto.Amount <= 0)
                    return BadRequest(new { message = "Số tiền phải lớn hơn 0" });
                
                if (string.IsNullOrEmpty(paymentDto.OrderInfo))
                    return BadRequest(new { message = "Thông tin đơn hàng không được để trống" });

                var response = _vnpayService.CreatePaymentUrl(paymentDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Không thể tạo thanh toán VNPAY: " + ex.Message, details = ex.StackTrace });
            }
        }

        [HttpGet("vnpay-return")]
        [AllowAnonymous]
        public async Task<IActionResult> VNPayReturn()
        {
            try
            {
                var queryParams = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());
                
                // Log query parameters for debugging
                Console.WriteLine($"VNPAY Return Query Params: {string.Join(", ", queryParams.Select(kv => $"{kv.Key}={kv.Value}"))}");
                
                if (!queryParams.ContainsKey("vnp_SecureHash"))
                {
                    Console.WriteLine("Missing vnp_SecureHash parameter");
                    return BadRequest("Thiếu thông tin xác thực");
                }

                Console.WriteLine("Processing VNPAY return...");
                var response = _vnpayService.ProcessPaymentReturn(queryParams);
                
                Console.WriteLine($"VNPAY Response - IsSuccess: {response.IsSuccess}, OrderId: {response.OrderId}, ResponseCode: {response.ResponseCode}, Message: {response.ResponseMessage}");
                
                if (response.IsSuccess)
                {
                    // Cập nhật trạng thái đơn hàng thành COMPLETED
                    if (int.TryParse(response.OrderId, out int orderId))
                    {
                        Console.WriteLine($"Updating transaction status to COMPLETED for orderId: {orderId}");
                        await _transactionService.UpdateTransactionStatusAsync(orderId, "COMPLETED");
                    }
                    
                    // Redirect to success page
                    var webAppUrl = _configuration["WebApp:BaseUrl"];
                    var successUrl = $"{webAppUrl}/Pages/Account/paymentResult.html?orderId={Uri.EscapeDataString(response.OrderId)}&status=success&transactionId={Uri.EscapeDataString(response.TransactionId ?? "")}";
                    Console.WriteLine($"Redirecting to success: {successUrl}");
                    return Redirect(successUrl);
                }
                else
                {
                    // Redirect to error page
                    var webAppUrl = _configuration["WebApp:BaseUrl"];
                    var errorUrl = $"{webAppUrl}/Pages/Account/paymentResult.html?orderId={Uri.EscapeDataString(response.OrderId)}&status=error&message={Uri.EscapeDataString(response.ResponseMessage ?? "")}";
                    Console.WriteLine($"Redirecting to error: {errorUrl}");
                    return Redirect(errorUrl);
                }
            }
            catch (Exception ex)
            {
                // Log error details for debugging
                Console.WriteLine($"VNPAY Return Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                
                // Log error and redirect to error page
                var webAppUrl = _configuration["WebApp:BaseUrl"];
                var errorMessage = $"Đã xảy ra lỗi khi xử lý thanh toán: {ex.Message}";
                var errorUrl = $"{webAppUrl}/Pages/Account/paymentResult.html?orderId=unknown&status=error&message={Uri.EscapeDataString(errorMessage)}";
                return Redirect(errorUrl);
            }
        }

        [HttpGet("test-config")]
        public IActionResult TestConfig()
        {
            try
            {
                var vnpayConfig = _configuration.GetSection("VNPAY");
                return Ok(new
                {
                    TmnCode = vnpayConfig["TmnCode"],
                    HashSecret = vnpayConfig["HashSecret"]?.Substring(0, 10) + "...",
                    BaseUrl = vnpayConfig["BaseUrl"],
                    ReturnUrl = vnpayConfig["ReturnUrl"],
                    Version = vnpayConfig["Version"],
                    Command = vnpayConfig["Command"],
                    CurrCode = vnpayConfig["CurrCode"],
                    Locale = vnpayConfig["Locale"]
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi cấu hình: " + ex.Message });
            }
        }

        [HttpPost("test-vnpay-url")]
        public IActionResult TestVNPayUrl([FromBody] VNPayPaymentDTO paymentDto)
        {
            try
            {
                var response = _vnpayService.CreatePaymentUrl(paymentDto);
                return Ok(new
                {
                    success = true,
                    paymentUrl = response.PaymentUrl,
                    orderId = response.OrderId,
                    amount = response.Amount
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi tạo URL: " + ex.Message });
            }
        }
    }
} 