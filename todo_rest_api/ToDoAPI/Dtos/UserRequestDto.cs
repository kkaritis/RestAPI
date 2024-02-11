using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Dtos
{
    public class UserRequestDto
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        [Required]
        public string Password { get; set; }
        public byte[] Salt { get; set; }
    }
}
