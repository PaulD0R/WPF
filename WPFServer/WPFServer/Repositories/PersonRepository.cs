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

        public async Task<bool> DeleteUserAsync(string id)
        {
            var person = await context.Persons.Include(x => x.Files)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (person == null) return false;

            var tokens = context.RefreshTokens.Where(x => x.PersonId == id);

            context.RefreshTokens.RemoveRange(tokens);
            context.PersonsFiles.Remove(person.Files);
            context.Persons.Remove(person);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeRoleAsync(string id, string newRole)
        {
            var person = await context.Persons.FirstOrDefaultAsync(x => x.Id == id);
            
            if (person == null) return false;

            await userManager.AddToRoleAsync(person, newRole);
            return true;
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

        public async Task<Person?> ChangeUserAsync(string id, Person newPerson)
        {
            var person = await context.Persons.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null) return null;

            person.UserName = newPerson.UserName ?? person.UserName;
            person.Email = newPerson.Email ?? person.Email;

            await context.SaveChangesAsync();

            return person;
        }

        public async Task<ICollection<Person>> GetAllUsersAsync()
        {
            var admins = await userManager.GetUsersInRoleAsync("Admin");
            var persons = await userManager.Users.ToListAsync();

            return persons.Except(admins).ToList();
        }

        public async Task<ICollection<Person>> GetByNameSimilarAsync(string name)
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
