using BusinessObjects.EntityModel;

namespace Repositories.Infrastructure.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
    }
}
