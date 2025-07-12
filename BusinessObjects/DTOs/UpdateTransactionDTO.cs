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
    }
} 