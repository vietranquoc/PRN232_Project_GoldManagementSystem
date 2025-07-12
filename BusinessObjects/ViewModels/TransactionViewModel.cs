using System;

namespace BusinessObjects.ViewModels
{
    public class TransactionViewModel
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
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
} 