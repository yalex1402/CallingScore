using CallingScore.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Data.Entities
{
    public class UserEntity : IdentityUser
    {
        [Display(Name = "User Code")]
        [MaxLength(7, ErrorMessage = "The {0} field cannot have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string UserCode { get; set; }

        [Display(Name = "Document")]
        [MaxLength(20, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Document { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string LastName { get; set; }

        [Display(Name = "Picture")]
        public string PicturePath { get; set; }

        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";

        public ICollection<CallEntity> Calls { get; set; }

        public ICollection<MonitoringEntity> Monitorings { get; set; }

        public ICollection<ArrivalsEntity> Arrivals { get; set; }

        public CampaignEntity Campaign { get; set; }
    }
}
