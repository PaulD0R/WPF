namespace WPFServer.DTOs.Person
{
    public class TokensDto
    {
        public string Jwt { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
