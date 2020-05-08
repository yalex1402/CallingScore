using CallingScore.Common.Helpers;
using CallingScore.Common.Models;
using CallingScore.Common.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallingScore.Prism.ViewModels
{
    public class MenuItemViewModel : Menu
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _selectMenuCommand;

        public MenuItemViewModel(INavigationService navigationService,
            IApiService apiService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
        }

        public DelegateCommand SelectMenuCommand => _selectMenuCommand ?? (_selectMenuCommand = new DelegateCommand(SelectMenuAsync));

        private async void SelectMenuAsync()
        {
            if (PageName == "LoginPage" && Settings.IsLogin)
            {
                Settings.IsLogin = false;
                Settings.User = null;
                Settings.Token = null;
            }

            if (!Settings.IsLogin)
            {
                await _navigationService.NavigateAsync($"/CallingScoreMasterDetailPage/NavigationPage/LoginPage");
                return;
            }

            await _navigationService.NavigateAsync($"/CallingScoreMasterDetailPage/NavigationPage/{PageName}");
        }
    }
}
