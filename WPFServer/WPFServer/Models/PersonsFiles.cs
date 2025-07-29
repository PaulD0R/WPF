namespace WPFServer.Models
{
    public class PersonsFiles
    {
        public int Id { get; set; }
        public byte[]? Image { get; set; }
        public string? PersonId { get; set; }
        public Person? Person { get; set; }
    }
}
