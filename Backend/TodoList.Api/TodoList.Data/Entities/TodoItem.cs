using TodoList.Data.Interfaces;

namespace TodoList.Data.Entities
{
    public class TodoItem : BaseEntity
    {
        public string Description { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}