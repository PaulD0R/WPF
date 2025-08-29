using WPFTest.Core;
using System.Collections.ObjectModel;
using WPFTest.Data;
using System.Windows.Input;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.Exeptions;
using WPFTest.Services.Interfaces;
using WPFTest.ApiServices.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class DiscoverViewModel : ObserverItem, IDiscoverViewModel
    {
        private readonly IApiExerciseService _exerciseService;
        private readonly INavigationService _navigationService;

        private ObservableCollection<LiteExercise> _exercises = [];
        private List<PageButtonData> _pages = [];
        private int _pageNumber = 1;

        public ICommand ChangePageCommand { get; }
        public ICommand ExerciseViewCommand { get; }
        public ICommand ChangeIsLikedCommand { get; }

        public DiscoverViewModel(IApiExerciseService exerciseService, INavigationService navigationService)
        {
            _exerciseService = exerciseService;
            _navigationService = navigationService;

            ChangePageCommand = new AsyncRelayCommand(async x => await ChangePage((int)x));
            ExerciseViewCommand = new AsyncRelayCommand(async x => await OpenExerciseById((int) x));
            ChangeIsLikedCommand = new AsyncRelayCommand(async x => await ChangeIsLiked((int)x));
        }


        public ObservableCollection<LiteExercise> Exercises
        {
            get => _exercises;
            set
            {
                _exercises = value;
                OnPropertyChanged();
            }
        }

        public List<PageButtonData> Pages
        {
            get => _pages;
            set
            {
                _pages = value;
                OnPropertyChanged();
            }
        }

        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                _pageNumber = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadExercises()
        {
            var exercises = await _exerciseService.GetByPageAsync(_pageNumber);
            Exercises = new ObservableCollection<LiteExercise>(exercises);
        }

        public async Task LoadPages()
        {
            var pageCount = (double)await _exerciseService.GetCountAsync()
                / (double)StaticData.NUMBER_OF_ELEMENTS_PER_PAGE;
            var pages = new List<PageButtonData>();

            if (pageCount > 0)
            {
                for (var i = 0; i < Math.Ceiling(pageCount); i++)
                {
                    pages.Add(new PageButtonData(i + 1, false));
                }

                pages[PageNumber - 1].IsChecked = true;
            }

            Pages = pages;
        }
        private async Task ChangePage(int number)
        {
            PageNumber = number;
            await LoadExercises();
        }

        private async Task ChangeIsLiked(int id)
        {
            try
            {
                await _exerciseService.ChangeIsLikedAsync(id);
            } 
            catch
            {
                return;
            }
        }

        private async Task OpenExerciseById(int id)
        {
            try
            {
                await _navigationService.NavigateToAsync<IExerciseViewModel>(async x => await x.LoadExercise(id));
            }
            catch (ApiException ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }
    }
}
