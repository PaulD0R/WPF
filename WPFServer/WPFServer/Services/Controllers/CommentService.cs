using WPFServer.DTOs.Comment;
using WPFServer.Exceptions;
using WPFServer.Extensions.Mappers;
using WPFServer.Interfaces.Repositories;
using WPFServer.Interfaces.Services;

namespace WPFServer.Services.Controllers;

public class CommentService(
    ICommentRepository commentRepository,
    IExerciseRepository exerciseRepository,
    IPersonRepository personRepository)
    : ICommentService
{
    public async Task<ICollection<CommentDto>> GetCommentsByExerciseIdAsync(int exerciseId, string personId)
    {
        var comments = await commentRepository.GetCommentsByExerciseIdAsync(exerciseId);
        return comments.Select(x => x.ToCommentDto(personId)).ToList();
    }

    public async Task<ICollection<CommentDto>> GetCommentsByPersonIdAsync(string personId)
    {
        var comments = await commentRepository.GetCommentsByPersonIdAsync(personId);
        return comments.Select(x => x.ToCommentDto(personId)).ToList();
    }

    public async Task<ICollection<LiteCommentDto>> GetPersonCommentsByExerciseIdAsync(int exerciseId, string personId)
    {
        var comments = await commentRepository.GetPersonCommentsByExerciseId(exerciseId, personId);
        return comments.Select(x => x.ToLiteCommentDto()).ToList();
    }

    public async Task<CommentDto> AddCommentAsync(int exerciseId, string personId, CommentRequest commentRequest)
    {
        var exercise = await exerciseRepository.GetByIdAsync(exerciseId);
        if (exercise == null) throw new NotFoundException($"Exercise {exerciseId} not found");

        var person = await personRepository.GetByIdAsync(personId);
        if (person == null) throw new NotFoundException($"Person {personId} not found");

        var comment = commentRequest.ToComment(person, exerciseId);

        await commentRepository.AddAsync(comment);
            
        return comment.ToCommentDto(personId);
    }

    public async Task<bool> DeleteCommentAsync(int id, string personId)
    {
        return await commentRepository.DeleteByIdAsync(id, personId)
            ? true : throw new NotFoundException($"Comment {id} not found");
    }
}