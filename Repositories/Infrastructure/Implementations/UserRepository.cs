using BusinessObjects.DBContext;
using BusinessObjects.EntityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.Infrastructure.Interfaces;

namespace Repositories.Infrastructure.Implementations
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(GoldManagementContext context, IHttpContextAccessor httpContext) : base(context, httpContext)
        {
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Role) 
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive)
                ?? throw new InvalidOperationException($"User with username '{username}' not found or is inactive.");
        }
    }
}
