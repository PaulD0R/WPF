using System.ComponentModel.DataAnnotations;

namespace WPFServer.DTOs.Comment
{
    public class CommentRequest
    {
        [Required]
        public string? Text { get; set; }
    }
}
