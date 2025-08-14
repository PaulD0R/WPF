using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WPFTest.Data;
using WPFTest.Services.Interfaces;

namespace WPFTest.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        public ICollection<string> GetRole()
        {
            var token = _jwtSecurityTokenHandler.ReadJwtToken(StaticData.TOKEN);

            return token.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
        }
    }
}
