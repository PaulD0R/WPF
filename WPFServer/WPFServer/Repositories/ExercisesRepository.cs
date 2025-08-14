using Microsoft.EntityFrameworkCore;
using WPFServer.Context;
using WPFServer.Data;
using WPFServer.DTOs.ExercisesFiles;
using WPFServer.Extensions.Mappers;
using WPFServer.Interfaces;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class ExercisesRepository(ApplicationContext context) : IExercisesRepository
    {
        private readonly ApplicationContext _context = context;

        public async Task<ICollection<Exercise>> GetAllAsync()
        {
            return await _context.Exercises.Include(x => x.Subject).Include(x => x.Persons).ToListAsync();
        }

        public async Task<Exercise?> GetByIdAsync(int id)
        {
            return await _context.Exercises.Include(x => x.Subject).Include(x => x.Persons).Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exercise = await _context.Exercises.FirstOrDefaultAsync(x => x.Id == id);

            if (exercise == null) return false;

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<Exercise>> GetByPageAsync(int pageNumber)
        {
            return await _context.Exercises.Include(x => x.Subject).Include(x => x.Persons)
                .Skip((pageNumber - 1) * StaticData.NUMBER_OF_ELEMENTS_PER_PAGE)
                .Take(StaticData.NUMBER_OF_ELEMENTS_PER_PAGE).ToListAsync();
        }

        public async Task<int> GetLengthAsync()
        {
            return await _context.Exercises.CountAsync();
        }

        public ICollection<Exercise> GetByPage(int pageNumber)
        {
            return _context.Exercises.Include(x => x.Subject)
                .Skip((pageNumber - 1) * StaticData.NUMBER_OF_ELEMENTS_PER_PAGE)
                .Take(StaticData.NUMBER_OF_ELEMENTS_PER_PAGE).ToList();
        }

        public int GetLength()
        {
            return _context.Exercises.Count();
        }

        public async Task AddAsync(Exercise exercise)
        {
            await _context.Exercises.AddAsync(exercise);

            var exercisesFiles = exercise.ExercisesFiles;
            exercisesFiles.ExerciseId = exercise.Id;

            await _context.ExercisesFiles.AddAsync(exercisesFiles);
            await _context.SaveChangesAsync();
        }

        public async Task<ExercisesFilesDto?> GetTasksFileByIdAsync(int id)
        {
            var exercise = await _context.Exercises.Include(x => x.ExercisesFiles).FirstOrDefaultAsync(x => x.Id == id);

            if (exercise == null) return null;

            return exercise.ExercisesFiles?.ToExercisesFilesDto();
        }

        public async Task<bool?> ChangeIsLikedAsync(Person person, int id)
        {
            var exercise = await _context.Exercises.Include(x => x.Persons).FirstOrDefaultAsync(x => x.Id == id);

            if (exercise == null) return null;

            if (!exercise.Persons?.Remove(person) ?? false)
            {
                exercise.Persons?.Add(person);

                await _context.SaveChangesAsync();
                return true;
            }

            await _context.SaveChangesAsync();
            return false;
        }

        public async Task<int?> GetLikesCountByIdAsync(int id)
        {
            var exercise = await _context.Exercises.Include(x => x.Persons).FirstOrDefaultAsync(x => x.Id == id);

            if (exercise == null) return null;

            return exercise.Persons?.Count() ?? 0;
        }
    }
}
