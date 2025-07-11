using BusinessObjects.EntityModel;

namespace Services.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> AddAsync(Category entity);
        Task UpdateAsync(Category entity);
        Task DeleteAsync(int id);
    }
} 