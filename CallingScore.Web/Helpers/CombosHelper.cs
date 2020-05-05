using CallingScore.Common.Enums;
using CallingScore.Web.Data;
using CallingScore.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        public CombosHelper(DataContext dataContext,
            IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
        }

        public IEnumerable<SelectListItem> GetComboRoles()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "[Select a role...]" },
                new SelectListItem { Value = "1", Text = "Supervisor" },
                new SelectListItem { Value = "2", Text = "Call Adviser" }
            };

            return list;
        }

        public IEnumerable<SelectListItem> GetComboUsers()
        {
            List<UserEntity> users =  _dataContext.Users.Where(u => u.UserType == UserType.CallAdviser).ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "0", Text = "[Select the user...]" });
            foreach (UserEntity user in users)
            {
                list.Add(new SelectListItem
                {
                    Value = user.UserCode,
                    Text = user.FullName
                });
            }
            return list;
        }

        public IEnumerable<SelectListItem> GetComboCampaigns()
        {
            List<CampaignEntity> campaigns = _dataContext.Campaigns.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "0", Text = "[Select a campaign...]" });
            foreach (CampaignEntity campaign in campaigns)
            {
                list.Add(new SelectListItem 
                {
                    Value = campaign.Id.ToString(),
                    Text = campaign.Name
                });
            }
            return list;
        }
    }
}
