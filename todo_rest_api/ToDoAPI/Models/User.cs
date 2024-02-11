using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models
{
    public class User
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
        [Required]
        public byte[] Salt { get; set; }
    }
}
