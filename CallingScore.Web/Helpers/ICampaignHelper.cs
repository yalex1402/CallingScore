using CallingScore.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Helpers
{
    public interface ICampaignHelper
    {
        Task<CampaignEntity> GetCampaign(int campaignId);
    }
}
