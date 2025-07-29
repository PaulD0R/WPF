using System.Windows;
using WPFTest.Services.Interfaces;

namespace WPFTest.Services
{
    class NavigationService : INavigationService
    {
        public void ShowWindow<T>() where T : Window
        {
            var window = Activator.CreateInstance<T>();
            window.Show();
        }

        public void CloseAnotherWindow<T>() where T : Window
        {
            foreach (var window in Application.Current.Windows.OfType<Window>().Where(w => w.IsActive && !(w is T)))
            {
                window.Close();
            }
        }
    }
}
