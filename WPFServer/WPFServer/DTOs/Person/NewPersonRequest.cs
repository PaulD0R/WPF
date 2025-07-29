using System.ComponentModel.DataAnnotations;

namespace WPFServer.DTOs.Person
{
    public class NewPersonRequest
    {
        [Required]
        [MinLength(3)]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MinLength(8)]
        public string? Password { get; set; }
    }
}
