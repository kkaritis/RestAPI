using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TodoAPI.Models;

namespace TodoAPI.Dtos
{
    public class TodoListRequestDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public User User { get; set; }
        [Required]
        public List<TodoItemRequestDto> TodoItems { get; set; }
    }
}
