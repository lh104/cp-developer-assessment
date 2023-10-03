using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TodoList.Data.Interfaces;
using TodoList.Service.Interfaces;

namespace TodoList.Service.Services
{
    public class DataService<TEntity> : IDataService<TEntity> where TEntity : class, IEntity
    {
        protected readonly ILogger<TEntity> logger;
        protected readonly IBaseRepository<TEntity> repo;

        public DataService(
           ILogger<TEntity> logger,
           IBaseRepository<TEntity> repo ) {
            this.logger = logger;
            this.repo = repo;
        }

        public Task<IList<TEntity>> GetListAsync( Expression<Func<TEntity, bool>> filter = null, params string[] includeProperties ) {
            logger.LogDebug( "GetListAsync() called" );
            return this.repo.GetListAsync( filter, includeProperties );
        }

        public virtual async Task<TEntity> CreateAsync( TEntity entity ) {
            logger.LogDebug( $"CreateAsync() called with entity: {entity}" );
            var isValidTuple = IsValid( entity );
            if ( isValidTuple.isValid ) {
                var result = await this.repo.CreateAsync( entity );
                return result;
            } else {
                throw new InvalidOperationException( string.Join( ", ", isValidTuple.errors ) );
            }
        }

        public virtual async Task<TEntity> UpdateAsync( TEntity entity ) {
            logger.LogDebug( $"UpdateAsync() called with entity: {entity}" );
            var isValidTuple = IsValid( entity );
            if ( isValidTuple.isValid ) {
                var result = await this.repo.UpdateAsync( entity );
                return result;
            } else {
                throw new InvalidOperationException( string.Join( ", ", isValidTuple.errors ) );
            }
        }

        public Task<TEntity> GetOneAsync( Guid id, params string[] includeProperties ) {
            logger.LogDebug( $"GetOneAsync() called with id: {id}" );
            return this.repo.GetOneAsync( id, includeProperties );
        }

        public Task<TEntity> GetOneAsync( Expression<Func<TEntity, bool>> filter = null, params string[] includeProperties ) {
            logger.LogDebug( $"GetOneAsync() called with filter: {filter}" );
            return this.repo.GetOneAsync( filter, includeProperties );
        }

        public Task<bool> ExistsAsync( Guid id ) {
            logger.LogDebug( $"ExistsAsync() called with id: {id}" );
            return this.repo.GetExistsAsync( c => c.Id == id );
        }

        public Task<bool> ExistsAsync( Expression<Func<TEntity, bool>> filter = null ) {
            logger.LogDebug( $"ExistsAsync() called with filter: {filter}" );
            return this.repo.GetExistsAsync( filter );
        }

        protected static List<string> NoErrors = new List<string>();
        public virtual (bool isValid, IEnumerable<string> errors) IsValid( TEntity entity ) {
            return (true, NoErrors);
        }

    }
}
