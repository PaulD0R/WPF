using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace WPFTest.Behaviors
{
    public class PasswordBoxBehavior : Behavior<PasswordBox>
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                nameof(Password),
                typeof(string),
                typeof(PasswordBoxBehavior),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPasswordChanged));

        private bool _isUpdating;

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PasswordChanged -= OnPasswordChanged;
            base.OnDetaching();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_isUpdating) return;

            _isUpdating = true;
            Password = AssociatedObject.Password;
            _isUpdating = false;
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (PasswordBoxBehavior)d;

            if (behavior._isUpdating ||
                behavior.AssociatedObject == null ||
                behavior.AssociatedObject.Password == e.NewValue?.ToString())
            {
                return;
            }

            behavior._isUpdating = true;

            try
            {
                var passwordBox = behavior.AssociatedObject;
                var newPassword = e.NewValue?.ToString() ?? string.Empty;

                passwordBox.Password = newPassword;
            }
            finally
            {
                behavior._isUpdating = false;
            }
        }
    }
}