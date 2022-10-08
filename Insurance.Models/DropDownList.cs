using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models
{
    public class DropDownList
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Key")]
        [Required]
        [MaxLength(50)]
        public string Key { get; set; } 
        
        [Display(Name = "Value")]
        [Required]
        [MaxLength(50)]
        public string Value { get; set; }

        [Display(Name = "DropDown")]
        [Required]
        [MaxLength(50)]
        public string DropDown { get; set; }

        [Display(Name = "Status")]
        [Required]
        public bool IsActive { get; set; }

        [Display(Name = "Created By")]
        [Required]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        [Required]
        public DateTime CreatedDate { get; set; }

    }
}
