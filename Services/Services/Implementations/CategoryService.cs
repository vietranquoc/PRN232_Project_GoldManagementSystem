using BusinessObjects.EntityModel;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;

namespace Services.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Category>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<Category> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<Category> AddAsync(Category entity)
        {
            _repo.Insert(entity);
            await _repo.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(Category entity)
        {
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity != null)
            {
                _repo.Remove(entity);
                await _repo.SaveChangesAsync();
            }
        }
    }
} 