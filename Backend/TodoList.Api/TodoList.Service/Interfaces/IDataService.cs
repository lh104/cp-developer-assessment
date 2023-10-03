using System.Linq.Expressions;
using TodoList.Data.Interfaces;

namespace TodoList.Service.Interfaces
{
    public interface IDataService<T> where T : class, IEntity
    {
        Task<T> GetOneAsync( Guid id, params string[] includeProperties );
        Task<T> GetOneAsync( Expression<Func<T, bool>> filter = null, params string[] includeProperties );
        Task<IList<T>> GetListAsync( Expression<Func<T, bool>> filter = null, params string[] includeProperties );
        Task<T> CreateAsync( T entity );
        Task<T> UpdateAsync( T entity );
        Task<bool> ExistsAsync( Guid id );
        Task<bool> ExistsAsync( Expression<Func<T, bool>> filter = null );
    }
}
