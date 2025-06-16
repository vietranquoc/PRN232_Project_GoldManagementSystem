namespace Repositories.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : class 
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        void Insert(T entity);
        void Insert(IEnumerable<T> LstEntities);
        void Update(T entityToUpdate);
        void Update(IEnumerable<T> LstEntities);
        void Remove(T entity);
        void Remove(IEnumerable<T> LstEntities);
        Task<bool> CheckExist(int id);
        Task SaveChangesAsync();
    }
}
