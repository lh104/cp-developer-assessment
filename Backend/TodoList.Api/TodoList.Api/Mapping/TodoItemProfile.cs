using AutoMapper;
using TodoList.Api.Model;
using TodoList.Data.Entities;

namespace TodoList.Api.Mapping
{
    public class TodoItemProfile : Profile
    {
        public TodoItemProfile() {
            CreateMap<TodoItem, TodoItemDto>()
                .ReverseMap();
        }
    }
}
