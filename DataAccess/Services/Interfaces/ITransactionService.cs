using BusinessObjects.DTOs;
using BusinessObjects.EntityModel;

namespace Services.Services.Interfaces
{
    public interface ITransactionService
    {
        Task CreateTransactionAsync(CreateTransactionDTO dto, int userId);
        Task<IEnumerable<Transaction>> GetUserTransactionsAsync(int userId);
    }
}
