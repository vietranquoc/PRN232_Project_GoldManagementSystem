using BusinessObjects.DTOs;
using BusinessObjects.EntityModel;

namespace Services.Services.Interfaces
{
    public interface IGoldPriceService
    {
        Task AddManualGoldPriceAsync(ManualGoldPriceDTO dto);
        Task<IEnumerable<GoldPriceResponseDTO>> GetLatestGoldPricesAsync();

    }
}
