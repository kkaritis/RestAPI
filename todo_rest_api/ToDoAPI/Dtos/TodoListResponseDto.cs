using System.Collections.Generic;

namespace TodoAPI.Dtos
{
    public class TodoListResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public UserResponseDto User { get; set; }
        public List<TodoItemResponseDto> TodoItems { get; set; }
    }
}
