using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models.ViewModels
{
    public class PasswordChangeVM
    {
        [Display(Name = "Old Password")]
        [Required]
        public string OldPassword { get; set; }
        [Display(Name = "New Password")]
        [Required]
        public string NewPassword { get; set; }

    }
}
