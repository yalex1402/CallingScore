﻿using CallingScore.Common.Helpers;
using CallingScore.Common.Models;
using CallingScore.Common.Services;
using CallingScore.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CallingScore.Prism.ViewModels
{
    public class CallingScoreMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private static CallingScoreMasterDetailPageViewModel _instance;
        private UserResponse _user;
        private DelegateCommand _modifyUserCommand;

        public CallingScoreMasterDetailPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            _apiService = apiService;
            LoadUser();
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        public DelegateCommand ModifyUserCommand => _modifyUserCommand ?? (_modifyUserCommand = new DelegateCommand(ModifyUserAsync));

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        public static CallingScoreMasterDetailPageViewModel GetInstance()
        {
            return _instance;
        }

        public async void ReloadUser()
        {
            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                return;
            }

            UserResponse user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            EmailRequest emailRequest = new EmailRequest
            {
                CultureInfo = "es",
                Email = user.Email
            };

            Response response = await _apiService.GetUserByEmail(url, "api", "/Account/GetUser", "bearer", token.Token, emailRequest);
            UserResponse userResponse = (UserResponse)response.Result;
            Settings.User = JsonConvert.SerializeObject(userResponse);
            LoadUser();
        }


        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            }
        }

        private void LoadMenus()
        {
            List<Menu> menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_refresh",
                    PageName = "HomePage",
                    Title = "Home"
                },
                new Menu
                {
                    Icon = "ic_account_circle",
                    PageName = "ModifyUserPage",
                    Title = "Modify User"
                },
                new Menu
                {
                    Icon = "ic_exit_to_app",
                    PageName = "LoginPage",
                    Title = Settings.IsLogin? "Logout" : "Login"
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService, _apiService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }

        private async void ModifyUserAsync()
        {
            await _navigationService.NavigateAsync($"/CallingScoreMasterDetailPage/NavigationPage/{nameof(ModifyUserPage)}");
        }

    }
}
