using WPFServer.Models;

namespace WPFServer.Interfaces.Repositories
{
    public interface IJwtRepository
    {
        public Task<string> CreateJwtAsync(Person person);
    }
}
