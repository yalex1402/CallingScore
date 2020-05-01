using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Models
{
    public class ArrivalsUploadedViewModel
    {
        [DataType(DataType.DateTime)]
        [Display(Name = "In Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime InDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "In Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime InDateToLocal => InDate.ToLocalTime();

        [DataType(DataType.DateTime)]
        [Display(Name = "Out Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime OutDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Out Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime OutDateToLocal => OutDate.ToLocalTime();

        public string UserCode { get; set; }
    }
}
