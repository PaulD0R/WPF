using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WPFServer.Data;
using WPFServer.Interfaces;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        public string CreateJwt(Person person)
        {
            var claims = new List<Claim> 
            { 
                new Claim(JwtRegisteredClaimNames.GivenName, person.UserName),
                new Claim(JwtRegisteredClaimNames.Email, person.Email)
            };
            var jwt = new JwtSecurityToken(
                    issuer: StaticData.ISSURE,
                    audience: StaticData.AUDIENCE,
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StaticData.KEY)), SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
