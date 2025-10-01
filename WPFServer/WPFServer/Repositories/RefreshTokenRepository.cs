using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WPFServer.Context;
using WPFServer.Interfaces.Repositories;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class RefreshTokenRepository(ApplicationContext context) : IRefreshTokenRepository
    {
        public async Task<string> CreateNewRefreshTokenAsync(Person person)
        {
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                PersonId = person.Id,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                LiveTime = DateTime.UtcNow.AddDays(7)
            };
            
            await context.RefreshTokens.AddAsync(refreshToken);
            await context.SaveChangesAsync();

            return refreshToken.Token;
        }

        public async Task<bool> DeleteOldRefreshTokensAsync(string personId)
        {
            var oldTokens = await context.RefreshTokens
                .Where(x => x.PersonId == personId).ToListAsync();

            context.RefreshTokens.RemoveRange(oldTokens);

            return true;
        }

        public async Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token)
        {
            return await context.RefreshTokens.Include(x => x.Person)
                .FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task<RefreshToken> UpdateRefreshToken(RefreshToken refreshToken)
        {
            refreshToken.LiveTime = DateTime.UtcNow.AddDays(7);
            refreshToken.Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            await context.SaveChangesAsync();
            
            return refreshToken;
        }

        public async Task<bool> DeleteRefreshToken(string id)
        {
            var token = await context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.PersonId == id);

            if (token == null) return false;
            
            context.RefreshTokens.Remove(token);
            await context.SaveChangesAsync();

            return true;

        }
    }
}
