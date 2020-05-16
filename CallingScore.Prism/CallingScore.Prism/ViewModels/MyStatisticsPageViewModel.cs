using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CallingScore.Prism.ViewModels
{
    public class MyStatisticsPageViewModel : ViewModelBase
    {
        public MyStatisticsPageViewModel(INavigationService navigationService): base(navigationService)
        {
            Title = "My Statistics";
        }
    }
}
