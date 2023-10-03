using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Data.Context;
using TodoList.Data.Interfaces;
using TodoList.Data.Repositories;

namespace TodoList.Data
{
    public static class RepoServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories( this IServiceCollection services ) {
            services.AddDbContext<TodoContext>( options => options.UseInMemoryDatabase( "TodoItemsDB" ), ServiceLifetime.Transient );
            services.AddTransient<ITodoItemRepository, TodoItemRepository>();
            return services;
        }
    }
}