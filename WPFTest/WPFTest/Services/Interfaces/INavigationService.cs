namespace WPFTest.Services.Interfaces
{
    public interface INavigationService
    {
        object? CurrentView { get; }
        event EventHandler<object>? NavigationChanged;

        void NavigateTo<TViewModel>(Action<TViewModel>? initialization = null) where TViewModel : class;
        Task NavigateToAsync<TViewModel>(Func<TViewModel, Task>? initialization = null) where TViewModel : class;
        bool CanNavigateBack();
        void NavigateBack();
        void ClearHistory();
    }
}
