using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFTest.ApiServices.Interfaces;
using WPFTest.Core;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Comments;
using WPFTest.MVVM.Model.Person;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class AdminViewModel : ObserverItem, IAdminViewModel
    {
        private readonly IApiAdminService _adminService;
        private readonly ICheckCorrectService _checkCorrectService;

        private ObservableCollection<PersonBorderData> _users = [];

        public ICommand GetCommentsCommand { get; }
        public ICommand ChangeRoleCommand { get; }
        public ICommand UpdateUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand DeleteCommentCommand { get; }

        public AdminViewModel(IApiAdminService adminService, ICheckCorrectService checkCorrectService)
        {
            _adminService = adminService;
            _checkCorrectService = checkCorrectService;

            GetCommentsCommand = new AsyncRelayCommand(async x => await GetComments((PersonBorderData)x));
            ChangeRoleCommand = new AsyncRelayCommand(async x => await ChangeRole((PersonBorderData)x));
            UpdateUserCommand = new AsyncRelayCommand(async x => await UpdateUser((PersonBorderData)x));
            DeleteUserCommand = new AsyncRelayCommand(async x => await DeleteUser((PersonBorderData)x));
            DeleteCommentCommand = new AsyncRelayCommand(async x => await DeleteComment((LiteComment)x));
        }

        public ObservableCollection<PersonBorderData> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadUsers()
        {
            var users = (await _adminService.GetUsersAsync()).Select(x => new PersonBorderData
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                IsVisibleComments = false,
                IsError = false
            });

            Users = new ObservableCollection<PersonBorderData>(users);
        }

        private async Task GetComments(PersonBorderData person)
        {
            try
            { 
                if (person.IsVisibleComments == true) person.IsVisibleComments = false;
                else
                {
                    var comments = await _adminService.GetCommentsAsync(person.Id);

                    person.Comments = new ObservableCollection<LiteComment>(comments);
                    person.IsVisibleComments = true;
                }
            }
            catch (ApiException ex)
            {
                person.ErrorText = ex.Message;
                person.IsError = true;
            }
        } 

        private async Task ChangeRole(PersonBorderData person)
        {
            try
            {
                if (!await _adminService.ChangeRoleAsync(person.Id, "Admin"))
                {
                    person.ErrorText = "Не удалось повысить";
                    person.IsError = true;

                    return;
                }

                await LoadUsers();
            }
            catch (ApiException ex)
            {
                person.ErrorText = ex.Message;
                person.IsError = true;
            }
        }

        private async Task UpdateUser(PersonBorderData person)
        {
            try
            {
                if (string.IsNullOrEmpty(person.Name) || string.IsNullOrEmpty(person.Email))
                {
                    person.ErrorText = "Заполните все поля";
                    person.IsError = true;

                    return;
                }
                if (!_checkCorrectService.IsEmail(person.Email))
                {
                    person.ErrorText = "Некорректная почта";
                    person.IsError = true;

                    return;
                }

                var updatePerson = new UpdatePerson
                {
                    Name = person.Name,
                    Email = person.Email
                };

                if (!await _adminService.UpdateUserAsync(person.Id, updatePerson))
                {
                    person.ErrorText = "Не удалось изменить";
                    person.IsError = true;

                    return;
                }

                person.ErrorText = string.Empty;
                person.IsError = false;
            }
            catch (ApiException ex)
            {
                person.ErrorText = ex.Message;
                person.IsError = true;
            }
        }

        private async Task DeleteUser(PersonBorderData person)
        {
            try
            {
                if (!await _adminService.DeleteUserAsync(person.Id)) 
                {
                    person.ErrorText = "Не удалось удалить";
                    person.IsError = true;

                    return;
                }

                await LoadUsers();
            }
            catch (ApiException ex)
            {
                person.ErrorText = ex.Message;
                person.IsError = true;
            }
        }

        private async Task DeleteComment(LiteComment comment)
        {
            var person = Users.First(x => x.Id == comment.PersonId);

            try
            {
                if (!await _adminService.DeleteCommentAsync(comment.Id))
                {
                    person.ErrorText = "Неудалось удалить";
                    person.IsError = true;
                }

                var comments = await _adminService.GetCommentsAsync(person.Id);

                person.Comments = new ObservableCollection<LiteComment>(comments);
            }
            catch (ApiException ex)
            {
                person.ErrorText = ex.Message;
                person.IsError = true;
            }
        }
    }
}
