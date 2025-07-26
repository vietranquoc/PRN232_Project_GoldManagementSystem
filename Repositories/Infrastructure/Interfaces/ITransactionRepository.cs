using BusinessObjects.EntityModel;

namespace Repositories.Infrastructure.Interfaces
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Transaction>> GetRecentAsync(int count);
        Task<(decimal totalRevenue, decimal monthIncome, decimal lastMonthIncome, IEnumerable<(int year, decimal amount)> yearAnalysis)> GetStatisticsAsync();
        new Task<Transaction?> GetByIdAsync(int id);
        Task<bool> UpdateStatusAsync(int transactionId, string status);
    }
}
