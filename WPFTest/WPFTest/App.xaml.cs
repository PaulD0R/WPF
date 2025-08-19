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

            services.AddTransient<IHomeViewModel, HomeViewModel>();
            services.AddTransient<IDiscoverViewModel, DiscoverViewModel>();
            services.AddSingleton<IMainViewModel, MainViewModel>(); 
            services.AddTransient<INewExerciseViewModel, NewExerciseViewModel>();
            services.AddTransient<INewSubjectViewModel, NewSubjectViewModel>();
            services.AddTransient<IAuthenticationViewModel, AuthenticationViewModel>(); 
            services.AddTransient<ISignupViewModel, SignupViewModel>();
            services.AddTransient<ISigninViewModel, SigninViewModel>();
            services.AddSingleton<IExerciseViewModel, ExerciseViewModel>();
            services.AddSingleton<ISubjectViewModel, SubjectViewModel>();
            services.AddTransient<IPersonViewModel, PersonViewModel>();
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

            services.AddSingleton<MainWindow>();
            services.AddSingleton<AuthenticationWindow>();

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