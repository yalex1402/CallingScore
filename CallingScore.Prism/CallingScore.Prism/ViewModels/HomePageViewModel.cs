using CallingScore.Common.Helpers;
using CallingScore.Common.Models;
using CallingScore.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CallingScore.Prism.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private bool _isEnabled;
        private bool _isRunning;
        private DelegateCommand _showStatisticsCommand;
        private DelegateCommand _showStatisticsByCampaignCommand;
        private UserResponse _user;

        public HomePageViewModel(INavigationService navigationService): base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Home";
            IsRunning = false;
            IsEnabled = true;
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
        }

        public DelegateCommand ShowStatisticsCommand => _showStatisticsCommand ?? (_showStatisticsCommand = new DelegateCommand(ShowStatisticsAsync));

        public DelegateCommand ShowStatisticsByCampaignCommand => _showStatisticsByCampaignCommand ?? (_showStatisticsByCampaignCommand = new DelegateCommand(ShowStatisticsByCampaignAsync));

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

        public async void ShowStatisticsAsync()
        {
            IsRunning = true;
            if (_user.UserType == Common.Enums.UserType.Supervisor)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "You're not authorized to access into that statistic", "Accept");
                return;
            }
            await _navigationService.NavigateAsync("/CallingScoreMasterDetailPage/NavigationPage/ShowStatisticsPage");
            IsRunning = false;
        }

        public async void ShowStatisticsByCampaignAsync()
        {
            IsRunning = true;
            if(_user.UserType == Common.Enums.UserType.CallAdviser)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "You're not authorized to access into that statistic", "Accept");
                return;
            }
            await _navigationService.NavigateAsync("/CallingScoreMasterDetailPage/NavigationPage/ShowStatisticsByCampaignPage");
            IsRunning = false;
        }

    }
}
