using CallingScore.Common.Helpers;
using CallingScore.Common.Models;
using CallingScore.Common.Services;
using CallingScore.Prism.Helpers;
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
    public class ShowStatisticsByCampaignPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ToShowChart _chart;
        private ObservableCollection<StatisticsType> _statisticsTypes;
        private StatisticsType _statisticType;
        private ObservableCollection<Month> _months;
        private Month _month;
        private DelegateCommand _showStatisticsCommand;
        private bool _isRunning;
        private bool _isVisible;
        private bool _isVisibleContact;
        private bool _isVisibleEffectivity;
        public ShowStatisticsByCampaignPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            IsRunning = false;
            IsVisible = false;
            Chart = new ToShowChart();
            StatisticsTypes = new ObservableCollection<StatisticsType>(CombosHelper.GetStatisticsTypes());
            Months = new ObservableCollection<Month>(CombosHelper.GetMonths());
            Title = "Statistics By Campaign";
        }

        public DelegateCommand ShowStatisticsCommand => _showStatisticsCommand ?? (_showStatisticsCommand = new DelegateCommand(ShowStatisticsAsync));

        public ToShowChart Chart
        {
            get => _chart;
            set => SetProperty(ref _chart, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public bool IsVisibleContact
        {
            get => _isVisibleContact;
            set => SetProperty(ref _isVisibleContact, value);
        }

        public bool IsVisibleEffectivity
        {
            get => _isVisibleEffectivity;
            set => SetProperty(ref _isVisibleEffectivity, value);
        }

        public ObservableCollection<StatisticsType> StatisticsTypes
        {
            get => _statisticsTypes;
            set => SetProperty(ref _statisticsTypes, value);
        }

        public StatisticsType StatisticType
        {
            get => _statisticType;
            set => SetProperty(ref _statisticType, value);
        }

        public ObservableCollection<Month> Months
        {
            get => _months;
            set => SetProperty(ref _months, value);
        }

        public Month MonthSelected
        {
            get => _month;
            set => SetProperty(ref _month, value);
        }

        private async void LoadChart()
        {
            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Check the internet connection", "Accept");
                return;
            }
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            UserResponse userResponse = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            StatisticsRequest request = new StatisticsRequest
            {
                UserCode = userResponse.UserCode,
                Month = MonthSelected.Id,
                CampaignId = userResponse.Campaign.Id
            };
            Response response = await _apiService.GetStatistics(url, "api", "/Calls/StatisticsByCampaign", "bearer", token.Token, request);
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "There has ocurred an error, try again...", "Accept");
                return;
            }
            Chart = (ToShowChart)response.Result;
            IsVisible = true;
            IsRunning = false;
        }

        private void ShowStatisticsAsync()
        {
            IsRunning = true;
            LoadChart();
            if (StatisticType.Name == "Contact")
            {
                IsVisibleContact = true;
                IsVisibleEffectivity = false;
            }
            else
            {
                IsVisibleContact = false;
                IsVisibleEffectivity = true;
            }

        }
    }
}
