using BusinessObjects.EntityModel;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;

namespace Services.Services.Implementations
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _repo;
        public ProductImageService(IProductImageRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ProductImage>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<ProductImage> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<ProductImage> AddAsync(ProductImage entity)
        {
            _repo.Insert(entity);
            await _repo.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(ProductImage entity)
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