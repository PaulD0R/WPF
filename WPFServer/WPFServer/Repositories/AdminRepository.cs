using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPFServer.Context;
using WPFServer.Interfaces;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class AdminRepository(ApplicationContext context, UserManager<Person> userManager) : IAdminRepository
    {
        private readonly ApplicationContext _context = context;
        private readonly UserManager<Person> _userManager = userManager;

        public async Task<bool> ChangeRoleAsync(string id, string newRole)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null) return false;

            await _userManager.AddToRoleAsync(person, newRole);

            return true;
        }

        public async Task<Person?> ChangeUserAsync(string id, Person newPerson)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null) return null;

            person.UserName = newPerson.UserName ?? person.UserName;
            person.Email = newPerson.Email ?? person.Email;

            await _context.SaveChangesAsync();

            return person;
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null) return false;

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var person = await _context.Persons.Include(x => x.Files).FirstOrDefaultAsync(x => x.Id == id);
            if (person == null) return false;

            var tokens = _context.RefreshTokens.Where(x => x.PersonId == id);

            _context.RefreshTokens.RemoveRange(tokens);
            _context.PersonsFiles.Remove(person.Files);
            _context.Persons.Remove(person);
            _context.SaveChanges();

            return true;
        }

        public async Task<ICollection<Person>> GetAllUsersAsync()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var persons = await _userManager.Users.ToListAsync();

            return [.. persons.Except(admins)];
        }

        public async Task<ICollection<Comment>?> GetCommentsByPersonIdAsync(string id)
        {
            return (await _context.Persons.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id))?.Comments;
        }
    }
}
