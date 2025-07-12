using System;

namespace BusinessObjects.ViewModels
{
    public class GoldPriceViewModel
    {
        public int Id { get; set; }
        public int GoldTypeId { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public DateTime RecordedAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
} 