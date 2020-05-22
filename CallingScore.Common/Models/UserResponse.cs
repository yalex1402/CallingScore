using CallingScore.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallingScore.Common.Models
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string UserCode { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Document { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PictureFullPath => string.IsNullOrEmpty(PicturePath)
                    ? "https://callingscoreweb.azurewebsites.net//images/noimage.png"
                    : LoginType == LoginType.App ? $"https://callingscoreweb.azurewebsites.net{PicturePath.Substring(1)}" : PicturePath;

        public string PicturePath { get; set; }

        public UserType UserType { get; set; }

        public LoginType LoginType { get; set; }

        public Campaign Campaign { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";
    }
}
