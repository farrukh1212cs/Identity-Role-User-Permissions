﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models
{
    public class UserRegisterInputModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "CURRENT ADDRESS")]
       
        public string StreetAddress { get; set; }

        [Display(Name = "CURRENT City")]
        public string City { get; set; }

        [Display(Name = "CURRENT State")]
        public string State { get; set; }
        [Display(Name = "CURRENT ZIP CODE")]
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }

        public bool Is_Active { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}
