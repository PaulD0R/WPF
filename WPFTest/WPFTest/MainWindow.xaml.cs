using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CornerRadius MAX_RADIUS = new(0);
        private CornerRadius MIN_RADIUS = new(15);

        private bool _isMenuVisability = true;
        public MainWindow()
        {
            DataContext = App.ServiceProvider?.GetRequiredService<IMainViewModel>();
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChangeSizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                MainBorder.CornerRadius = MAX_RADIUS;
                WindowState = WindowState.Maximized;
            }
            else
            {
                MainBorder.CornerRadius = MIN_RADIUS;
                WindowState = WindowState.Normal;
            }
        }

        private void ChangeMenuButton_Click(Object sender, RoutedEventArgs e)
        {
            _isMenuVisability = !_isMenuVisability;

            MenuBorder.Visibility = _isMenuVisability ? Visibility.Visible : Visibility.Hidden;
            MenuStack.Visibility = _isMenuVisability ? Visibility.Visible : Visibility.Hidden;

            Menu.Width = _isMenuVisability ? 
                new GridLength(12, GridUnitType.Star) : new GridLength(0);
        }
    }
}