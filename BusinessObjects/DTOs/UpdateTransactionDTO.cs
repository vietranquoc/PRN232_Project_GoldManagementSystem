using System;

namespace BusinessObjects.DTOs
{
    public class UpdateTransactionDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GoldTypeId { get; set; }
        public string TransactionType { get; set; }
        public decimal Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string? ReceiverEmail { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string? Note { get; set; }
    }
} 