using CallingScore.Prism.Interfaces;
using CallingScore.Prism.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CallingScore.Prism.Helpers
{
    public class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            Culture = ci.Name;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Culture { get; set; }

        public static string Error => Resource.Error;

        public static string Accept => Resource.Accept;

        public static string InternetConnection => Resource.InternetConnection;

        public static string Ok => Resource.Ok;

        public static string ErrorEmail => Resource.ErrorEmail;

        public static string RecoverPassword => Resource.RecoverPassword;

        public static string Register => Resource.Register;

        public static string ErrorEmptyField => Resource.ErrorEmptyField;

        public static string ErrorRole => Resource.ErrorRole;

        public static string ErrorPassword => Resource.ErrorPassword;

        public static string ErrorPasswordShort => Resource.ErrorPasswordShort;

        public static string ErrorPassword2 => Resource.ErrorPassword2;

        public static string FromGallery => Resource.FromGallery;

        public static string FromCamera => Resource.FromCamera;

        public static string Cancel => Resource.Cancel;

        public static string PictureSource => Resource.PictureSource;

        public static string ModifyUser => Resource.ModifyUser;

        public static string UserUpdated => Resource.UserUpdated;

        public static string Login => Resource.Login;

        public static string LoginFB => Resource.LoginFB;

        public static string Logout => Resource.Logout;

        public static string ErrorLogin => Resource.ErrorLogin;

        public static string FBAuthorization => Resource.FBAuthorization;

        public static string Canceled => Resource.Canceled;

        public static string Unauthorized => Resource.Unauthorized;

        public static string Loading => Resource.Loading;

        public static string Email => Resource.Email;

        public static string EmailPlaceHolder => Resource.EmailPlaceHolder;

        public static string Password => Resource.Password;

        public static string PasswordPlaceHolder => Resource.PasswordPlaceHolder;

        public static string ForgotPassword => Resource.ForgotPassword;

        public static string Document => Resource.Document;

        public static string DocumentPlaceHolder => Resource.DocumentPlaceHolder;

        public static string FirstName => Resource.FirstName;

        public static string FirstNamePlaceHolder => Resource.FirstNamePlaceHolder;

        public static string LastName => Resource.LastName;

        public static string LastNamePlaceHolder => Resource.LastNamePlaceHolder;

        public static string Phone => Resource.Phone;

        public static string PhonePlaceHolder => Resource.PhonePlaceHolder;

        public static string ConfirmPassword => Resource.ConfirmPassword;

        public static string Role => Resource.Role;

        public static string RolePlaceHolder => Resource.RolePlaceHolder;

        public static string Campaign => Resource.Campaign;

        public static string CampaignPlaceHolder => Resource.CampaignPlaceHolder;

        public static string Save => Resource.Save;

    }
}
