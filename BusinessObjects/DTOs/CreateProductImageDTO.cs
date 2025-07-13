namespace BusinessObjects.DTOs
{
    public class CreateProductImageDTO
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }

    public class CreateMultipleProductImagesDTO
    {
        public int ProductId { get; set; }
        public List<ProductImageItemDTO> Images { get; set; } = new List<ProductImageItemDTO>();
    }

    public class ProductImageItemDTO
    {
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }
} 