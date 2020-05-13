using CallingScore.Web.Data;
using CallingScore.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Helpers
{
    public class CampaignHelper : ICampaignHelper
    {
        private readonly DataContext _dataContext;

        public CampaignHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<CampaignEntity> GetCampaign(int campaignId)
        {
            return await _dataContext.Campaigns.FindAsync(campaignId);
        }
    }
}
