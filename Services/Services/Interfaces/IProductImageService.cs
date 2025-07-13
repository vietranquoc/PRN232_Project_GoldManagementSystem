using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObjects.DTOs;
using BusinessObjects.ViewModels;

namespace Services.Services.Interfaces
{
    public interface IProductImageService
    {
        Task<IEnumerable<ProductImageViewModel>> GetImagesByProductIdAsync(int productId);
        Task<ProductImageViewModel> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateProductImageDTO dto);
        Task<bool> CreateMultipleAsync(CreateMultipleProductImagesDTO dto);
        Task<ProductImageViewModel> UpdateAsync(UpdateProductImageDTO dto);
        Task<bool> DeleteAsync(int id);
    }
} 