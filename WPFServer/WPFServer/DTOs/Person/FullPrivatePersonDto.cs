namespace WPFServer.DTOs.Person
{
    public class FullPrivatePersonDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] Image { get; set; } = null!;
    }
}
