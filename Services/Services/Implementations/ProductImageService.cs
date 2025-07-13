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
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _repo;
        public ProductImageService(IProductImageRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ProductImageViewModel>> GetImagesByProductIdAsync(int productId)
        {
            var images = await _repo.GetAllAsync();
            return images.Where(i => i.ProductId == productId).Select(i => new ProductImageViewModel
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                IsMain = i.IsMain,
                IsActive = i.IsActive
            });
        }

        public async Task<ProductImageViewModel> GetByIdAsync(int id)
        {
            var i = await _repo.GetByIdAsync(id);
            if (i == null) return null;
            return new ProductImageViewModel
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                IsMain = i.IsMain,
                IsActive = i.IsActive
            };
        }

        public async Task<bool> CreateAsync(CreateProductImageDTO dto)
        {
            var entity = new ProductImage
            {
                ProductId = dto.ProductId,
                ImageUrl = dto.ImageUrl,
                IsMain = dto.IsMain
            };
            _repo.Insert(entity);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateMultipleAsync(CreateMultipleProductImagesDTO dto)
        {
            var entities = new List<ProductImage>();
            
            foreach (var image in dto.Images)
            {
                var entity = new ProductImage
                {
                    ProductId = dto.ProductId,
                    ImageUrl = image.ImageUrl,
                    IsMain = image.IsMain
                };
                entities.Add(entity);
            }
            
            _repo.Insert(entities);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<ProductImageViewModel> UpdateAsync(UpdateProductImageDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.Id);
            if (entity == null) return null;
            entity.ImageUrl = dto.ImageUrl;
            entity.IsMain = dto.IsMain;
            entity.IsActive = dto.IsActive;
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return new ProductImageViewModel
            {
                Id = entity.Id,
                ImageUrl = entity.ImageUrl,
                IsMain = entity.IsMain,
                IsActive = entity.IsActive
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            _repo.Remove(entity);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
} 