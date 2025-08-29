using System.Windows.Input;
using WPFTest.Core;
using WPFTest.Exeptions;
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
        public ICommand AdminCommand { get; }
        public ICommand FindPersonCommand { get; }
        public ICommand BackCommand { get; }

        public MainViewModel(IJwtService jwtService, INavigationService navigationService)
        {
            _jwtService = jwtService;
            _navigationService = navigationService;

            HomeCommand = new AsyncRelayCommand(async _ => await OpenHome());
            DiscoverCommand = new AsyncRelayCommand(async _ => await OpenDiscover());
            NewExercisesCommand = new AsyncRelayCommand(async _ => await OpenNewExercise());
            AdminCommand = new AsyncRelayCommand(async _ => await OpenAdmin());
            FindPersonCommand = new AsyncRelayCommand(async _ => await OpenPerson());
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

        private async Task OpenHome()
        {
            try
            {
                await _navigationService.NavigateToAsync<IHomeViewModel>(async x => await x.LoadPerson());
            }
            catch (ApiException ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }

        private async Task OpenDiscover()
        {
            try
            {
                await _navigationService.NavigateToAsync<IDiscoverViewModel>(async x =>
                {
                    await x.LoadExercises();
                    await x.LoadPages();
                });
            }
            catch (ApiException ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }

        private async Task OpenNewExercise()
        {
            try
            {
                await _navigationService.NavigateToAsync<INewExerciseViewModel>(async x => await x.LoadSubjects());
            }
            catch (ApiException ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }

        private async Task OpenAdmin()
        {
            try
            {
                await _navigationService.NavigateToAsync<IAdminViewModel>(async x => await x.LoadUsers());
            }
            catch (ApiException ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }

        private async Task OpenPerson()
        {
            try
            {
                if (!string.IsNullOrEmpty(FindName))
                {
                    await _navigationService.NavigateToAsync<IPersonViewModel>(async x => await x.LoadPerson(FindName ?? string.Empty));
                    FindName = string.Empty;
                }
            }
            catch (ApiException ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
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
