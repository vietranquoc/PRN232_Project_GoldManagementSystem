using BusinessObjects.EntityModel;

namespace Services.Services.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetOrCreateActiveCartAsync();
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId);
        Task AddOrUpdateCartItemAsync(int cartId, int productId, int quantity, decimal price);
        Task RemoveCartItemAsync(int cartItemId);
        Task ClearCartAsync(int cartId);
        Task<bool> CheckoutAsync();
    }
} 