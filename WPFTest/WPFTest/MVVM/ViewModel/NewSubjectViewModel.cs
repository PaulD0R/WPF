using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Subject;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class NewSubjectViewModel : ObserverItem, INewSubjectViewModel
    {
        private readonly ApiSubjectService _apiSubjectService;

        private IMainViewModel _mainViewModel;

        private string? _name = string.Empty;
        private int? _year = 1;
        private string? _description = string.Empty;
        private bool? _isError = false;
        private string? _errorText = string.Empty;

        public ICommand SaveCommand { get; set; }

        public NewSubjectViewModel(ApiSubjectService apiSubjectService, IMainViewModel mainViewModel)
        {
            _apiSubjectService = apiSubjectService;

            _mainViewModel = mainViewModel;

            SaveCommand = new AsyncRelayCommand(async _ => await CreateNewSubject());
        }

        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
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

        public string? Description
        {
            get => _description;
            set
            {
                _description = value;
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

        public async Task CreateNewSubject()
        {
            try
            {
                if (Name != string.Empty && Year != null && Description != string.Empty)
                {
                    var subject = new NewSubject
                    {
                        Name = Name,
                        Year = Year,
                        Description = Description
                    };

                    if (!await _apiSubjectService.AddSubjectAsync(subject))
                    {
                        ErrorText = "Неудалось создать предмет";
                        IsError = true;
                        return;
                    }

                    Name = string.Empty;
                    Year = 1;
                    Description = string.Empty;
                    IsError = false;
                }
                else
                {
                    ErrorText = "Заполните все поля";
                    IsError = true;
                }
            }
            catch (ApiExeption ex)
            {
                _mainViewModel.OpenErrorView(ex.Message);
            }
        }
    }
}
