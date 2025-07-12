namespace BusinessObjects.DTOs
{
    public class CreateGoldTypeDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Karat { get; set; }
        public string PriceType { get; set; }
    }
} 