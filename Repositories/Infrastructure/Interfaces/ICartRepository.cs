using BusinessObjects.EntityModel;

namespace Repositories.Infrastructure.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetActiveCartByUserIdAsync();
        int GetCurrentUserId();
    }
} 