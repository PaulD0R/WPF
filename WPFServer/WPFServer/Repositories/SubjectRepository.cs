using Microsoft.EntityFrameworkCore;
using WPFServer.Context;
using WPFServer.Interfaces.Repositories;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class SubjectRepository(ApplicationContext context) : ISubjectRepository
    {
        public async Task<bool> AddAsync(Subject subject)
        {
            try
            {
                await context.Subjects.AddAsync(subject);
                await context.SaveChangesAsync();

                return true;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subject = context.Subjects.FirstOrDefault(x => x.Id == id);

            if (subject == null) return false;

            context.Subjects.Remove(subject);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<Subject>> GetAllAsync()
        {
            return await context.Subjects.ToListAsync();
        }

        public async Task<Subject?> GetByIdAsync(int id)
        {
            return await context.Subjects.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
