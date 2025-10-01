using Microsoft.EntityFrameworkCore;
using WPFServer.Context;
using WPFServer.Interfaces.Repositories;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class CommentRepository(ApplicationContext context) : ICommentRepository
    {
        public async Task<Comment?> AddAsync(Comment comment)
        {
            try
            {
                await context.Comments.AddAsync(comment);
                await context.SaveChangesAsync();

                return comment;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id, string personId)
        {
            var comment = await context.Comments
                .FirstOrDefaultAsync(x => x.Id == id &&  x.PersonId == personId);

            if (comment == null) return false;

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<Comment>> GetCommentsByPersonIdAsync(string personId)
        {
            return await context.Comments.Where(x => x.PersonId == personId).ToListAsync();
        }

        public async Task<ICollection<Comment>> GetCommentsByExerciseIdAsync(int exerciseId)
        {
            return await context.Comments.Where(x => x.ExerciseId == exerciseId).ToListAsync();
        }

        public async Task<ICollection<Comment>> GetPersonCommentsByExerciseId(int exerciseId, string personId)
        {
            return await context.Comments
                .Where(x => x.ExerciseId == exerciseId &&  x.PersonId == personId)
                .ToListAsync();
        }
    }
}
