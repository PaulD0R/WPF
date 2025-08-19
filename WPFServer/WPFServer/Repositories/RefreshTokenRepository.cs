using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WPFServer.Context;
using WPFServer.Interfaces;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class RefreshTokenRepository(ApplicationContext context) : IRefreshTokenRepository
    {
        private readonly ApplicationContext _context = context;

        public async Task<string> CreateRefreshTokenAsync(Person person)
        {
            var refershToken = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                PersonId = person.Id,
                Token = CreateToken(),
                LiveTime = DateTime.UtcNow.AddDays(7)
            };

            var oldTokens = await _context.RefreshTokens.Where(x => x.PersonId == person.Id).ToListAsync();

            _context.RefreshTokens.RemoveRange(oldTokens);

            await _context.RefreshTokens.AddAsync(refershToken);
            await _context.SaveChangesAsync();

            return refershToken.Token;
        }

        public string CreateToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public async Task<bool> DeleteRefreshToken(string id)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.PersonId == id);

            if (token != null)
            {
                _context.RefreshTokens.Remove(token);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
