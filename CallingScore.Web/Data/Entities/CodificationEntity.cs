using CallingScore.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Data.Entities
{
    public class CodificationEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string CodeName { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public int Priority { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public ContactType ContactType { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public EffectivityType EffectivityType { get; set; }

        public List<CallEntity> Calls { get; set; }
    }
}
