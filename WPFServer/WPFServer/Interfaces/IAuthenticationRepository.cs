using WPFServer.DTOs.Person;
using WPFServer.Models;

namespace WPFServer.Interfaces
{
    public interface IAuthenticationRepository
    {
        public Task<Person> Signin(string name, string password);
        public Task<Person?> Signup(Person person, string password);
        public Task<TokensDto?> SigninWithRefreshToken(string refreshToken);
    }
}
