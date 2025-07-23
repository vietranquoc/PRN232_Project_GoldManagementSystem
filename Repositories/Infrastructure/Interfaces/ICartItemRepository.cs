using BusinessObjects.EntityModel;

namespace Repositories.Infrastructure.Interfaces
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetItemsByCartIdAsync(int cartId);
    }
} 