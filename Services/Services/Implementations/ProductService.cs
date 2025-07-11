using BusinessObjects.EntityModel;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;

namespace Services.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<Product> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<Product> AddAsync(Product entity)
        {
            _repo.Insert(entity);
            await _repo.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(Product entity)
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