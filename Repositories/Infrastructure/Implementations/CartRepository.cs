using BusinessObjects.DBContext;
using BusinessObjects.EntityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.Infrastructure.Interfaces;

namespace Repositories.Infrastructure.Implementations
{
    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        public CartRepository(GoldManagementContext context, IHttpContextAccessor httpContext) : base(context, httpContext) { }

        public async Task<Cart?> GetActiveCartByUserIdAsync()
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == CurrentUserId && c.IsActive);
        }

        public int GetCurrentUserId()
        {
            return CurrentUserId;
        }
    }
} 