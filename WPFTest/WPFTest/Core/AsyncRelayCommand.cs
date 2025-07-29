using System.Windows.Input;

namespace WPFTest.Core
{
    class AsyncRelayCommand(Func<object, Task> execute, Func<object, bool>? canExecute = null) : ICommand
    {
        private Func<object, Task> _execute = execute;
        private Func<object, bool>? _canExecute = canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;  
        }

        public async void Execute(object? parameter)
        {
            await _execute(parameter).ConfigureAwait(true);
        }
    }
}
