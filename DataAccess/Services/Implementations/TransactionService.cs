using BusinessObjects.DTOs;
using BusinessObjects.EntityModel;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;

namespace Services.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;

        public TransactionService(ITransactionRepository transactionRepository, IUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        public async Task CreateTransactionAsync(CreateTransactionDTO dto, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user.Role.Name != "Employee" && user.Role.Name != "Manager")
            {
                throw new Exception("Only employees or managers can create transactions.");
            }

            var transaction = new Transaction
            {
                UserId = userId,
                GoldTypeId = dto.GoldTypeId,
                TransactionType = dto.TransactionType,
                Weight = dto.Weight,
                UnitPrice = dto.UnitPrice,
                TotalAmount = dto.Weight * dto.UnitPrice,
                TransactionDate = DateTime.Now,
                Status = "COMPLETED"
                // CreatedBy và CreatedDate sẽ được gán trong RepositoryBase
            };

            _transactionRepository.Insert(transaction);
            await _transactionRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(int userId)
        {
            return await _transactionRepository.GetByUserIdAsync(userId);
        }
    }
}
