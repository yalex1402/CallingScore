using CallingScore.Common.Helpers;
using CallingScore.Common.Models;
using CallingScore.Common.Services;
using CallingScore.Prism.Helpers;
using CallingScore.Prism.Views;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IFacebookClient _facebookService = CrossFacebookClient.Current;
        private bool _isRunning;
        private bool _isEnabled;
        private string _password;
        private DelegateCommand _loginCommand;
        private DelegateCommand _registerCommand;
        private DelegateCommand _forgotPasswordCommand;
        private DelegateCommand _loginFacebookCommand;

        public LoginPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Login;
            IsEnabled = true;
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(LoginAsync));

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));

        public DelegateCommand ForgotPasswordCommand => _forgotPasswordCommand ?? (_forgotPasswordCommand = new DelegateCommand(ForgotPasswordAsync));

        public DelegateCommand LoginFacebookCommand => _loginFacebookCommand ?? (_loginFacebookCommand = new DelegateCommand(LoginFacebookAsync));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private async void LoginAsync()
        {
            bool IsValid = ValidateData();
            if (!IsValid)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorEmptyField, Languages.Accept);
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.InternetConnection, Languages.Accept);
                return;
            }

            TokenRequest request = new TokenRequest
            {
                Password = Password,
                UserName = Email
            };

            Response response = await _apiService.GetTokenAsync(url, "Account", "/CreateToken", request);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorLogin, Languages.Accept);
                Password = string.Empty;
                return;
            }

            TokenResponse token = (TokenResponse)response.Result;
            EmailRequest emailRequest = new EmailRequest
            {
                CultureInfo = Languages.Culture,
                Email = Email
            };

            Response response2 = await _apiService.GetUserByEmail(url, "api", "/Account/GetUser", "bearer", token.Token, emailRequest);
            UserResponse userResponse = (UserResponse)response2.Result;

            Settings.User = JsonConvert.SerializeObject(userResponse);
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsLogin = true;

            IsRunning = false;
            IsEnabled = true;

            await _navigationService.NavigateAsync("/CallingScoreMasterDetailPage/NavigationPage/HomePage");

            Password = string.Empty;

        }

        private async void LoginFacebookAsync()
        {
            try
            {

                if (_facebookService.IsLoggedIn)
                {
                    _facebookService.Logout();
                }

                async void userDataDelegate(object sender, FBEventArgs<string> e)
                {
                    switch (e.Status)
                    {
                        case FacebookActionStatus.Completed:
                            FacebookProfile facebookProfile = await Task.Run(() => JsonConvert.DeserializeObject<FacebookProfile>(e.Data));
                            await LoginFacebookAsync(facebookProfile);
                            break;
                        case FacebookActionStatus.Canceled:
                            await App.Current.MainPage.DisplayAlert(Languages.FBAuthorization, Languages.Canceled, Languages.Ok);
                            break;
                        case FacebookActionStatus.Error:
                            await App.Current.MainPage.DisplayAlert(Languages.FBAuthorization, Languages.Error, Languages.Ok);
                            break;
                        case FacebookActionStatus.Unauthorized:
                            await App.Current.MainPage.DisplayAlert(Languages.FBAuthorization, Languages.Unauthorized, Languages.Ok);
                            break;
                    }

                    _facebookService.OnUserData -= userDataDelegate;
                }

                _facebookService.OnUserData += userDataDelegate;

                string[] fbRequestFields = { "email", "first_name", "picture.width(999)", "gender", "last_name" };
                string[] fbPermisions = { "email" };
                await _facebookService.RequestUserDataAsync(fbRequestFields, fbPermisions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private async void RegisterAsync()
        {
            await _navigationService.NavigateAsync(nameof(RegisterPage));
        }

        private async void ForgotPasswordAsync()
        {
            await _navigationService.NavigateAsync(nameof(RememberPasswordPage));
        }

        private bool ValidateData()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                return false;
            }
            return true;
        }

        private async Task LoginFacebookAsync(FacebookProfile facebookProfile)
        {
            IsRunning = true;
            IsEnabled = false;

            string url = App.Current.Resources["UrlAPI"].ToString();

            Response response = await _apiService.GetTokenAsync(url, "api", "/Account/LoginFacebook", facebookProfile);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorLogin, Languages.Accept);
                Password = string.Empty;
                return;
            }

            TokenResponse token = (TokenResponse)response.Result;
            EmailRequest request2 = new EmailRequest
            {
                CultureInfo = Languages.Culture,
                Email = facebookProfile.Email
            };

            Response response2 = await _apiService.GetUserByEmail(url, "api", "/Account/GetUser", "bearer", token.Token, request2);
            UserResponse userResponse = (UserResponse)response2.Result;

            Settings.User = JsonConvert.SerializeObject(userResponse);
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsLogin = true;

            IsRunning = false;
            IsEnabled = true;

            await _navigationService.NavigateAsync("/CallingScoreMasterDetailPage/NavigationPage/HomePage");
            Password = string.Empty;
        }

    }
}
