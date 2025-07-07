    namespace BusinessObjects.DTOs
{
    public class ManualGoldPriceDTO
    {
        public string Name { get; set; }
        public int? Karat { get; set; }
        public string PriceType { get; set; }
        public string Description { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public DateTime? RecordedAt { get; set; }
    }
} 