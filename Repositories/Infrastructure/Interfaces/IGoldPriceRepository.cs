using BusinessObjects.EntityModel;

namespace Repositories.Infrastructure.Interfaces
{
    public interface IGoldPriceRepository : IRepository<GoldPrice>
    {
        Task<IEnumerable<GoldPrice>> GetLatestPricesAsync();
    }
}
