using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CallingScore.Common.Models
{
    public class StatisticsRequest
    {
        public string UserCode { get; set; }

        [Required]
        public int Month { get; set; }

        public int CampaignId { get; set; }
    }
}
