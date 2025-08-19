using System.ComponentModel.DataAnnotations;

namespace WPFServer.DTOs.Subject
{
    public class NewSubjectRequest
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public int? Year { get; set; }
        [Required]
        public string? Description { get; set; }
    }
}
