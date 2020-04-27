using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Data.Entities
{
    public class ArrivalsEntity
    {
        public int Id { get; set; }

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

        public UserEntity User { get; set; }
    }
}
