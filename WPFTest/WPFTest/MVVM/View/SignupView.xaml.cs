using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.View
{
    /// <summary>
    /// Interaction logic for SigninView.xaml
    /// </summary>
    public partial class SigninView : UserControl
    {
        public SigninView()
        {
            DataContext = App.ServiceProvider?.GetRequiredService<ISignupViewModel>();
            InitializeComponent();
        }
    }
}
