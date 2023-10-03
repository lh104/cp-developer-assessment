using System;

namespace TodoList.Api.Model
{
    public class TodoItemDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
