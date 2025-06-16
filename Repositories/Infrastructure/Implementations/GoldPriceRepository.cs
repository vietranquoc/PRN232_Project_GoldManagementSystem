using BusinessObjects.DBContext;
using BusinessObjects.EntityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.Infrastructure.Interfaces;

namespace Repositories.Infrastructure.Implementations
{
    public class GoldPriceRepository : RepositoryBase<GoldPrice>, IGoldPriceRepository
    {
        public GoldPriceRepository(GoldManagementContext context, IHttpContextAccessor httpContext) : base(context, httpContext)
        {
        }

        public async Task<IEnumerable<GoldPrice>> GetLatestPricesAsync()
        {
            return await _context.GoldPrices
                .Include(gp => gp.GoldType)
                .GroupBy(gp => gp.GoldTypeId)
                .Select(g => g.OrderByDescending(gp => gp.RecordedAt).FirstOrDefault()!)
                .ToListAsync();
        }
    }
}
