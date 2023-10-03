using System.Collections.Generic;
using System.Linq;
using TodoList.Api.Model;
using TodoList.Data.Entities;

namespace TodoList.Api.Mapping
{
    public static class TodoItemMappingExtensions
    {
        public static TodoItem ToData( this TodoItemDto value ) {
            return Bootstrapper.Mapper.Map<TodoItem>( value );
        }

        public static TodoItemDto ToDC( this TodoItem value ) {
            return Bootstrapper.Mapper.Map<TodoItemDto>( value );
        }

        public static IEnumerable<TodoItem> ToData( this IEnumerable<TodoItemDto> values ) {
            return (from d in values select d.ToData());
        }

        public static IEnumerable<TodoItemDto> ToDC( this IEnumerable<TodoItem> values ) {
            return (from d in values select d.ToDC());
        }
    }
}
