using CallingScore.Common.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Models
{
    public class ToShowCharByCampaignViewModel : ToShowChart
    {
        [Display(Name = "Campaign")]
        public int CampaignId { get; set; }

        public IEnumerable<SelectListItem> Campaigns { get; set; }
    }
}
