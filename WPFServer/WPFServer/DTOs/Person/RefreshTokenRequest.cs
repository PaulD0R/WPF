using System.ComponentModel.DataAnnotations;

namespace WPFServer.DTOs.Person
{
    public class RefreshTokenRequest
    {
        [Required]
        public string? Token { get; set; }
    }
}
