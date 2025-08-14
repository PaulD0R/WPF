using WPFTest.Core;
using System.Collections.ObjectModel;
using WPFTest.Data;
using System.Windows.Input;
using WPFTest.FileStreamers;
using WPFTest.ApiServices;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.MVVM.Model.Exercise;

namespace WPFTest.MVVM.ViewModel
{
    public class DiscoverViewModel : ObserverItem, IDiscoverViewModel
    {
        private readonly ApiExerciseService _exerciseService;

        private readonly IMainViewModel _mainViewModel;
        private readonly Lazy<IExerciseViewModel> _exerciseViewModel;

        private ObservableCollection<LightExercise> _exercises;
        private List<PageButtonData> _pages;
        private int _pageNumer;

        public ICommand ChangePage { get; set; }
        public ICommand ExerciseViewCommand { get; set; }
        public ICommand ChangeIsLikedCommand { get; set; }

        public DiscoverViewModel(ApiExerciseService exerciseService, IMainViewModel mainViewModel, Lazy<IExerciseViewModel> exerciseViewModel)
        {
            _exerciseService = exerciseService;
            _mainViewModel = mainViewModel;
            _exerciseViewModel = exerciseViewModel;

            _pageNumer = 1;
            _pages = [];
            _exercises = [];

            ChangePage = new RelayCommand(x => {
                PageNumber = (int)x;
                LoadExercises();
            });
            ExerciseViewCommand = new RelayCommand(x => OpenExerciseById((int) x));
            ChangeIsLikedCommand = new AsyncRelayCommand(async x => await _exerciseService.ChangeIsLikedAsync((int)x));

            LoadExercises();
            LoadPages();
        }

        public ObservableCollection<LightExercise> Exercises
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
            get => _pageNumer;
            set
            {
                _pageNumer = value;
                OnPropertyChanged();
            }
        }

        public async void LoadExercises()
        {
            var exercises = await _exerciseService.GetByPageAsync(_pageNumer);
            Exercises = new ObservableCollection<LightExercise>(exercises);
        }

        public async void LoadPages()
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

        public void OpenExerciseById(int id)
        {
            (_exerciseViewModel.Value).LoadExercise(id);
            _mainViewModel.ChangeCurrentView(_exerciseViewModel.Value);
        }
    }
}
