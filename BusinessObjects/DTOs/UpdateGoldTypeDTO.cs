namespace BusinessObjects.DTOs
{
    public class UpdateGoldTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Karat { get; set; }
        public string PriceType { get; set; }
        public bool IsActive { get; set; }
    }
} 