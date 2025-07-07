using BusinessObjects.DBContext;
using BusinessObjects.EntityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.Infrastructure.Interfaces;
using System.Security.Claims;

namespace Repositories.Infrastructure.Implementations
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly GoldManagementContext _context;
        protected readonly IHttpContextAccessor _httpContext;

        public RepositoryBase(GoldManagementContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        protected int CurrentUserId
        {
            get
            {
                var identity = _httpContext.HttpContext?.User.Identity as ClaimsIdentity;
                if (identity == null || !identity.IsAuthenticated)
                {
                    throw new InvalidOperationException("User is not authenticated.");
                }

                var userClaim = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userClaim == null)
                {
                    throw new InvalidOperationException("User ID claim is missing.");
                }

                return int.Parse(userClaim.Value);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Insert(T entity)
        {
            if (entity is EntityBase entityBase)
            {
                int createdBy = 0;
                try
                {
                    createdBy = CurrentUserId;
                }
                catch
                {
                    // Không có user đăng nhập, dùng 0 (system)
                }
                entityBase.CreatedBy = createdBy;
                entityBase.CreatedDate = DateTime.UtcNow;
                entityBase.IsActive = true;
            }
            _context.Set<T>().Add(entity);
        }

        public void Insert(IEnumerable<T> LstEntities)
        {
            foreach (var entity in LstEntities)
            {
                if (entity is EntityBase entityBase)
                {
                    entityBase.CreatedBy = CurrentUserId;
                    entityBase.CreatedDate = DateTime.UtcNow;
                    entityBase.IsActive = true;
                }
            }
            _context.Set<T>().AddRange(LstEntities);
        }

        public void Update(T entityToUpdate)
        {
            if (entityToUpdate is EntityBase entityBase)
            {
                entityBase.UpdatedBy = CurrentUserId;
                entityBase.UpdatedDate = DateTime.UtcNow;
            }
            _context.Set<T>().Update(entityToUpdate);
        }

        public void Update(IEnumerable<T> LstEntities)
        {
            foreach (var entity in LstEntities)
            {
                if ( entity is EntityBase entityBase)
                {
                    entityBase.UpdatedBy = CurrentUserId;
                    entityBase.UpdatedDate = DateTime.UtcNow;
                }
            }
            _context.Set<T>().UpdateRange(LstEntities);
        }

        public void Remove(T entity)
        {
            if (entity is EntityBase entityBase)
            {
                entityBase.IsActive = false; // Soft delete
                entityBase.UpdatedBy = CurrentUserId;
                entityBase.UpdatedDate = DateTime.UtcNow;
            }
            _context.Set<T>().Update(entity);
        }

        public void Remove(IEnumerable<T> LstEntities)
        {
            foreach (var entity in LstEntities)
            {
                if (entity is EntityBase entityBase)
                {
                    entityBase.IsActive = false; // Soft delete
                    entityBase.UpdatedBy = CurrentUserId;
                    entityBase.UpdatedDate = DateTime.UtcNow;
                }
            }
            _context.Set<T>().RemoveRange(LstEntities);
        }

        public async Task<bool> CheckExist(int id)
        {
            return await _context.Set<T>().FindAsync(id) != null;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
