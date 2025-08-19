using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.View
{
    /// <summary>
    /// Interaction logic for ErrorView.xaml
    /// </summary>
    public partial class ErrorView : UserControl
    {
        public ErrorView()
        {
            DataContext = App.ServiceProvider?.GetRequiredService<IErrorViewModel>();
            InitializeComponent();
        }
    }
}
