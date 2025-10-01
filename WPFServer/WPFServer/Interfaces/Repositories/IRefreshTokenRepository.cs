using WPFServer.Models;

namespace WPFServer.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        public Task<string> CreateNewRefreshTokenAsync(Person person);
        public Task<bool> DeleteOldRefreshTokensAsync(string personId);
        public Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token);
        public Task<RefreshToken> UpdateRefreshToken(RefreshToken refreshToken);
        public Task<bool> DeleteRefreshToken(string id);
    }
}
