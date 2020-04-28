using CallingScore.Web.Data;
using CallingScore.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _dataContext;

        public UserHelper(
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            DataContext dataContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dataContext = dataContext;
        }
        public async Task<IdentityResult> AddUserAsync(UserEntity user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(UserEntity user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task AddUserToCampaignAsync(UserEntity user, string campaignName)
        {
            CampaignEntity campaign = _dataContext.Campaigns.FirstOrDefault(c => c.Name == campaignName);
            if(campaign == null)
            {
                _dataContext.Add(new CampaignEntity { Name = campaignName });
                campaign = _dataContext.Campaigns.FirstOrDefault(c => c.Name == campaignName);
            }
            user.Campaign = campaign;
            _dataContext.Update(user);
            await _dataContext.SaveChangesAsync();
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<UserEntity> GetUserAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> IsUserInRoleAsync(UserEntity user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }
    }

}
