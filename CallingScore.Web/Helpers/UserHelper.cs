using CallingScore.Common.Enums;
using CallingScore.Web.Data;
using CallingScore.Web.Data.Entities;
using CallingScore.Web.Models;
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
        private readonly SignInManager<UserEntity> _signInManager;

        public UserHelper(
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<UserEntity> signInManager,
            DataContext dataContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dataContext = dataContext;
        }

        public async Task<IdentityResult> AddUserAsync(UserEntity user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<UserEntity> GetUserAsync(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task AddUserToRoleAsync(UserEntity user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
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

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
            model.Username,
            model.Password,
            model.RememberMe,
            false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<UserEntity> AddUserAsync(AddUserViewModel model, string path)
        {
            UserEntity userEntity = new UserEntity
            {
                UserCode = model.UserCode,
                Document = model.Document,
                Email = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PicturePath = path,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username,
                UserType = model.UserTypeId == 1 ? UserType.Supervisor : UserType.CallAdviser
            };

            IdentityResult result = await _userManager.CreateAsync(userEntity, model.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            UserEntity newUser = await GetUserAsync(model.Username);
            await AddUserToRoleAsync(newUser, userEntity.UserType.ToString());
            return newUser;
        }

        public async Task<IdentityResult> ChangePasswordAsync(UserEntity user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<IdentityResult> UpdateUserAsync(UserEntity user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<SignInResult> ValidatePasswordAsync(UserEntity user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(UserEntity user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(UserEntity user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(UserEntity user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(UserEntity user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public string GetUserCode(string name, string lastName)
        {
            Random random = new Random();
            string abc = "abcdefghijklmnopqrstuvwxyz";
            string userCode = $"cob1{name.ToLower()[random.Next(0, 1)]}{lastName.ToLower()[random.Next(0, 1)]}{abc[random.Next(0,abc.Length-1)]}";
            UserEntity user = GetUserByCodeAsync(userCode);
            while (user != null)
            {
                 userCode = $"cob1{name.ToLower()[random.Next(0, 1)]}{lastName.ToLower()[random.Next(0, 1)]}{abc[random.Next(0, abc.Length - 1)]}";
                 user = GetUserByCodeAsync(userCode);
            }
            return userCode;
        }

        public UserEntity GetUserByCodeAsync(string userCode)
        {
            return _dataContext.Users.FirstOrDefault(u => u.UserCode == userCode);
        }

        public async Task AddUserToCampaignAsync(UserEntity user, string campaignName)
        {
            CampaignEntity campaign = _dataContext.Campaigns.FirstOrDefault(c => c.Name == campaignName);
            if (campaign == null)
            {
                _dataContext.Add(new CampaignEntity { Name = campaignName });
                campaign = _dataContext.Campaigns.FirstOrDefault(c => c.Name == campaignName);
            }
            user.Campaign = campaign;
            _dataContext.Update(user);
            await _dataContext.SaveChangesAsync();
        }
    }

}
