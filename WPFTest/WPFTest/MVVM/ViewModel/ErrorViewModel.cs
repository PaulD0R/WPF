using WPFTest.Core;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class ErrorViewModel : ObserverItem, IErrorViewModel
    {
        private string? _text = string.Empty;

        public ErrorViewModel()
        {
        }

        public string? Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public void LoadError(string error)
        {
            Text = error;
        }
    }
}
