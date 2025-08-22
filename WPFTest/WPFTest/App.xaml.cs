using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WPFTest.ApiServices;
using WPFTest.MVVM.ViewModel;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services;
using WPFTest.Services.Interfaces;

namespace WPFTest
{
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                var services = new ServiceCollection();
                ConfigureServices(services);
                ServiceProvider = services.BuildServiceProvider();

                var authenticationWindow = ServiceProvider.GetRequiredService<AuthenticationWindow>();
                authenticationWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка инициализации:\n{ex.ToString()}",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ApiAuthenticationService>();
            services.AddTransient<ApiExerciseService>();
            services.AddTransient<ApiSubjectService>();
            services.AddTransient<ApiPersonService>();

            services.AddTransient<IWindowNavigationService, WindowNavigationService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<ICheckCorrectServise, CheckCorrectService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IModelNavigationService, ModelNavigationService>();

            services.AddTransient<IMainViewModel, MainViewModel>();
            services.AddTransient<IAuthenticationViewModel, AuthenticationViewModel>(); 
            services.AddTransient<IHomeViewModel, HomeViewModel>();
            services.AddTransient<IDiscoverViewModel, DiscoverViewModel>();
            services.AddTransient<INewExerciseViewModel, NewExerciseViewModel>();
            services.AddTransient<INewSubjectViewModel, NewSubjectViewModel>();
            services.AddTransient<ISignupViewModel, SignupViewModel>();
            services.AddTransient<ISigninViewModel, SigninViewModel>();
            services.AddTransient<IExerciseViewModel, ExerciseViewModel>();
            services.AddTransient<ISubjectViewModel, SubjectViewModel>();
            services.AddTransient<IPersonViewModel, PersonViewModel>();
            services.AddTransient<IErrorViewModel, ErrorViewModel>();

            services.AddTransient<MainWindow>();
            services.AddTransient<AuthenticationWindow>();
        }
    }
}