using BusinessObjects.EntityModel;

namespace Repositories.Infrastructure.Interfaces
{
    public interface IGoldTypeRepository : IRepository<GoldType>
    {
        public Task<GoldType> GetByNameAsync(string name);
    }
}
