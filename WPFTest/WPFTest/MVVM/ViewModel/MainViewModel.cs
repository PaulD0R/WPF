using System.Windows.Input;
using WPFTest.Core;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class MainViewModel : ObserverItem, IMainViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IJwtService _jwtService;

        private ICollection<string>? _roles = [];
        private object? _currentView = null;
        private bool? _isDiscover = false;
        private bool? _isHome = true;
        private bool? _isAddVisable;
        private string? _findName = string.Empty;

        private readonly Lazy<IHomeViewModel> _homeViewModel;
        private readonly Lazy<IDiscoverViewModel> _discoverViewModel;
        private readonly Lazy<INewExerciseViewModel> _newExercisesViewModel;
        private readonly Lazy<IPersonViewModel> _personViewModel;
        private readonly Lazy<INewSubjectViewModel> _newSubjectViewModel;
        private readonly Lazy<IErrorViewModel> _errorViewModel;
        private readonly Lazy<IExerciseViewModel> _exerciseViewModel;
        private readonly Lazy<ISubjectViewModel> _subjectViewModel;

        public ICommand HomeCommand { get; set; }
        public ICommand DiscoverCommand { get; set; }
        public ICommand NewExercisesCommand { get; set; }
        public ICommand FindPersonCommand { get; set; }
        public ICommand RolesCommand { get; set; }

        public MainViewModel(Lazy<IHomeViewModel> homeViewModel, Lazy<IDiscoverViewModel> discoverViewModel,
            Lazy<INewExerciseViewModel> newExercisesViewModel, Lazy<IPersonViewModel> personViewModel,
            Lazy<INewSubjectViewModel> newSubjectViewModel, Lazy<IErrorViewModel> errorViewModel,
            Lazy<IExerciseViewModel> exerciseViewModel, Lazy<ISubjectViewModel> subjectViewModel,
            INavigationService navigationService, IJwtService jwtService)
        {
            _navigationService = navigationService;
            _jwtService = jwtService;

            _homeViewModel = homeViewModel;
            _discoverViewModel = discoverViewModel;
            _newExercisesViewModel = newExercisesViewModel;
            _personViewModel = personViewModel;
            _newSubjectViewModel = newSubjectViewModel;
            _errorViewModel = errorViewModel;
            _exerciseViewModel = exerciseViewModel;
            _subjectViewModel = subjectViewModel;

            HomeCommand = new RelayCommand(_ => OpenHomeView());
            DiscoverCommand = new RelayCommand(_ => OpenDictonaryView());
            NewExercisesCommand = new RelayCommand(_ => OpenNewExercise());
            FindPersonCommand = new RelayCommand(_ => OpenPersonView());
            RolesCommand = new RelayCommand(_ => LoadRoles());

            _navigationService.CloseAnotherWindow<MainWindow>();
        }

        public object? CurrentView
        {
            get => _currentView; 
            set 
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public bool? IsDiscover
        {
            get => _isDiscover;
            set
            {
                _isDiscover = value;
                OnPropertyChanged();
            }
        }

        public bool? IsHome
        {
            get => _isHome;
            set
            {
                _isHome = value;
                OnPropertyChanged();
            }
        }

        public bool? IsAddVisible 
        {
            get => _isAddVisable;
            set
            {
                _isAddVisable = value;
                OnPropertyChanged();
            }
        }

        public string? FindName
        {
            get => _findName;
            set
            {
                _findName = value;
                OnPropertyChanged();
            }
        }

        public void OpenPersonView()
        {
            _personViewModel.Value.LoadPerson(FindName ?? string.Empty);
            ChangeCurrentView(_personViewModel.Value);
            FindName = string.Empty;
        }

        public void OpenExerciseView(int id)
        {
            _exerciseViewModel.Value.LoadExercise(id);
            ChangeCurrentView(_exerciseViewModel.Value);
        }

        public void OpenNewExercise()
        {
            _newExercisesViewModel.Value.LoadSubjects();
            ChangeCurrentView(_newExercisesViewModel.Value);
        }

        public void OpenNewSubjectView()
        {
            ChangeCurrentView(_newSubjectViewModel.Value);
        }

        public void OpenHomeView()
        {
            _homeViewModel.Value.LoadPerson();
            ChangeCurrentView(_homeViewModel.Value);
        }

        public void OpenDictonaryView()
        {
            _discoverViewModel.Value.LoadExercises();
            _discoverViewModel.Value.LoadPages();
            ChangeCurrentView(_discoverViewModel.Value);
        }

        public void OpenErrorView(string error)
        {
            _errorViewModel.Value.LoadError(error);
            ChangeCurrentView(_errorViewModel.Value);
        }

        public void OpenSubject(int id)
        {
            _subjectViewModel.Value.LoadSubject(id);
            ChangeCurrentView(_subjectViewModel.Value);
        }

        private void ChangeCurrentView(object currentView)
        {
            CurrentView = currentView;
            IsHome = currentView is IHomeViewModel;
            IsDiscover = currentView is IDiscoverViewModel;
        }

        public void LoadRoles()
        {
            _roles = _jwtService.GetRole();

            IsAddVisible = _roles.Contains("Admin");
        }
    }
}
