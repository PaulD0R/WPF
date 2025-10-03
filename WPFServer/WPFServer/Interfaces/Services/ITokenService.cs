using WPFServer.DTOs.Person;

namespace WPFServer.Interfaces.Services;

public interface ITokenService
{
    Task<TokensDto> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
}