using System.ComponentModel.DataAnnotations;

namespace WPFServer.DTOs.Person
{
    public class RoleRequests
    {
        [Required(ErrorMessage = "Задайте роль")]
        public string? Role {  get; set; }
    }
}
