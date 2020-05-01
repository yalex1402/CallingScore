using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CallingScore.Web.Models
{
    public class UploadCallViewModel
    {
        [Display(Name = "File Source")]
        [Required]
        public IFormFile File { get; set; }
    }
}
