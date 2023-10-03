using Microsoft.Extensions.Logging;
using TodoList.Data.Context;
using TodoList.Data.Entities;
using TodoList.Data.Interfaces;

namespace TodoList.Data.Repositories
{
    public class TodoItemRepository : BaseRepository<TodoItem>, ITodoItemRepository
    {
        public TodoItemRepository(

           ILogger<TodoItem> log, IServiceProvider serviceProvider ) : base( log, serviceProvider ) {
        }
    }
}