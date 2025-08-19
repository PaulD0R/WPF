using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WPFTest.ApiServices;
using WPFTest.MVVM.View;
using WPFTest.MVVM.ViewModel;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services;
using WPFTest.Services.Interfaces;

namespace WPFTest
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLazy<T>(this IServiceCollection services) where T : class
        {
            services.AddTransient(provider => new Lazy<T>(() => provider.GetRequiredService<T>()));
            return services;
        }
    }

    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            var authenticationWindow = ServiceProvider.GetRequiredService<AuthenticationWindow>();
            authenticationWindow.Show();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ApiAuthenticationService>();
            services.AddScoped<ApiExerciseService>();
            services.AddScoped<ApiSubjectService>();
            services.AddScoped<ApiPersonService>();

            services.AddTransient<INavigationService, NavigationService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<ICheckCorrectServise, CheckCorrectService>();

            services.AddSingleton<IHomeViewModel, HomeViewModel>();
            services.AddSingleton<IDiscoverViewModel, DiscoverViewModel>();
            services.AddSingleton<IMainViewModel, MainViewModel>(); 
            services.AddSingleton<INewExerciseViewModel, NewExerciseViewModel>();
            services.AddSingleton<INewSubjectViewModel, NewSubjectViewModel>();
            services.AddSingleton<IAuthenticationViewModel, AuthenticationViewModel>(); 
            services.AddSingleton<ISignupViewModel, SignupViewModel>();
            services.AddSingleton<ISigninViewModel, SigninViewModel>();
            services.AddSingleton<IExerciseViewModel, ExerciseViewModel>();
            services.AddSingleton<ISubjectViewModel, SubjectViewModel>();
            services.AddSingleton<IPersonViewModel, PersonViewModel>();
            services.AddSingleton<IErrorViewModel, ErrorViewModel>();

            services.AddLazy<IHomeViewModel>();
            services.AddLazy<IDiscoverViewModel>();
            services.AddLazy<IExerciseViewModel>();
            services.AddLazy<INewExerciseViewModel>();
            services.AddLazy<INewSubjectViewModel>();
            services.AddLazy<ISubjectViewModel>();
            services.AddLazy<IMainViewModel>();
            services.AddLazy<IPersonViewModel>();
            services.AddLazy<IErrorViewModel>();
            services.AddLazy<IAuthenticationViewModel>();
            services.AddLazy<ISigninViewModel>();
            services.AddLazy<ISignupViewModel>();

            services.AddTransient<MainWindow>();
            services.AddTransient<AuthenticationWindow>();

            services.AddTransient<HomeView>();
            services.AddTransient<DiscoverView>();
            services.AddTransient<NewExercisesView>();
            services.AddTransient<NewSubjectView>();
            services.AddTransient<SigninView>();
            services.AddTransient<SignupView>();
            services.AddTransient<ExerciseView>();
            services.AddTransient<SubjectView>();
            services.AddTransient<PersonView>();
            services.AddTransient<ErrorView>();
        }
    }
}