namespace WPFTest.Services.Interfaces
{
    public interface IModelNavigationService
    {
        object? CurrentView { get; }
        event EventHandler<object>? NavigationChanged;

        void NavigateTo<TViewModel>(Action<TViewModel>? initialization = null) where TViewModel : class;
    }
}
