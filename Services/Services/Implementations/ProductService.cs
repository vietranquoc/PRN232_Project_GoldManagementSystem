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
        private readonly ICategoryService _categoryService;
        private readonly IGoldTypeService _goldTypeService;
        
        public ProductService(IProductRepository repo, ICategoryService categoryService, IGoldTypeService goldTypeService)
        {
            _repo = repo;
            _categoryService = categoryService;
            _goldTypeService = goldTypeService;
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
                UpdatedDate = p.UpdatedDate,
                Images = p.ProductImages != null ? p.ProductImages.Where(img => img.IsActive).Select(img => new ProductImageViewModel
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    IsMain = img.IsMain,
                    IsActive = img.IsActive
                }).ToList() : new List<ProductImageViewModel>()
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
                UpdatedDate = p.UpdatedDate,
                Images = p.ProductImages != null ? p.ProductImages.Where(img => img.IsActive).Select(img => new ProductImageViewModel
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    IsMain = img.IsMain,
                    IsActive = img.IsActive
                }).ToList() : new List<ProductImageViewModel>()
            };
        }

        public async Task<int> CreateProductAsync(CreateProductDTO dto)
        {
            // Validate CategoryId exists
            var category = await _categoryService.GetCategoryById(dto.CategoryId);
            if (category == null)
                throw new ArgumentException($"Category with ID {dto.CategoryId} does not exist");
            
            // Validate GoldTypeId exists
            var goldType = await _goldTypeService.GetByIdAsync(dto.GoldTypeId);
            if (goldType == null)
                throw new ArgumentException($"GoldType with ID {dto.GoldTypeId} does not exist");
            
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
            return entity.Id; // Trả về productId
        }

        public async Task<ProductViewModel> UpdateProductAsync(UpdateProductDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.Id);
            if (entity == null) return null;
            
            // Validate CategoryId exists if changed
            if (entity.CategoryId != dto.CategoryId)
            {
                var category = await _categoryService.GetCategoryById(dto.CategoryId);
                if (category == null)
                    throw new ArgumentException($"Category with ID {dto.CategoryId} does not exist");
            }
            
            // Validate GoldTypeId exists if changed
            if (entity.GoldTypeId != dto.GoldTypeId)
            {
                var goldType = await _goldTypeService.GetByIdAsync(dto.GoldTypeId);
                if (goldType == null)
                    throw new ArgumentException($"GoldType with ID {dto.GoldTypeId} does not exist");
            }
            
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.CategoryId = dto.CategoryId;
            entity.GoldTypeId = dto.GoldTypeId;
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
                UpdatedDate = entity.UpdatedDate,
                Images = entity.ProductImages != null ? entity.ProductImages.Where(img => img.IsActive).Select(img => new ProductImageViewModel
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    IsMain = img.IsMain,
                    IsActive = img.IsActive
                }).ToList() : new List<ProductImageViewModel>()
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