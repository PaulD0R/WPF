using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace WPFTest.Behaviors
{
    public class ValueChangedBehavior : Behavior<IntegerUpDown>
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
        "Command",
        typeof(ICommand),
        typeof(ValueChangedBehavior));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ValueChanged += OnValueChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ValueChanged -= OnValueChanged;
            base.OnDetaching();
        }

        private void OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (Command?.CanExecute(AssociatedObject.Value) == true)
            {
                Command.Execute(AssociatedObject.Value);
            }
        }
    }
}
