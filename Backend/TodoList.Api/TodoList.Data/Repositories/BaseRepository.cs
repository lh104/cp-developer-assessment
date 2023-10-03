using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TodoList.Data.Context;
using TodoList.Data.Interfaces;
using TodoList.Common.Extensions;
using System;

namespace TodoList.Data.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly ILogger<TEntity> log;
        private readonly IServiceProvider serviceProvider;



        public BaseRepository( ILogger<TEntity> log, IServiceProvider serviceProvider ) {
            this.log = log;
            this.serviceProvider = serviceProvider;
        }
        protected DbSet<TEntity> GetEntitySet( TodoContext context ) {
            return context.Set<TEntity>();
        }

        protected TodoContext CreateContext() {
            return serviceProvider.GetRequiredService<TodoContext>();
        }

        public virtual async Task<TEntity> GetOneAsync( Guid id, params string[] includeProperties ) {
            log.LogDebug( $"GetOneAsync() called with id: {id}" );
            try {
                using ( var context = CreateContext() ) {
                    IQueryable<TEntity> query = GetEntitySet( context );
                    query = query.Where( e => e.Id == id );

                    if ( includeProperties != null ) {
                        foreach ( var includeProperty in includeProperties ) {
                            query = query.Include( includeProperty );
                        }
                    }

                    var result = await query.FirstOrDefaultAsync();
                    return result;
                }
            } catch ( Exception ex ) {
                string errorMessage = $"Failed to get a {typeof( TEntity ).Name} with id: {id}. {ex.GetErrorMessage()}";
                log.LogError( errorMessage, ex );
                throw new Exception( errorMessage, ex );
            }
        }

        public async Task<TEntity> GetOneAsync( Expression<Func<TEntity, bool>> filter = null, params string[] includeProperties ) {
            log.LogDebug( $"GetOneAsync() called with filter: {filter}" );
            try {
                using ( var context = CreateContext() ) {
                    IQueryable<TEntity> query = GetEntitySet( context );
                    query = query.Where( filter );

                    if ( includeProperties != null ) {
                        foreach ( var includeProperty in includeProperties ) {
                            query = query.Include( includeProperty );
                        }
                    }

                    var result = await query.FirstOrDefaultAsync();
                    return result;
                }
            } catch ( Exception ex ) {
                string errorMessage = $"Failed to get a {typeof( TEntity ).Name}. {ex.GetErrorMessage()}";
                log.LogError( errorMessage, ex );
                throw new Exception( errorMessage, ex );
            }
        }

        public virtual async Task<IList<TEntity>> GetListAsync( Expression<Func<TEntity, bool>> filter = null, params string[] includeProperties ) {
            log.LogDebug( "GetListAsync() called" );
            try {
                using ( var context = CreateContext() ) {
                    IQueryable<TEntity> query = GetEntitySet( context );
                    if ( filter != null ) {
                        query = query.Where( filter );
                    }

                    if ( includeProperties != null ) {
                        foreach ( var prop in includeProperties ) {
                            query = query.Include( prop );
                        }
                    }

                    var result = await query.ToListAsync();
                    return result;
                }
            } catch ( Exception ex ) {
                string errorMessage = $"Failed to get list of {typeof( TEntity ).Name}. {ex.GetErrorMessage()}";
                log.LogError( errorMessage, ex );
                throw new Exception( errorMessage, ex );
            }
        }

        public async Task<TEntity> CreateAsync( TEntity entity ) {
            log.LogDebug( $"CreateAsync() called with entity: {entity}" );
            try {
                using ( var context = CreateContext() ) {
                    if ( entity.Id == Guid.Empty ) {
                        entity.Id = Guid.NewGuid();
                    }

                    var entry = await context.Set<TEntity>().AddAsync( entity );

                    if ( await context.SaveChangesAsync() > 0 ) {
                        return entry.Entity;
                    } else {
                        return null;
                    }
                }
            } catch ( Exception ex ) {
                string errorMessage = $"{typeof( TEntity ).Name}Repo.CreateAsync() failed: {ex.GetErrorMessage()}";
                log.LogError( errorMessage );
                throw new Exception( errorMessage, ex );
            }
        }

        public async Task<TEntity> UpdateAsync( TEntity entity ) {
            log.LogDebug( $"UpdateAsync() called with entity: {entity}" );
            try {
                using ( var context = CreateContext() ) {
                    context.Set<TEntity>().Attach( entity );
                    context.Entry( entity ).State = EntityState.Modified;
                    if ( await context.SaveChangesAsync() > 0 ) {
                        return entity;
                    } else {
                        return null;
                    }
                }
            } catch ( Exception ex ) {
                string errorMessage = $"{typeof( TEntity ).Name}Repo.UpdateAsync() failed: {ex.GetErrorMessage()}";
                log.LogError( errorMessage );
                throw new Exception( errorMessage, ex );
            }
        }

        public async Task<bool> GetExistsAsync( Expression<Func<TEntity, bool>> filter = null ) {
            using ( var context = CreateContext() ) {
                return await context.Set<TEntity>().AnyAsync( filter );
            }
        }
    }
}