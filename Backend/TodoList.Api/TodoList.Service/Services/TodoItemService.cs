using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using TodoList.Data.Entities;
using TodoList.Data.Interfaces;
using TodoList.Service.Interfaces;

namespace TodoList.Service.Services
{

    public class TodoItemService : DataService<TodoItem>, ITodoItemService
    {
        public TodoItemService(
          ILogger<TodoItem> log,
          ITodoItemRepository repo ) :
          base( log, repo ) {
        }

        public override async Task<TodoItem> CreateAsync( TodoItem entity ) {
            logger.LogDebug( $"CreateAsync() called with entity: {entity}" );

            var duplicateEntityExists = await repo.GetExistsAsync( x => x.Description.ToLowerInvariant() == entity.Description.ToLowerInvariant() && !x.IsCompleted );

            if ( !duplicateEntityExists ) {
                var result = await this.repo.CreateAsync( entity );
                return result;
            } else {
                throw new InvalidOperationException( "Description already exist." );
            }
        }

        public override async Task<TodoItem> UpdateAsync( TodoItem entity ) {
            logger.LogDebug( $"UpdateAsync() called with entity: {entity}" );

            var existing = await repo.GetExistsAsync( x=>x.Id == entity.Id );
            if( !existing ) {
                throw new DbUpdateConcurrencyException( $"TodoItem with id: '{entity.Id}' does not exist!" );
            }

            var duplicateEntityExists = await repo.GetExistsAsync( x => x.Id != entity.Id && x.Description.ToLowerInvariant() == entity.Description.ToLowerInvariant() && !x.IsCompleted );
            if ( duplicateEntityExists ) {
              throw new InvalidOperationException( "Description already exist." );
            }

            var result = await this.repo.UpdateAsync( entity );
            return result;
        }


    }
}
