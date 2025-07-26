using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs
{
    public class VNPayPaymentDTO
    {
        [Required]
        public string OrderId { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public string OrderInfo { get; set; }
        
        public string? ReturnUrl { get; set; }
    }

    public class VNPayResponseDTO
    {
        public string OrderId { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string PaymentUrl { get; set; }
        public bool IsSuccess { get; set; }
    }
} 