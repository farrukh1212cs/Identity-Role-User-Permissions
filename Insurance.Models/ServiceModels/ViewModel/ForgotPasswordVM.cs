using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models.ServiceModels.ViewModel
{
    public class ForgotPasswordVM
    {
        [Required]
        public string Email { get; set; }
        public string OTP { get; set; }
        public string NewPassword { get; set; }

    }
}
