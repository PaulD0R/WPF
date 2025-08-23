using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFTest.MVVM.Model.Comments;

namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface IExerciseViewModel
    {
        int Id { get; set; }
        int SubjectId {  get; set; }
        string? Subject {  get; set; }
        int? Year {  get; set; }
        int? Number {  get; set; }
        string? Task {  get; set; }
        bool? IsLiked { get; set; }
        int? LikesCount {  get; set; }
        string? NewCommentText {  get; set; }
        ObservableCollection<FullComment>? Comments {  get; set; }
        bool? IsError {  get; set; }
        string? ErrorText {  get; set; }

        ICommand SubjectViewCommand { get; }
        ICommand LoadTasksFileCommand { get; }
        ICommand ChangeIsLikedCommand { get; }
        ICommand CreateCommentCommand { get; }

        Task LoadExercise(int id);
    }
}
