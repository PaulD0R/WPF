using WPFServer.DTOs.Comment;
using WPFServer.Models;

namespace WPFServer.Extensions.Mappers
{
    public static class CommentMapper
    {
        public static Comment ToComment(this CommentRequest request, Person person, int exerciseId)
        {
            return new Comment 
            {
                UserName = person.UserName,
                Text = request.Text,
                Date = DateTime.UtcNow,
                PersonId = person.Id,
                ExerciseId = exerciseId
            };
        }

        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                UserName = comment.UserName,
                Text = comment.Text,
                Date = comment.Date.Value.ToString("dd.MM.yyyy")
            };
        }
    }
}
