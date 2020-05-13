using CallingScore.Web.Data.Entities;
using CallingScore.Web.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Helpers
{
    public interface IUserHelper
    {
        Task<UserEntity> GetUserAsync(string email);

        Task<UserEntity> GetUserAsync(Guid userId);

        Task<IdentityResult> AddUserAsync(UserEntity user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(UserEntity user, string roleName);

        Task<bool> IsUserInRoleAsync(UserEntity user, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<UserEntity> AddUserAsync(AddUserViewModel model, string path);

        Task<IdentityResult> ChangePasswordAsync(UserEntity user, string oldPassword, string newPassword);

        Task<IdentityResult> UpdateUserAsync(UserEntity user);

        Task<SignInResult> ValidatePasswordAsync(UserEntity user, string password);

        Task<string> GenerateEmailConfirmationTokenAsync(UserEntity user);

        Task<IdentityResult> ConfirmEmailAsync(UserEntity user, string token);

        Task<string> GeneratePasswordResetTokenAsync(UserEntity user);

        Task<IdentityResult> ResetPasswordAsync(UserEntity user, string token, string password);

        string GetUserCode(string name, string lastname);

        UserEntity GetUserByCodeAsync(string userCode);

        Task AddUserToCampaignAsync(UserEntity user, string campaignName);

        Task<List<UserEntity>> GetUsersByCampaign(int campaignId);
    }
}
