using System;

namespace BusinessObjects.DTOs
{
    public class CreateGoldPriceDTO
    {
        public int GoldTypeId { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public DateTime RecordedAt { get; set; }
    }
} 