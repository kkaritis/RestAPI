using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Dtos
{
    public class TodoItemRequestDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Status { get; set; }
        public TodoListRequestDto TodoList { get; set; }
    }
}
