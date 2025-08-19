using WPFServer.Models;

namespace WPFServer.Interfaces
{
    public interface IRefreshTokenRepository
    {
        public Task<string> CreateRefreshTokenAsync(Person person);
        public string CreateToken();
        public Task<bool> DeleteRefreshToken(string id);
    }
}
