using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models
{
    public class UserRightAction
    {
        [Key]
        public int Id { get; set; }

        //---------------------------------------------------------------

        [Display(Name = "User Right Id")]
        public int UserRightId { get; set; }
       
        //---------------------------------------------------------------

        [Display(Name = "Action Id")]
        public int ActionId { get; set; }      

        //---------------------------------------------------------------
        [Display(Name = "Menu Id")]
        [Required(ErrorMessage = "Menu Id is required")]
        public int MenuId { get; set; }

        [Display(Name = "User Id")]
        [Required(ErrorMessage = "User Id is required")]
        public string Userid { get; set; }
       // -------------------------------------------------

        [Display(Name = "Status")]
        [Required]
        public bool IsActive { get; set; }

        [Display(Name = "Created By")]
        [Required]
        public string CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public ApplicationUser App_Users_Created { get; set; }

        [Display(Name = "Created Date")]
        [Required]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }
        [ForeignKey("ModifiedBy")]
        public ApplicationUser App_Users_Modified { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }



    }
}
