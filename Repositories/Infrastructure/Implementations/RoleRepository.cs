using BusinessObjects.DBContext;
using BusinessObjects.EntityModel;
using Microsoft.AspNetCore.Http;
using Repositories.Infrastructure.Interfaces;

namespace Repositories.Infrastructure.Implementations
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(GoldManagementContext context, IHttpContextAccessor httpContext) : base(context, httpContext)
        {
        }
    }
}
