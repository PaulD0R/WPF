using System.ComponentModel.DataAnnotations;

namespace WPFServer.DTOs.PersonsFiles
{
    public class NewPersonsImageRequest
    {
        [Required]
        public byte[]? Image { get; set; }
    }
}
