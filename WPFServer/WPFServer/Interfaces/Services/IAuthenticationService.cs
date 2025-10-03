using WPFServer.DTOs.Person;

namespace WPFServer.Interfaces.Services;

public interface IAuthenticationService
{
    Task<TokensDto> SigninAsync(SigninRequest signinRequest);
    Task<TokensDto> SignupAsync(NewPersonRequest newPersonRequest);
    Task<bool> LogoutAsync(string id);
}