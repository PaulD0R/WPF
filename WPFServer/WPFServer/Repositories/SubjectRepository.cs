using Microsoft.EntityFrameworkCore;
using WPFServer.Context;
using WPFServer.Interfaces;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class SubjectRepository(ApplicationContext context) : ISubjectRepository
    {
        private readonly ApplicationContext _context = context;

        public async Task<bool> AddAsync(Subject subject)
        {
            if (subject == null) return false;

            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subject = _context.Subjects.FirstOrDefault(x => x.Id == id);

            if (subject == null) return false;

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<Subject>> GetAllAsync()
        {
            return await _context.Subjects.ToListAsync();
        }

        public async Task<Subject?> GetByIdAsync(int id)
        {
            return await _context.Subjects.Include(x => x.Exercises).ThenInclude(x => x.Persons).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
