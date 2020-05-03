using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Models
{
    public class ToShowChartViewModel
    {
        public List<ContactStatisticsViewModel> ContactStatistics { get; set; }

        public List<EffectivityStatisticsViewModel> EffectivityStatistics { get; set; }
    }
}
