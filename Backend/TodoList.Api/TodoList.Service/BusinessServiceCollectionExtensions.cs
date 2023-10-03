using Microsoft.Extensions.DependencyInjection;
using TodoList.Service.Interfaces;
using TodoList.Service.Services;

namespace TodoList.Service
{
    public static class BusinessServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices( this IServiceCollection services ) {
            services.AddScoped<ITodoItemService, TodoItemService>();
            return services;
        }
    }
}
