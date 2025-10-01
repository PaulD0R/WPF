using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPFServer.Context;
using WPFServer.Interfaces.Repositories;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class PersonRepository(
        UserManager<Person> userManager,
        ApplicationContext context) 
        : IPersonRepository
    {
        public async Task<bool> AddRoleByIdAsync(string id, string role)
        {
            var person = await context.Persons.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null) return false;

            await userManager.AddToRoleAsync(person, role);

            return true;
        }

        public Task<bool> DeleteRoleByIdAsync(string id, string role)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Person>> GetAllAsync()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<Person?> GetByIdAsync(string id)
        {
            return await context.Persons.Include(x => x.Files)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Person?> GetByNameAsync(string name)
        {
            return await context.Persons.Include(x => x.Files)
                .FirstOrDefaultAsync(x => x.UserName == name);
        }

        public async Task<ICollection<Person>> GetByNameSimilarsAsync(string name)
        {
            return await context.Persons.Where(x => x.UserName!.ToLower()
                .Contains(name.ToLower())).Take(5).ToListAsync();
        }

        public async Task<Person?> GetLiteByIdAsync(string id)
        {
            return await context.Persons.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool?> GetIsLikedByIdAsync(string id, int exerciseId)
        {
            var userExists = await context.Persons.AnyAsync(p => p.Id == id);
            if (!userExists) return null;

            return await context.Persons
                .Where(p => p.Id == id)
                .SelectMany(p => p.Exercises)
                .AnyAsync(e => e.Id == exerciseId);
        }

        public async Task<bool> DeleteCommentByIdAsync(string id, int commentId)
        {
            var comment = await context.Comments
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.Id == commentId && c.Person.Id == id);

            if (comment == null)
                return false;

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();  

            return true;
        }
    }
}
