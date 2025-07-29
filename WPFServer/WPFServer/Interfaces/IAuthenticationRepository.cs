using WPFServer.Models;

namespace WPFServer.Interfaces
{
    public interface IAuthenticationRepository
    {
        public string CreateJwt(Person person);
    }
}
