using System.Windows;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    /// <summary>
    /// Interaction logic for AuthenticationWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow(IAuthenticationViewModel authenticationViewModel)
        {
            DataContext = authenticationViewModel;
            InitializeComponent();
        }
    }
}
