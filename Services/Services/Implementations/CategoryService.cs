using BusinessObjects.DTOs;
using BusinessObjects.EntityModel;
using BusinessObjects.ViewModels;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategories()
        {
            var categories = await _repo.GetAllAsync();
            return categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive,
                CreatedDate = c.CreatedDate,
                UpdatedDate = c.UpdatedDate
            });
        }

        public async Task<CategoryViewModel> GetCategoryById(int categoryId)
        {
            var c = await _repo.GetByIdAsync(categoryId);
            if (c == null) return null;
            return new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive,
                CreatedDate = c.CreatedDate,
                UpdatedDate = c.UpdatedDate
            };
        }

        public async Task<bool> CreateNewCategory(CreateCategoryDTO request)
        {
            var entity = new Category
            {
                Name = request.Name,
                Description = request.Description
            };
            _repo.Insert(entity);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<CategoryViewModel> UpdateCategory(UpdateCategoryDTO request)
        {
            var entity = await _repo.GetByIdAsync(request.Id);
            if (entity == null) return null;
            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.IsActive = request.IsActive;
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return new CategoryViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate
            };
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            var entity = await _repo.GetByIdAsync(categoryId);
            if (entity == null) return false;
            _repo.Remove(entity);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
} 