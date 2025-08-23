using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.Exeptions;
using WPFTest.FileStreamers;
using WPFTest.MVVM.Model.Comments;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class ExerciseViewModel : ObserverItem, IExerciseViewModel
    {
        private readonly ApiExerciseService _exerciseService;
        private readonly ApiPersonService _personService;
        private readonly INavigationService _navigationService;

        private int _id = 0;
        private int _subjectId = 0;
        private string? _subject = null;
        private int? _year = null;
        private int? _number = null;
        private string? _task = null;
        private bool? _isLiked = false;
        private int? _likesCount = null;
        private string? _newCommentText = string.Empty;
        private ObservableCollection<FullComment>? _comments = [];
        private bool? _isError = false;
        private string? _errorText = string.Empty;

        public ICommand SubjectViewCommand { get; }
        public ICommand LoadTasksFileCommand { get; }
        public ICommand ChangeIsLikedCommand { get; }
        public ICommand CreateCommentCommand { get; }

        public ExerciseViewModel(ApiExerciseService apiExerciseService, ApiPersonService personService, 
            INavigationService navigationService)
        {
            _exerciseService = apiExerciseService;
            _personService = personService;
            _navigationService = navigationService;

            SubjectViewCommand = new AsyncRelayCommand(async _ => await OpenSubject());
            LoadTasksFileCommand = new AsyncRelayCommand(async _ => await GetExercisesTasksFileAsync());
            ChangeIsLikedCommand = new AsyncRelayCommand(async _ => await ChangeIsLiked());
            CreateCommentCommand = new AsyncRelayCommand(async _ => await CreateCommentAsync());
        }

        public int Id 
        {
            get => _id;
            set => _id = value;
        }

        public int SubjectId
        {
            get => _subjectId;
            set => _subjectId = value;
        }

        public string? Subject
        {
            get => _subject;
            set
            {
                _subject = value;
                OnPropertyChanged();
            }
        }

        public int? Year
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChanged();
            }
        }

        public int? Number 
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged();
            }
        }

        public string? Task
        {
            get => _task;
            set
            {
                _task = value;
                OnPropertyChanged();
            }
        }

        public bool? IsLiked
        {
            get => _isLiked;
            set
            {
                _isLiked = value;
                OnPropertyChanged();
            }
        }

        public int? LikesCount 
        {
            get => _likesCount;
            set
            {
                _likesCount = value;
                OnPropertyChanged();
            }
        }

        public string? NewCommentText
        {
            get => _newCommentText;
            set
            {
                _newCommentText = value;
                OnPropertyChanged();
            }
        }

        public bool? IsError 
        {
            get => _isError;
            set
            {
                _isError = value;
                OnPropertyChanged();
            }
        }

        public string? ErrorText
        {
            get => _errorText;
            set
            {
                _errorText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FullComment>? Comments 
        {
            get => _comments;
            set
            {
                _comments = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadExercise(int id)
        {
            var exercise = await _exerciseService.GetByIdAsync(id) ?? throw new Exception();

            Id = exercise.Id;
            SubjectId = exercise.SubjectId;
            Subject = exercise.Subject?.Name;
            Year = exercise.Subject?.Year;
            Number = exercise.Number;
            Task = exercise.Task;
            IsLiked = await _personService.GetIsLickedAsync(Id);
            LikesCount = await GetLikesCount();
            NewCommentText = string.Empty;
            Comments = new ObservableCollection<FullComment>(exercise.Comments ?? []);
            IsError = false;
            ErrorText = string.Empty;
        }

        private async Task<int?> GetLikesCount()
        {
            var exerciseState = await _exerciseService.GetLikesCountByIdAsync(Id);

            if (exerciseState == null) return null;

            return exerciseState.LikesCount;
        }

        private async Task OpenSubject()
        {
            try
            {
                await _navigationService.NavigateToAsync<ISubjectViewModel>(async x => await x.LoadSubject(SubjectId));
            }
            catch (ApiException ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }

        private async Task GetExercisesTasksFileAsync()
        {
            try
            {
                var files = await _exerciseService.GetFileByIdAsync(Id);
                ZipFileStreamer.GetFile(files.TasksFile ?? []);
            } 
            catch (ApiException ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }

        private async Task ChangeIsLiked()
        {
            await _exerciseService.ChangeIsLikedAsync(Id);
            LikesCount = await GetLikesCount();
        }

        private async Task CreateCommentAsync()
        {
            try
            {
                ErrorText = string.Empty;
                IsError = false;

                if (NewCommentText != string.Empty && NewCommentText != null)
                {
                    var newComment = new NewComment { Text = NewCommentText };

                    if (await _exerciseService.AddCommentAsync(Id, newComment))
                    {
                        NewCommentText = string.Empty;

                        var newComments = await _exerciseService.GetCommentsByIdAsync(Id);

                        if (newComments != null) Comments = new ObservableCollection<FullComment>(newComments);
                    }
                    else
                    {
                        ErrorText = "Не удалось написать коментаарий";
                        IsError = true;
                        return;
                    }
                }
                else
                {
                    ErrorText = "Заполните все поля";
                    IsError = true;
                    return;
                }
            }
            catch (ApiException ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }
    }
}
