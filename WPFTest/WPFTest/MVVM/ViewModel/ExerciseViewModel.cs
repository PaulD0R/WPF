using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.Exeptions;
using WPFTest.FileStreamers;
using WPFTest.MVVM.Model.Comments;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class ExerciseViewModel : ObserverItem, IExerciseViewModel
    {
        private readonly ApiExerciseService _exerciseService;
        private readonly ApiPersonService _personService;

        private readonly IMainViewModel _mainViewModel;
        private readonly Lazy<ISubjectViewModel> _subjectViewModel;
        private readonly Lazy<IErrorViewModel> _errorViewModel;

        private int _id = 0;
        private int _subjectId = 0;
        private string? _subject = null;
        private int? _year = null;
        private int? _number = null;
        private string? _task = null;
        private bool _isLoaded = false;
        private bool? _isLiked = false;
        private int? _likesCount = null;
        private string? _newCommentText = string.Empty;
        private ObservableCollection<FullComment>? _comments = [];
        private bool? _isError = false;
        private string? _errorText = string.Empty;

        public ICommand SubjectViewCommand { get; set; }
        public ICommand LoadTasksFileCommand { get; set; }
        public ICommand ChangeIsLikedCommand { get; set; }
        public ICommand CreateCommentCommand { get; set; }

        public ExerciseViewModel(ApiExerciseService apiExerciseService, ApiPersonService personService, IMainViewModel mainViewModel, Lazy<ISubjectViewModel> subjectViewModel, Lazy<IErrorViewModel> errorViewModel)
        {
            _exerciseService = apiExerciseService;
            _personService = personService;

            _mainViewModel = mainViewModel;
            _subjectViewModel = subjectViewModel;
            _errorViewModel = errorViewModel;

            SubjectViewCommand = new RelayCommand(_ => OpenSubjectById(SubjectId));
            LoadTasksFileCommand = new AsyncRelayCommand(async _ => await GetExercisesTasksFileAsync());
            ChangeIsLikedCommand = new AsyncRelayCommand(async x =>
            {
                await _exerciseService.ChangeIsLikedAsync((int)x);
                LikesCount = await GetLikesCount();
            });
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

        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                _isLoaded = value;
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

        public async void LoadExercise(int id)
        {
            try
            {
                IsLoaded = false;
                Id = id;
                var exercise = await _exerciseService.GetByIdAsync(Id);

                if (exercise == null) throw new Exception();

                Id = exercise.Id;
                SubjectId = exercise.SubjectId;
                Subject = exercise.Subject?.Name;
                Year = exercise.Subject?.Year;
                Number = exercise.Number;
                Task = exercise.Task;
                IsLoaded = true;
                IsLiked = await _personService.GetIsLickedAsync(Id);
                LikesCount = await GetLikesCount();
                NewCommentText = string.Empty;
                Comments = new ObservableCollection<FullComment>(exercise.Comments ?? []);
                IsError = false;
                ErrorText = string.Empty;
            }
            catch (ApiExeption ex)
            {
                _errorViewModel.Value.LoadError(ex.Message);
                _mainViewModel.ChangeCurrentView(_errorViewModel.Value);
            }
        }

        public async Task<int?> GetLikesCount()
        {
            try
            {
                var exerciseState = await _exerciseService.GetLikesCountByIdAsync(Id);

                if (exerciseState == null) return null;

                return exerciseState.LikesCount;
            }
            catch (ApiExeption ex)
            {
                _errorViewModel.Value.LoadError(ex.Message);
                _mainViewModel.ChangeCurrentView(_errorViewModel.Value);

                return 0;
            }
        }

        public void OpenSubjectById(int id)
        {
            if (id <= 0) return;

            _subjectViewModel.Value.LoadSubject(id);
            _mainViewModel.ChangeCurrentView(_subjectViewModel.Value);
        }

        public async Task GetExercisesTasksFileAsync()
        {
            try
            {
                var files = await _exerciseService.GetFileByIdAsync(Id);
                ZipFileStreamer.GetFile(files.TasksFile ?? []);
            } 
            catch (ApiExeption ex)
            {
                _errorViewModel.Value.LoadError(ex.Message);
                _mainViewModel.ChangeCurrentView(_errorViewModel.Value);
            }
        }

        public async Task CreateCommentAsync()
        {
            try
            {
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
            catch (ApiExeption ex)
            {
                _errorViewModel.Value.LoadError(ex.Message);
                _mainViewModel.ChangeCurrentView(_errorViewModel.Value);
            }
        }
    }
}
