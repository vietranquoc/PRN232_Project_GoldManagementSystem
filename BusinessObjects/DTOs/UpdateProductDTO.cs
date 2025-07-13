using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs
{
    public class UpdateProductDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Tên sản phẩm không được quá 100 ký tự")]
        public string Name { get; set; }
        
        [MaxLength(500, ErrorMessage = "Mô tả không được quá 500 ký tự")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Danh mục là bắt buộc")]
        public int CategoryId { get; set; }
        
        [Required(ErrorMessage = "Loại vàng là bắt buộc")]
        public int GoldTypeId { get; set; }
        
        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int Quantity { get; set; }
        
        public bool IsActive { get; set; }
    }
} 