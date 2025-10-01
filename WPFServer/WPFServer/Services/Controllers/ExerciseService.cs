using WPFServer.DTOs.Exercise;
using WPFServer.DTOs.ExercisesFiles;
using WPFServer.Exceptions;
using WPFServer.Extensions.Mappers;
using WPFServer.Interfaces.Repositories;
using WPFServer.Interfaces.Services;

namespace WPFServer.Services.Controllers;

public class ExerciseService(
    IExerciseRepository exerciseRepository,
    IExerciseFilesRepository exerciseFilesRepository,
    IPersonRepository personRepository)
    : IExerciseService
{
    public async Task<ICollection<ExerciseDto>> GetAllAsync(string personId)
    {
        var exercises = await exerciseRepository.GetAllAsync();
        return exercises.Select(e => e.ToExerciseDto(personId)).ToList();
    }

    public async Task<ICollection<ExerciseDto>> GetByPageAsync(int page, string personId)
    {
        var exercises = await exerciseRepository.GetByPageAsync(page);
        return exercises.Select(e => e.ToExerciseDto(personId)).ToList();
    }

    public async Task<ICollection<ExerciseDto>> GetExercisesByPersonIdAsync(string personId)
    {
        var exercises = await exerciseRepository.GetByPersonIdAsync(personId);
        return exercises.Select(e => e.ToExerciseDto(true)).ToList();
    }

    public async Task<ICollection<ExerciseDto>> GetExercisesBySubjectAsync(int subjectId, string personId)
    {
        var exercises = await exerciseRepository.GetBySubjectIdAsync(subjectId);
        return exercises.Select(e => e.ToExerciseDto(personId)).ToList();
    }

    public async Task<ExercisesFilesDto> GetTaskAsync(int id)
    {
        var task = await exerciseFilesRepository.GetTasksFileByIdAsync(id);
        return task?.ToExercisesFilesDto() ?? 
               throw new NotFoundException($"Exercise {id} not found");
    }

    public async Task<FullExerciseDto> GetByIdAsync(int id, string personId)
    {
        var exercise = await exerciseRepository.GetByIdAsync(id);
        return exercise?.ToFullExerciseDto(personId) ?? 
               throw new NotFoundException($"Exercise {id} not found");
    }

    public async Task<ExerciseDto> AddAsync(NewExerciseRequest exerciseRequest)
    {
        var exercise = exerciseRequest.ToExercise();
        return await exerciseRepository.AddAsync(exercise) ? 
            exercise.ToExerciseDto() : throw new Exception("Failed to create new exercise");
    }

    public async Task<int> CountAsync()
    {
        return await exerciseRepository.GetCountAsync();
    }

    public async Task<int> LikesCountAsync(int id)
    {
        return await exerciseRepository.GetLikesCountByIdAsync(id) ??
               throw new NotFoundException($"Exercise {id} not found");
    }

    public async Task<bool> IsLikedAsync(int id, string personId)
    {
        return await personRepository.GetIsLikedByIdAsync(personId, id) ??
               throw new NotFoundException($"Exercise {id} or Person {personId} not found");
    }

    public async Task<bool> SwitchIsLikedAsync(int id, string personId)
    {
        return await exerciseRepository.ChangeIsLikedAsync(personId, id) ??
               throw new NotFoundException($"Exercise {id} or Person {personId} not found");
    }
}