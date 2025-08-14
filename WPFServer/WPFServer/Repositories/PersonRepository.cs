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

        public async Task<Person?> GetLiteByNameAsync(string name)
        {
            return await _context.Persons.FirstOrDefaultAsync(x => x.UserName == name);
        }

        public async Task<bool?> GetIsLikedByIdAsync(string name, int exerciseId)
        {
            bool userExists = await _context.Persons.AnyAsync(p => p.UserName == name);
            if (!userExists) return null;

            return await _context.Persons
                .Where(p => p.UserName == name)
                .SelectMany(p => p.Exercises)
                .AnyAsync(e => e.Id == exerciseId);
        }

        public async Task<bool> DeleteCommentByIdAsync(string userName, int commentId)
        {
            var comment = await _context.Comments
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.Id == commentId && c.Person.UserName == userName);

            if (comment == null)
                return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();  

            return true;
        }

        public async Task<ICollection<Comment>?> GetCommentsByNameAsync(string name)
        {
            return (await _context.Persons.Include(x => x.Comments).FirstOrDefaultAsync(x => x.UserName ==  name))?.Comments;
        }
    }
}
