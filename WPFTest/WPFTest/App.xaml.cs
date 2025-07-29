using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WPFTest.ApiServices;
using WPFTest.Context;
using WPFTest.Data;
using WPFTest.MVVM.View;
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
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            //var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            //mainWindow.Show();

            var authenticationWindow = ServiceProvider.GetRequiredService<AuthenticationWindow>();
            authenticationWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            //Api context
            services.AddDbContext<ApplicationContext>(option =>
                option.UseSqlServer(StaticData.EXERCISE_ROUDE)
            );
            services.AddSingleton(_ =>
                new ApiExerciseService(StaticData.EXERCISE_ROUDE)
            );
            services.AddSingleton(_ =>
                new ApiSubjectService(StaticData.SUBJECT_ROUDE)
            );
            services.AddSingleton(_ =>
                new ApiAuthenticationService(StaticData.AUTHENTICATION_ROUDE)
            );
            services.AddSingleton(_ =>
                new ApiPersonService(StaticData.PERSON_ROUDE)
            );

            services.AddTransient<INavigationService, NavigationService>();

            //ViewModels
            services.AddSingleton<IHomeViewModel, HomeViewModel>();
            services.AddSingleton<IDiscoverViewModel, DiscoverViewModel>();
            services.AddSingleton<IMainViewModel, MainViewModel>();
            services.AddSingleton<INewExerciseViewModel, NewExerciseViewModel>();
            services.AddSingleton<INewSubjectViewModel, NewSubjectViewModel>();
            services.AddSingleton<IAuthenticationViewModel, AuthenticationViewModel>();
            services.AddSingleton<ISignupViewModel, SignupViewModel>();
            services.AddSingleton<ISigninViewModel, SigninViewModel>(); 
            services.AddScoped<IExerciseViewModel, ExerciseViewModel>();
            services.AddSingleton<ISubjectViewModel, SubjectViewModel>();

            //Lazy ViewModels
            services.AddSingleton(provider =>
                new Lazy<IHomeViewModel>(() => provider.GetRequiredService<IHomeViewModel>())
            );
            services.AddSingleton(provider =>
                new Lazy<IDiscoverViewModel>(() => provider.GetRequiredService<IDiscoverViewModel>())
            );
            services.AddSingleton(provider =>
                new Lazy<IExerciseViewModel>(() => provider.GetRequiredService<IExerciseViewModel>())
            );
            services.AddSingleton(provider =>
                new Lazy<INewExerciseViewModel>(() => provider.GetRequiredService<INewExerciseViewModel>())
            );
            services.AddSingleton(provider =>
                new Lazy<INewSubjectViewModel>(() => provider.GetRequiredService<INewSubjectViewModel>())
            );
            services.AddSingleton(provider =>
                new Lazy<ISubjectViewModel>(() => provider.GetRequiredService<ISubjectViewModel>())
            );
            services.AddSingleton(provider =>
                new Lazy<IMainViewModel>(() => provider.GetRequiredService<IMainViewModel>())
            );

            //View
            services.AddTransient<HomeView>();
            services.AddTransient<DiscoverView>();
            services.AddTransient<MainWindow>();
            services.AddTransient<NewExercisesView>();
            services.AddTransient<NewSubjectView>();
            services.AddTransient<AuthenticationWindow>();
            services.AddTransient<SigninView>();
            services.AddTransient<SignupView>();
            services.AddTransient<ExerciseView>();
            services.AddTransient<SubjectView>();
        }
    }
}