using WPFTest.MVVM.Model.Comments;
using WPFTest.MVVM.Model.Data;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.MVVM.Model.Files;

namespace WPFTest.ApiServices.Interfaces
{
    public interface IApiExerciseService
    {
        Task<int> GetCountAsync();
        Task<List<LiteExercise>> GetByPageAsync(int page);
        Task<ExercisesTasksFile> GetFileByIdAsync(int id);
        Task<FullExercise?> GetByIdAsync(int id);
        Task<bool> AddExerciseAsync(NewExercise exercise);
        Task<ExerciseState?> ChangeIsLikedAsync(int id);
        Task<ExerciseState?> GetLikesCountByIdAsync(int id);
        Task<bool> AddCommentAsync(int id, NewComment comment);
        Task<ICollection<FullComment>?> GetCommentsByIdAsync(int id);
    }
}
