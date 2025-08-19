using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
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

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                var desiredWidth = textBlock.DesiredSize.Width;
                var parentCanvas = VisualTreeHelper.GetParent(textBlock) as Canvas;

                if (parentCanvas != null && desiredWidth > parentCanvas.ActualWidth)
                {
                    var storyboard = textBlock.FindResource("scrollStoryboard") as Storyboard;
                    storyboard?.Begin();
                }
            }
        }
    }
}
