using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObjects.DTOs;
using BusinessObjects.ViewModels;

namespace Services.Services.Interfaces
{
    public interface IGoldTypeService
    {
        Task<IEnumerable<GoldTypeViewModel>> GetAllAsync();
        Task<GoldTypeViewModel> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateGoldTypeDTO dto);
        Task<GoldTypeViewModel> UpdateAsync(UpdateGoldTypeDTO dto);
        Task<bool> DeleteAsync(int id);
    }
} 