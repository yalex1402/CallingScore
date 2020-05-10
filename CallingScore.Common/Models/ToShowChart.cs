using System;
using System.Collections.Generic;
using System.Text;

namespace CallingScore.Common.Models
{
   public class ToShowChart
   {
        public List<ContactStatistics> ContactStatistics { get; set; }

        public List<EffectivityStatistics> EffectivityStatistics { get; set; }
    }
}
