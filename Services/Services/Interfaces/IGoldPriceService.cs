using BusinessObjects.EntityModel;

namespace Services.Services.Interfaces
{
    public interface IGoldPriceService
    {
        Task UpdateGoldPricesAsync();
        Task<IEnumerable<GoldPrice>> GetLatestGoldPricesAsync();
    }
}
