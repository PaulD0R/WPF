using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Windows.Data;
using WPFTest.Data;

namespace WPFTest.Converters
{
    public class ViewModelConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var serviceProvider = App.ServiceProvider ??
                    throw new InvalidOperationException("ServiceProvider not found in application resources");
                using var scope = serviceProvider.CreateScope();
                var scopedProvider = scope.ServiceProvider;

                if (value is Type viewModelType)
                    return scopedProvider.GetService(viewModelType);

                if (value is ViewModelRequest request)
                    return CreateViewModelWithParameters(request, scopedProvider);

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ViewModelConverter error: {ex.Message}");
                return null;
            }
        }

        private object CreateViewModelWithParameters(ViewModelRequest request, IServiceProvider serviceProvider)
        {
            if (request.Parameters == null || request.Parameters.Length == 0)
                return serviceProvider.GetService(request.ViewModelType);

            return ActivatorUtilities.CreateInstance(serviceProvider, request.ViewModelType, request.Parameters);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ViewModel conversion is one-way only");
        }
    }
}