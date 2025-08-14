using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPFServer.Context;
using WPFServer.Interfaces;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class PersonRepository(UserManager<Person> userManager, ApplicationContext context) : IPersonRepository
    {
        private readonly UserManager<Person> _userManager = userManager;   
        private readonly ApplicationContext _context = context;

        public async Task<bool> AddRoleByIdAsync(string userId, string role)
        {
            var user = await _context.Persons.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) return false;

            await _userManager.AddToRoleAsync(user, role);
            return true;
        }

        public Task<bool> DeleteRoleByNameAsync(string userId, string role)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Person>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<Person?> GetByIdAsync(string id)
        {
            return await _context.Persons.Include(x => x.Files).Include(x => x.Exercises)
                .ThenInclude(x => x.Subject).FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Person?> GetByNameAsync(string name)
        {
            return await _context.Persons.Include(x => x.Files).Include(x => x.Exercises)
                .ThenInclude(x => x.Subject).FirstOrDefaultAsync(x => x.UserName.Equals(name));
        }

        public async Task<ICollection<Person>> GetByNameSimilarsAsync(string name)
        {
            return await _context.Persons.Where(x => x.UserName.ToLower()
                .Contains(name.ToLower())).Take(5).ToListAsync();
        }

        public async Task<bool?> GetIsLikedByIdAsync(string id, int exerciseId)
        {
            bool userExists = await _context.Persons.AnyAsync(p => p.Id == id);
            if (!userExists) return null;

            return await _context.Persons
                .Where(p => p.Id == id)
                .SelectMany(p => p.Exercises)
                .AnyAsync(e => e.Id == exerciseId);
        }
    }
}
