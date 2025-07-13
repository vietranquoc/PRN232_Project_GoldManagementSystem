using BusinessObjects.DTOs;
using BusinessObjects.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
        Task<ProductViewModel> GetProductByIdAsync(int id);
        Task<int> CreateProductAsync(CreateProductDTO dto);
        Task<ProductViewModel> UpdateProductAsync(UpdateProductDTO dto);
        Task<bool> DeleteProductAsync(int id);
    }
} 