using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPFServer.Context;
using WPFServer.Interfaces.Repositories;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class AdminRepository(
        ApplicationContext context, 
        UserManager<Person> userManager) 
        : IAdminRepository
    {
        public async Task<bool> ChangeRoleAsync(string id, string newRole)
        {
            var person = await context.Persons.FirstOrDefaultAsync(x => x.Id == id);
            
            if (person == null) return false;

            await userManager.AddToRoleAsync(person, newRole);
            return true;
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

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null) return false;

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();

            return true;
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

        public async Task<ICollection<Person>> GetAllUsersAsync()
        {
            var admins = await userManager.GetUsersInRoleAsync("Admin");
            var persons = await userManager.Users.ToListAsync();

            return persons.Except(admins).ToList();
        }

        public async Task<ICollection<Comment>?> GetCommentsByPersonIdAsync(string id)
        {
            return (await context.Persons.Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == id))?.Comments;
        }
    }
}
