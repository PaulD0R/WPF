using WPFServer.Models;

namespace WPFServer.Interfaces
{
    public interface IJwtRepository
    {
        public Task<string> CreateJwtAsync(Person person);
    }
}
