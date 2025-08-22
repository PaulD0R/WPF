using System.Windows.Input;
using WPFTest.Core;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class MainViewModel : ObserverItem, IMainViewModel, IDisposable
    {
        private readonly IJwtService _jwtService;
        private readonly INavigationService _navigationService;

        private ICollection<string>? _roles = [];
        private bool? _isDiscover = false;
        private bool? _isHome = true;
        private bool? _isAddVisible = false;
        private string? _findName = string.Empty;

        public ICommand HomeCommand { get; }
        public ICommand DiscoverCommand { get; }
        public ICommand NewExercisesCommand { get; }
        public ICommand FindPersonCommand { get; }
        public ICommand BackCommand { get; }

        public MainViewModel(IJwtService jwtService, INavigationService navigationService)
        {
            _jwtService = jwtService;
            _navigationService = navigationService;

            HomeCommand = new RelayCommand(_ => OpenHome());
            DiscoverCommand = new RelayCommand(_ => OpenDiscover());
            NewExercisesCommand = new RelayCommand(_ => OpenNewExercise());
            FindPersonCommand = new RelayCommand(_ => OpenPerson());
            BackCommand = new RelayCommand(_ => Back());

            _navigationService.ClearHistory();
            _navigationService.NavigationChanged += ChangeCurrentView;

            LoadRoles();
        }

        public object? CurrentView { get; set; }

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
            get => _isAddVisible;
            set
            {
                _isAddVisible = value;
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

        private void OpenHome()
        {
            _navigationService.NavigateTo<IHomeViewModel>();
        }

        private void OpenDiscover()
        {
            _navigationService.NavigateTo<IDiscoverViewModel>(x =>
            {
                x.LoadExercises();
                x.LoadPages();
            });
        }

        private void OpenNewExercise()
        {
            _navigationService.NavigateTo<INewExerciseViewModel>();
        }

        private void OpenPerson()
        {
            _navigationService.NavigateTo<IPersonViewModel>(x => x.LoadPerson(FindName ?? string.Empty));
            FindName = string.Empty;
        }

        private void Back()
        {
            _navigationService.NavigateBack();
        }

        private void ChangeCurrentView(object sender, object value)
        {
            CurrentView = _navigationService.CurrentView;
            OnPropertyChanged(nameof(CurrentView));

            IsHome = CurrentView is IHomeViewModel;
            IsDiscover = CurrentView is IDiscoverViewModel;
        }

        private void LoadRoles()
        {
            _roles = _jwtService.GetRole();
            IsAddVisible = _roles.Contains("Admin");
        }

        public void Dispose()
        {
            _navigationService.NavigationChanged -= ChangeCurrentView;
        }
    }
}
