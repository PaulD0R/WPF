using System.ComponentModel.DataAnnotations;

namespace WPFServer.DTOs.Person
{
    public class SigninRequest
    {
        [Required]
        [MinLength(3)]
        public string? Name { get; set; }
        [Required]
        [MinLength(8)]
        public string? Password { get; set; }
    }
}
