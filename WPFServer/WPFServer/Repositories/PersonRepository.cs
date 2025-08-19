using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WPFServer.Context;
using WPFServer.Interfaces;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class PersonRepository(UserManager<Person> userManager, ApplicationContext context, IMemoryCache cache) : IPersonRepository
    {
        private readonly UserManager<Person> _userManager = userManager;   
        private readonly ApplicationContext _context = context;
        private readonly IMemoryCache _cache = cache;

        public async Task<bool> AddRoleByIdAsync(string id, string role)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.Id == id);

            if (person == null) return false;

            await _userManager.AddToRoleAsync(person, role);

            return true;
        }

        public Task<bool> DeleteRoleByIdAsync(string id, string role)
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
                .ThenInclude(x => x.Subject).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Person?> GetByNameAsync(string name)
        {
            return await _context.Persons.Include(x => x.Files).Include(x => x.Exercises)
                .ThenInclude(x => x.Subject).FirstOrDefaultAsync(x => x.UserName == name);
        }

        public async Task<ICollection<Person>> GetByNameSimilarsAsync(string name)
        {
            return await _context.Persons.Where(x => x.UserName.ToLower()
                .Contains(name.ToLower())).Take(5).ToListAsync();
        }

        public async Task<Person?> GetLiteByIdAsync(string id)
        {
            if (_cache.TryGetValue(id, out Person? person)) return person;

            return await _context.Persons.FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task<bool> DeleteCommentByIdAsync(string id, int commentId)
        {
            var comment = await _context.Comments
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.Id == commentId && c.Person.Id == id);

            if (comment == null)
                return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();  

            return true;
        }

        public async Task<ICollection<Comment>?> GetCommentsByIdAsync(string id)
        {
            return (await _context.Persons.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id))?.Comments;
        }
    }
}
