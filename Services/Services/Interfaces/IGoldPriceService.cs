using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObjects.DTOs;
using BusinessObjects.ViewModels;

namespace Services.Services.Interfaces
{
    public interface IGoldPriceService
    {
        Task<IEnumerable<GoldPriceViewModel>> GetAllAsync();
        Task<GoldPriceViewModel> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateGoldPriceDTO dto);
        Task<GoldPriceViewModel> UpdateAsync(UpdateGoldPriceDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<GoldPriceViewModel> GetLatestByGoldTypeIdAsync(int goldTypeId);
    }
}
