using WPFServer.Models;

namespace WPFServer.Interfaces
{
    public interface IAuthenticationRepository
    {
        public Task<string> CreateJwtAsync(Person person);
    }
}
