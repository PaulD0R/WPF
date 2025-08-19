using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WPFServer.Data;
using WPFServer.Interfaces;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class JwtRepository(UserManager<Person> userManager) : IJwtRepository
    {
        private readonly UserManager<Person> _userManager = userManager;

        public async Task<string> CreateJwtAsync(Person person)
        {
            var roles = await _userManager.GetRolesAsync(person);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.NameId, person.Id),
                new(JwtRegisteredClaimNames.GivenName, person.UserName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwt = new JwtSecurityToken(
                    issuer: StaticData.ISSURE,
                    audience: StaticData.AUDIENCE,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(1),
                    signingCredentials: new SigningCredentials
                        (new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StaticData.KEY)), SecurityAlgorithms.HmacSha256)
            );


            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
