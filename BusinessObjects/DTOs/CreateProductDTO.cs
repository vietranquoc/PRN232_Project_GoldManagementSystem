namespace BusinessObjects.DTOs
{
    public class CreateProductDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int GoldTypeId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
} 