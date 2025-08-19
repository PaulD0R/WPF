namespace WPFServer.Models
{
    public class RefreshToken
    {
        public string? Id { get; set; }
        public string? Token { get; set; }
        public DateTime LiveTime { get; set; }
        public string? PersonId { get; set; }
        public Person? Person { get; set; }
    }
}
