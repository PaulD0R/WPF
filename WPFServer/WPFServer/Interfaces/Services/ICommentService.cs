using WPFServer.DTOs.Comment;

namespace WPFServer.Interfaces.Services;

public interface ICommentService
{
    Task<ICollection<CommentDto>> GetCommentsByExerciseIdAsync(int exerciseId, string personId);
    Task<ICollection<CommentDto>> GetCommentsByPersonIdAsync(string personId);
    Task<ICollection<LiteCommentDto>> GetPersonCommentsByExerciseIdAsync(int exerciseId, string personId);
    Task<CommentDto> AddCommentAsync(int exerciseId, string personId, CommentRequest commentRequest);
    Task<bool> DeleteCommentAsync(int id, string personId);
}