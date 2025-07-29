using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.MVVM.Model.Files;
using WPFTest.MVVM.Model.Subject;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class NewSubjectViewModel : ObserverItem, INewSubjectViewModel
    {
        private readonly ApiSubjectService _apiSubjectService;

        private string? _name;
        private int? _year;
        private string? _description;

        public ICommand SaveCommand { get; set; }
        public ICommand NameCommand { get; set; }
        public ICommand YearCommand { get; set; }
        public ICommand DescriptionCommand { get; set; }

        public NewSubjectViewModel(ApiSubjectService apiSubjectService)
        {
            _apiSubjectService = apiSubjectService;

            Name = string.Empty;
            Year = 1;
            Description = string.Empty;

            NameCommand = new RelayCommand(x => Name = (string)x);
            YearCommand = new RelayCommand(x => Year = (int)x); 
            DescriptionCommand = new RelayCommand(x => Description = (string)x);
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

        public async Task CreateNewSubject()
        {
            if (Name != string.Empty && Year != null && Description != string.Empty)
            {
                var subject = new NewSubject
                {
                    Name = Name,
                    Year = Year,
                    Description = Description
                };

                var error = await _apiSubjectService.AddSubjectAsync(subject);
                Console.WriteLine(error);

                Name = string.Empty;
                Year = 1;
                Description = string.Empty;
            }
        }
    }
}
