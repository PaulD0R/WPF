using Microsoft.Extensions.DependencyInjection;
using WPFTest.Services.Interfaces;

namespace WPFTest.Services
{
    public class ModelNavigationService : IModelNavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private object? _currentView;

        public ModelNavigationService(IServiceProvider serviceProvider)
        {
             _serviceProvider = serviceProvider;
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

            CurrentView = viewModel;
        }
    }
}
