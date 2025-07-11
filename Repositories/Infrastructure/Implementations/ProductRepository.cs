using BusinessObjects.DBContext;
using BusinessObjects.EntityModel;
using Microsoft.AspNetCore.Http;
using Repositories.Infrastructure.Interfaces;

namespace Repositories.Infrastructure.Implementations
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(GoldManagementContext context, IHttpContextAccessor httpContext) : base(context, httpContext) { }
    }
} 