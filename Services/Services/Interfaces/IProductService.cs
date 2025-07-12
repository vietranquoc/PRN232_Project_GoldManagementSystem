using BusinessObjects.DTOs;
using BusinessObjects.ViewModels;

namespace Services.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
        Task<ProductViewModel> GetProductByIdAsync(int id);
        Task<bool> CreateProductAsync(CreateProductDTO dto);
        Task<ProductViewModel> UpdateProductAsync(UpdateProductDTO dto);
        Task<bool> DeleteProductAsync(int id);
    }
} 