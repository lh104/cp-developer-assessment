using System.Linq.Expressions;

namespace TodoList.Data.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> GetOneAsync( Guid id, params string[] includeProperties );
        Task<TEntity> GetOneAsync( Expression<Func<TEntity, bool>> filter = null, params string[] includeProperties );
        Task<IList<TEntity>> GetListAsync( Expression<Func<TEntity, bool>> filter = null, params string[] includeProperties );
        Task<TEntity> CreateAsync( TEntity entity );
        Task<TEntity> UpdateAsync( TEntity entity );
        Task<bool> GetExistsAsync( Expression<Func<TEntity, bool>> filter = null );
    }
}
