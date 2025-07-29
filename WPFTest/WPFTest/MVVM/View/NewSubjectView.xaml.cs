using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.View
{
    /// <summary>
    /// Interaction logic for NewSubjectView.xaml
    /// </summary>
    public partial class NewSubjectView : UserControl
    {
        public NewSubjectView()
        {
            DataContext = App.ServiceProvider?.GetRequiredService<INewSubjectViewModel>();
            InitializeComponent();
        }
    }
}
