using BusinessObjects.DBContext;
using BusinessObjects.EntityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.Infrastructure.Interfaces;

namespace Repositories.Infrastructure.Implementations
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(GoldManagementContext context, IHttpContextAccessor httpContext) : base(context, httpContext)
        {
        }
        public async Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId)
        {
            return await _context.Transactions
                .Include(t => t.GoldType)
                .Include(t => t.User) 
                .Where(t => t.UserId == userId && t.IsActive)
                .ToListAsync();
        }
    }
}
