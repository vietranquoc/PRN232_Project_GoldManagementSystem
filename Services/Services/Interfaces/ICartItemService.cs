using BusinessObjects.EntityModel;

namespace Services.Services.Interfaces
{
    public interface ICartItemService
    {
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
        Task<IEnumerable<CartItem>> GetItemsByCartIdAsync(int cartId);
    }
} 