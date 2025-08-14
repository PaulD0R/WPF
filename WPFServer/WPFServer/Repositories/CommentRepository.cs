using Microsoft.EntityFrameworkCore;
using WPFServer.Context;
using WPFServer.Interfaces;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class CommentRepository(ApplicationContext context) : ICommentRepository
    {
        private readonly ApplicationContext _context = context;

        public async Task<Comment?> AddAsync(Comment comment)
        {
            try
            {
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();

                return comment;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id ==  id);

            if (comment == null) return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<Comment>?> GetCommentsByExerciseIdAsync(int exerciseId)
        {
            var exercise = await _context.Exercises.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == exerciseId);

            if (exercise == null) return null;

            return exercise.Comments;
        }
    }
}
