﻿using CallingScore.Prism.Views;
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

        public HomePageViewModel(INavigationService navigationService): base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Home";
            IsEnabled = true;
        }

        public DelegateCommand ShowStatisticsCommand => _showStatisticsCommand ?? (_showStatisticsCommand = new DelegateCommand(ShowStatisticsAsync));

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
            await _navigationService.NavigateAsync("/CallingScoreMasterDetailPage/NavigationPage/ShowStatisticsPage");
            IsRunning = false;
        }

    }
}
