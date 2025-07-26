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
                .Include(t => t.User) 
                .Where(t => t.UserId == userId && t.IsActive)
                .ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.TransactionDetails)
                    .ThenInclude(td => td.Product)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetRecentAsync(int count)
        {
            return await _context.Transactions
                .OrderByDescending(t => t.TransactionDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<(decimal totalRevenue, decimal monthIncome, decimal lastMonthIncome, IEnumerable<(int year, decimal amount)> yearAnalysis)> GetStatisticsAsync()
        {
            var all = await _context.Transactions.ToListAsync();
            var now = DateTime.Now;
            var thisMonth = all.Where(t => t.TransactionDate.Month == now.Month && t.TransactionDate.Year == now.Year);
            var lastMonth = all.Where(t => t.TransactionDate.Month == now.AddMonths(-1).Month && t.TransactionDate.Year == now.AddMonths(-1).Year);

            var totalRevenue = all.Sum(t => t.TotalAmount);
            var monthIncome = thisMonth.Sum(t => t.TotalAmount);
            var lastMonthIncome = lastMonth.Sum(t => t.TotalAmount);

            var yearAnalysis = all
                .GroupBy(t => t.TransactionDate.Year)
                .Select(g => (year: g.Key, amount: g.Sum(t => t.TotalAmount)))
                .OrderBy(x => x.year)
                .ToList();

            return (totalRevenue, monthIncome, lastMonthIncome, yearAnalysis);
        }

        public async Task<bool> UpdateStatusAsync(int transactionId, string status)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null) return false;
            
            transaction.Status = status;
            transaction.UpdatedDate = DateTime.UtcNow;
            // Không set UpdatedBy để tránh lỗi authentication
            
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
