using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models
{
    public enum Status
    {
        Open,
        Closed
    } 
    
    public class TodoItem
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public TodoList TodoList { get; set; }
    }
}
