using System.Linq.Expressions;

namespace Diploma_DataAccess.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> FindAsync(int id);

        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true
            );

        Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null,
            bool isTracking = true
            );

        void Add(T entity);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

        Task SaveAsync();
    }
}