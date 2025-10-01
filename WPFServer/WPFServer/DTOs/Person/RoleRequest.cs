using System.ComponentModel.DataAnnotations;

namespace WPFServer.DTOs.Person
{
    public class RoleRequest
    {
        [Required(ErrorMessage = "Задайте роль")]
        public string? Role {  get; set; }
    }
}
