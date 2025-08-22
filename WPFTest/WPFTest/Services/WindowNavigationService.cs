using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WPFTest.Services.Interfaces;

namespace WPFTest.Services
{
    class WindowNavigationService : IWindowNavigationService
    {
        public void ShowWindow<T>() where T : Window
        {
            var window = App.ServiceProvider?.GetRequiredService<T>();
            window?.Show();
        }

        public void CloseAnotherWindow<T>() where T : Window
        {
            foreach (var window in Application.Current.Windows.OfType<Window>().Where(w => w.IsActive && !(w is T)))
            {
                window.Close();
            }
        }

        public void ShowAndHideAnotherWindow<T>() where T : Window
        {
            var windows = Application.Current.Windows.OfType<Window>();

            ShowWindow<T>();

            foreach (var window in windows)
            {
                window.Close();
            }
        }
    }
}
