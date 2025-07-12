namespace BusinessObjects.DTOs
{
    public class CreateProductImageDTO
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }
} 