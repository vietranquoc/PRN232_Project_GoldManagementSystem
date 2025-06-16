using BusinessObjects.EntityModel;

namespace Repositories.Infrastructure.Interfaces
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId);
    }
}
