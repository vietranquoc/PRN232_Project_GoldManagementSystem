using BusinessObjects.DBContext;
using BusinessObjects.EntityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.Infrastructure.Interfaces;

namespace Repositories.Infrastructure.Implementations
{
    public class GoldTypeRepository : RepositoryBase<GoldType>, IGoldTypeRepository
    {
        public GoldTypeRepository(GoldManagementContext context, IHttpContextAccessor httpContext) : base(context, httpContext)
        {
        }
        public async Task<GoldType> GetByNameAsync(string name)
        {
            return await _context.GoldTypes
                .FirstOrDefaultAsync(gt => gt.Name == name)
                ?? throw new InvalidOperationException($"Gold type with name '{name}' not found.");
        }

        public async Task<GoldType?> GetByConditionsAsync(string name, int? karat, string priceType)
        {
            return await _context.GoldTypes.FirstOrDefaultAsync(gt =>
                gt.Name == name &&
                gt.Karat == karat &&
                gt.PriceType == priceType
            );
        }

    }
}
