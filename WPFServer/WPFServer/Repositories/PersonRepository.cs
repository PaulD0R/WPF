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

        public async Task<bool?> GetIsLikedById(string name, int exerciseId)
        {
            var person = await _context.Persons.Include(x => x.Exercises)
                .FirstOrDefaultAsync(x => x.UserName.Equals(name));

            if (person == null) return null;

            var isLiked = person.Exercises?.Any(x => x.Id == exerciseId) ?? false;
            return isLiked;
        }
    }
}
