namespace BusinessObjects.DTOs
{
    public class UpdateProductImageDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
        public bool IsActive { get; set; }
    }
} 