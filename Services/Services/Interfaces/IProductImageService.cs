using BusinessObjects.EntityModel;

namespace Services.Services.Interfaces
{
    public interface IProductImageService
    {
        Task<IEnumerable<ProductImage>> GetAllAsync();
        Task<ProductImage> GetByIdAsync(int id);
        Task<ProductImage> AddAsync(ProductImage entity);
        Task UpdateAsync(ProductImage entity);
        Task DeleteAsync(int id);
    }
} 