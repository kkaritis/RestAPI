using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models
{
    public class TodoList
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public List<TodoItem> TodoItems { get; set; }
    }
}
