using BusinessObjects.DTOs;
using BusinessObjects.EntityModel;
using BusinessObjects.ViewModels;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
        {
            var products = await _repo.GetAllAsync();
            return products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CategoryId = p.CategoryId,
                Price = p.Price,
                Quantity = p.Quantity,
                IsActive = p.IsActive,
                CreatedDate = p.CreatedDate,
                UpdatedDate = p.UpdatedDate
            });
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;
            return new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CategoryId = p.CategoryId,
                Price = p.Price,
                Quantity = p.Quantity,
                IsActive = p.IsActive,
                CreatedDate = p.CreatedDate,
                UpdatedDate = p.UpdatedDate
            };
        }

        public async Task<bool> CreateProductAsync(CreateProductDTO dto)
        {
            var entity = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                GoldTypeId = dto.GoldTypeId,
                Price = dto.Price,
                Quantity = dto.Quantity
            };
            _repo.Insert(entity);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<ProductViewModel> UpdateProductAsync(UpdateProductDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.Id);
            if (entity == null) return null;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.CategoryId = dto.CategoryId;
            entity.Price = dto.Price;
            entity.Quantity = dto.Quantity;
            entity.IsActive = dto.IsActive;
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return new ProductViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CategoryId = entity.CategoryId,
                Price = entity.Price,
                Quantity = entity.Quantity,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate
            };
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            _repo.Remove(entity);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
} 