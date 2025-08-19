using System.Windows;

namespace WPFTest.Services.Interfaces
{
    public interface INavigationService
    {
        void ShowWindow<T>() where T : Window;
        void CloseAnotherWindow<T>() where T : Window;
        void ShowAndClothesAnotherWindow<T>() where T : Window;
    }
}
