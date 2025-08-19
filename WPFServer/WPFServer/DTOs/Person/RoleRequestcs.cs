using System.ComponentModel.DataAnnotations;

namespace WPFServer.DTOs.Person
{
    public class RoleRequestcs
    {
        [Required]
        public string? Role {  get; set; }
    }
}
