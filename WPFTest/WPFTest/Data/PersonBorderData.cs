using System.Collections.ObjectModel;
using WPFTest.Core;
using WPFTest.MVVM.Model.Comments;

namespace WPFTest.Data
{
    public class PersonBorderData : ObserverItem
    {
        private string? _id;
        private string? _name;
        private string? _email;
        private bool _isVisibleComments = false;
        private ObservableCollection<LiteComment>? _comments;
        private bool _isError = false;
        private string? _errorText;

        public string? Id 
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            } 
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

        public string? Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public bool IsVisibleComments
        {
            get => _isVisibleComments;
            set
            {
                _isVisibleComments = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<LiteComment>? Comments
        {
            get => _comments;
            set
            {
                _comments = value;
                OnPropertyChanged();
            }
        }
        public bool IsError
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
    }
}
