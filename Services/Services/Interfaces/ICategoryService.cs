using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObjects.DTOs;
using BusinessObjects.ViewModels;

namespace Services.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategories();
        Task<CategoryViewModel> GetCategoryById(int categoryId);
        Task<bool> CreateNewCategory(CreateCategoryDTO request);
        Task<CategoryViewModel> UpdateCategory(UpdateCategoryDTO request);
        Task<bool> DeleteCategory(int categoryId);
    }
} 