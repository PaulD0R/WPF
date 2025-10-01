using System.ComponentModel.DataAnnotations;

namespace WPFServer.Models
{
    public class PersonsFiles
    {
        public int Id { get; set; }
        public byte[]? Image { get; set; }
        [MaxLength(38)] public string PersonId { get; set; } =  null!;
        public Person Person { get; set; } =  null!;
    }
}
