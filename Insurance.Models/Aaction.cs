using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models
{
    public class Aaction
    {
        [Key]
        public int Id { get; set; }

    
        [Display(Name = "Action Name")]
        [Required]
        [MaxLength(50)]
        public string ActionName { get; set; }

      

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


        //------------------------------------------------
        [Display(Name = "Menu Id")]
        public int Menu_Ids { get; set; }
        [ForeignKey("Menu_Ids")]
        public virtual Menu Menu { get; set; }
        //------------------------------------------------
    }
}
