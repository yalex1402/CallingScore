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
using System.Linq;

namespace CallingScore.Prism.ViewModels
{
    public class ShowStatisticsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ToShowChart _chart;
        private List<ContactStatistics> _contact;
        private List<EffectivityStatistics> _effectivity;
        public ShowStatisticsPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Show Statistics";
            Chart = new ToShowChart();
            LoadChart();
        }

        public ToShowChart Chart
        {
            get => _chart;
            set => SetProperty(ref _chart, value);
        }

        public List<ContactStatistics> Contact
        {
            get => _contact;
            set => SetProperty(ref _contact, value);
        }

        public List<EffectivityStatistics> Effectivity
        {
            get => _effectivity;
            set => SetProperty(ref _effectivity, value);
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
                Month = 4
            };
            Response response = await _apiService.GetStatistics(url, "api", "/Calls/MyStatistics", "bearer", token.Token, request);
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "There has ocurred an error, try again...", "Accept");
                return;
            }
            Chart = (ToShowChart)response.Result;
            Contact = Chart.ContactStatistics;
            Effectivity = Chart.EffectivityStatistics;
        }
    }
}
