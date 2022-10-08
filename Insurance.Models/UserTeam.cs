using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models
{
    public class UserTeam
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "TEAM ID")]
        [Required]

        public int TeamId { get; set; }

        [Display(Name = "USER ID")]
        [Required]
        public string UserID { get; set; }

        [Display(Name = "RoleID")]
        public string RoleID { get; set; }

        [Display(Name = "STATUS")]
        public bool IsActive { get; set; }

        [Display(Name = "Created By")]
        [Required]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        [Required]
        public DateTime CreatedDate { get; set; }


    }
}
