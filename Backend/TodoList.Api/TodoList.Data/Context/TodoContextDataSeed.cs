using Microsoft.Extensions.DependencyInjection;
using TodoList.Data.Entities;

namespace TodoList.Data.Context
{
    public static class TodoContextDataSeed
    {

        public static void CreateDefaultData( IServiceProvider serviceProvider ) {
            using ( var ctx = serviceProvider.GetRequiredService<TodoContext>() ) {
                if ( !ctx.TodoItems.Any() ) {
                    CreateSampleTodoItems( ctx );
                }

            }
        }
        private static void CreateSampleTodoItems( TodoContext ctx ) {
            ctx.TodoItems.AddRange(
              new TodoItem() {
                  Description = "this is an incompleted TodoItem",
                  IsCompleted = false,

              },
              new TodoItem() {
                  Description = "this is another incompleted TodoItem",
                  IsCompleted = false,

              },
              new TodoItem() {
                  Description = "this is a completed TodoItem",
                  IsCompleted = true,
              }
            );

            ctx.SaveChanges();
        }


    }

}