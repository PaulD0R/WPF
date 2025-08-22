using Microsoft.Extensions.DependencyInjection;
using WPFTest.Services.Interfaces;

namespace WPFTest.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly Stack<NavigationState> _navigationStack;
        private object? _currentView;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigationStack = new Stack<NavigationState>();
        }

        public event EventHandler<object>? NavigationChanged;

        public object? CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                NavigationChanged?.Invoke(null, null);
            }
        }

        public void NavigateTo<TViewModel>(Action<TViewModel>? initialization = null) where TViewModel : class
        {
            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            initialization?.Invoke(viewModel);

            if (CurrentView != null)
            {
                _navigationStack.Push(new NavigationState(CurrentView));
            }

            CurrentView = viewModel;
        }

        public bool CanNavigateBack()
        {
            return _navigationStack.Count > 0;
        }

        public void NavigateBack()
        {
            if (!CanNavigateBack())
                return;

            var previousState = _navigationStack.Pop();
            CurrentView = previousState.ViewModel;
        }

        public void ClearHistory()
        {
            _navigationStack.Clear();
        }

        private class NavigationState
        {
            public object ViewModel { get; }

            public NavigationState(object viewModel)
            {
                ViewModel = viewModel;
            }
        }
    }
}
