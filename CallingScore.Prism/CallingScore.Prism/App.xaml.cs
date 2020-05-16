using Prism;
using Prism.Ioc;
using CallingScore.Prism.ViewModels;
using CallingScore.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.Licensing;
using CallingScore.Common.Services;
using CallingScore.Common.Helpers;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CallingScore.Prism
{
    public partial class App
    {

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("MjUzNzIyQDMxMzgyZTMxMmUzME4vZWNpMVo5VGs0cDlzUjlnYUtzSFZDdDlwZjJsMXVRRkVpYm9XOE9BUmM9");
            InitializeComponent();
            if (Settings.IsLogin)
            {
                await NavigationService.NavigateAsync("/CallingScoreMasterDetailPage/NavigationPage/HomePage");
                return;
            }
            await NavigationService.NavigateAsync("CallingScoreMasterDetailPage/NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IRegexHelper, RegexHelper>();
            containerRegistry.Register<IFilesHelper, FilesHelper>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<RememberPasswordPage, RememberPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<CallingScoreMasterDetailPage, CallingScoreMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<ShowStatisticsPage, ShowStatisticsPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<StatisticsByUserPage, StatisticsByUserPageViewModel>();
        }
    }
}
