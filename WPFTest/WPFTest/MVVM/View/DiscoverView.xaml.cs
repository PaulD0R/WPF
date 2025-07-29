using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.View
{
    /// <summary>
    /// Interaction logic for DiscoverView.xaml
    /// </summary>
    public partial class DiscoverView : UserControl
    {
        public DiscoverView()
        {
            DataContext = App.ServiceProvider?.GetRequiredService<IDiscoverViewModel>();
            InitializeComponent();
        }
    }
}
