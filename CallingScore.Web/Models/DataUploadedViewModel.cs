using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Models
{
    public class DataUploadedViewModel
    {
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string CustomerProduct { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime StartDateToLocal => StartDate.ToLocalTime();

        [DataType(DataType.DateTime)]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime EndDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime EndDateToLocal => EndDate.ToLocalTime();

        public int CodificationId { get; set; }

        public string UserCode { get; set; }
    }
}
