using System.ComponentModel.DataAnnotations;
using System.Security.Principal;


namespace TodoList.Data.Interfaces
{
    public class BaseEntity : IEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}