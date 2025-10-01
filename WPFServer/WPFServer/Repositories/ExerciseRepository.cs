using Microsoft.EntityFrameworkCore;
using WPFServer.Context;
using WPFServer.Data;
using WPFServer.Interfaces.Repositories;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class ExerciseRepository(ApplicationContext context): IExerciseRepository
    {
        public async Task<ICollection<Exercise>> GetAllAsync()
        {
            return await context.Exercises.Include(x => x.Subject)
                .Include(x => x.Persons).ToListAsync();
        }

        public async Task<Exercise?> GetByIdAsync(int id)
        {
            return await context.Exercises.Include(x => x.Subject)
                .Include(x => x.Persons)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<Exercise>> GetByPersonIdAsync(string personId)
        {
            return await context.Exercises.Include(x => x.Subject)
                .Include(x => x.Persons)
                .Where(x => x.Persons.Any(y => y.Id == personId))
                .ToListAsync();
        }

        public async Task<ICollection<Exercise>> GetBySubjectIdAsync(int subjectId)
        {
            return await context.Exercises.Include(x => x.Subject)
                .Include(x => x.Persons).Where(x => x.SubjectId == subjectId)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exercise = await context.Exercises.FirstOrDefaultAsync(x => x.Id == id);

            if (exercise == null) return false;

            context.Exercises.Remove(exercise);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<Exercise>> GetByPageAsync(int pageNumber)
        {
            return await context.Exercises.Include(x => x.Subject)
                .Include(x => x.Persons)
                .Skip((pageNumber - 1) * StaticData.NUMBER_OF_ELEMENTS_PER_PAGE)
                .Take(StaticData.NUMBER_OF_ELEMENTS_PER_PAGE).ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await context.Exercises.CountAsync();
        }

        public ICollection<Exercise> GetByPage(int pageNumber)
        {
            return context.Exercises.Include(x => x.Subject)
                .Skip((pageNumber - 1) * StaticData.NUMBER_OF_ELEMENTS_PER_PAGE)
                .Take(StaticData.NUMBER_OF_ELEMENTS_PER_PAGE).ToList();
        }

        public int GetLength()
        {
            return context.Exercises.Count();
        }

        public async Task<bool> AddAsync(Exercise exercise)
        {
            try
            {
                await context.Exercises.AddAsync(exercise);

                var exercisesFiles = exercise.ExercisesFiles;

                exercisesFiles.ExerciseId = exercise.Id;

                await context.ExercisesFiles.AddAsync(exercisesFiles);
                await context.SaveChangesAsync();

                return true;
            }
            catch { return false; }
        }

        public async Task<bool?> ChangeIsLikedAsync(string personId, int id)
        {
            var exercise = await context.Exercises.Include(x => x.Persons)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (exercise == null) return null;
            
            var person = await context.Persons.FirstOrDefaultAsync(x => x.Id == personId);
            if (person == null) return null;

            if (!exercise.Persons.Remove(person))
            {
                exercise.Persons.Add(person);

                await context.SaveChangesAsync();
                return true;
            }

            await context.SaveChangesAsync();
            
            return false;
        }

        public async Task<int?> GetLikesCountByIdAsync(int id)
        {
            var exercise = await context.Exercises.Include(x => x.Persons)
                .FirstOrDefaultAsync(x => x.Id == id);
            return exercise?.Persons.Count;
        }
    }
}
