using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Models
{
    public class ToShowCharByUserViewModel : ToShowChartViewModel
    {
        [Display(Name ="User Name")]
        public string UserCode { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
