using BusinessObjects.EntityModel;

namespace Services.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(Product entity);
        Task UpdateAsync(Product entity);
        Task DeleteAsync(int id);
    }
} 