using System.Windows.Input;
using System.Windows.Media;
using WPFTest.Core;
using WPFTest.Data;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class MainViewModel : ObserverItem, IMainViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IJwtService _jwtService;

        private ICollection<string>? _roles;
        private object _currentView;
        private bool _isMenuVisible;
        private bool? _isDiscover;
        private bool? _isHome;
        private bool? _isAddVisable;
        private ImageSource? _closeImage;
        private ImageSource? _fullSizeImage;
        private int _radius;
        private string? _findName;

        private readonly Lazy<IHomeViewModel> _homeViewModel;
        private readonly Lazy<IDiscoverViewModel> _discoverViewModel;
        private readonly Lazy<INewExerciseViewModel> _newExercisesViewModel;
        private readonly Lazy<IPersonViewModel> _personViewModel;

        public ICommand HomeCommand { get; set; }
        public ICommand DiscoverCommand { get; set; }
        public ICommand NewExercisesCommand { get; set; }
        public ICommand ChangeMenuCommand {  get; set; }
        public ICommand ChangeWindowSizeCommand { get; set; }
        public ICommand FindPersonCommand { get; set; }
        public ICommand FindNameCommand { get; set; }

        public MainViewModel(Lazy<IHomeViewModel> homeViewModel, Lazy<IDiscoverViewModel> discoverViewModel, Lazy<INewExerciseViewModel> newExercisesViewModel, Lazy<IPersonViewModel> personViewModel, INavigationService navigationService, IJwtService jwtService)
        {
            _navigationService = navigationService;
            _jwtService = jwtService;

            _homeViewModel = homeViewModel;
            _discoverViewModel = discoverViewModel;
            _newExercisesViewModel = newExercisesViewModel;
            _personViewModel = personViewModel;

            _currentView = _homeViewModel.Value;
            _isMenuVisible = true;
            _isDiscover = false;
            _isHome = true;
            _radius = StaticData.MAIN_WINDOW_RADIUS;

            HomeCommand = new RelayCommand(_ =>
            {
                (_homeViewModel.Value).LoadPerson();
                ChangeCurrentView(_homeViewModel.Value);
            });
            DiscoverCommand = new RelayCommand(_ =>
            {
                (_discoverViewModel.Value).LoadExercises();
                (_discoverViewModel.Value).LoadPages();
                ChangeCurrentView(_discoverViewModel.Value);
            });
            NewExercisesCommand = new RelayCommand(_ => 
            {
                (_newExercisesViewModel.Value).LoadSubjects();
                ChangeCurrentView(_newExercisesViewModel.Value);
            });
            FindPersonCommand = new RelayCommand(_ =>
            {
                (_personViewModel.Value).LoadPerson(FindName ?? string.Empty);
                ChangeCurrentView(_personViewModel.Value);
                FindName = string.Empty;
            });
            ChangeMenuCommand = new RelayCommand(_ => IsMenuVisible = !IsMenuVisible);
            ChangeWindowSizeCommand = new RelayCommand(_ => Radius = StaticData.MAIN_WINDOW_RADIUS - Radius);
            FindNameCommand = new RelayCommand(x => FindName = (string)x);

            _navigationService.CloseAnotherWindow<MainWindow>();

            LoadRoles();
        }

        public ImageSource? CloseImage
        {
            get => _closeImage;
            set
            {
                _closeImage = value;
                OnPropertyChanged();
            }
        }

        public ImageSource? FullSizeImage
        {
            get => _fullSizeImage;
            set
            {
                _fullSizeImage = value;
                OnPropertyChanged();
            }
        }

        public object CurrentView
        {
            get => _currentView; 
            set 
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public bool IsMenuVisible
        {
            get => _isMenuVisible;
            set
            {
                _isMenuVisible = value;
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

        public int Radius
        {
            get => _radius;
            set
            {
                _radius = value;
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

        public void ChangeCurrentView(object currentView)
        {
            CurrentView = currentView;
            IsHome = currentView is HomeViewModel;
            IsDiscover = currentView is DiscoverViewModel;
        }

        public void LoadRoles()
        {
            _roles = _jwtService.GetRole();

            IsAddVisible = _roles.Contains("Admin");
        }
    }
}
