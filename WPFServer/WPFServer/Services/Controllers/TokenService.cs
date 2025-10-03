using WPFServer.DTOs.Person;
using WPFServer.Exceptions;
using WPFServer.Interfaces.Repositories;
using WPFServer.Interfaces.Services;

namespace WPFServer.Services.Controllers;

public class TokenService(
    IRefreshTokenRepository refreshTokenRepository,
    IJwtRepository jwtRepository)
    :ITokenService
{
    public async Task<TokensDto> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var refreshToken = await refreshTokenRepository.GetRefreshTokenByTokenAsync(refreshTokenRequest.Token!);
        if (refreshToken == null || refreshToken.LiveTime < DateTime.UtcNow)
            throw new BadRequestException("Not correct token");
        
        var jwtToken = await jwtRepository.CreateJwtAsync(refreshToken.Person);
        await refreshTokenRepository.UpdateRefreshToken(refreshToken);
        
        return new TokensDto
        {
            Jwt = jwtToken,
            RefreshToken = refreshToken.Token
        };
    }
}