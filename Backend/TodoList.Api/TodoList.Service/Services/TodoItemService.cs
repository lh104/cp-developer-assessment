using Microsoft.Extensions.Logging;
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

        public override (bool isValid, IEnumerable<string> errors) IsValid( TodoItem entity ) {
            if ( entity.Description == "abc" ) {
                return (false, new string[] { "abc is not gott" });
            }
            return base.IsValid( entity );
        }

    }
}
