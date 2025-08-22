using System.Windows;

namespace WPFTest.Services.Interfaces
{
    public interface IWindowNavigationService
    {
        void ShowWindow<T>() where T : Window;
        void CloseAnotherWindow<T>() where T : Window;
        void ShowAndHideAnotherWindow<T>() where T : Window;
    }
}
