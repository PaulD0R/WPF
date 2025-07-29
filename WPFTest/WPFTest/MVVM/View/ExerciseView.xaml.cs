using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.View
{
    /// <summary>
    /// Interaction logic for ExerciseView.xaml
    /// </summary>
    public partial class ExerciseView : UserControl
    {
        public ExerciseView()
        {
            DataContext = App.ServiceProvider?.GetRequiredService<IExerciseViewModel>();
            InitializeComponent();
        }
    }
}
