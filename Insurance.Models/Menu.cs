using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Menu Name")]
        [Required]
        [MaxLength(50)]
        public string MenuName { get; set; }


        [Display(Name = "Area Name")]
        [Required]
        [MaxLength(50)]
        public string AreaName { get; set; }

        [Display(Name = "Controller Name")]
        [Required]
        [MaxLength(50)]
        public string ControllerName { get; set; }

        [Display(Name = "Action Name")]
        [Required]
        [MaxLength(50)]
        public string ActionName { get; set; }

        [Display(Name = "Menu Icon")]
        [Required]
        [MaxLength(250)]
        public string MenuIcon { get; set; }

        [Display(Name = "Menu URL")]
        [Required]
        [MaxLength(250)]
        public string MenuURL { get; set; }

        [Display(Name = "Menu Order")]
        [Required]
        public int MenuOrder { get; set; }

        [Display(Name = "Menu Under")]
        public int MenuUnder { get; set; }

        [Display(Name = "Status")]
        [Required]
        public bool IsActive { get; set; } 
        
        [Display(Name = "API")]
        [Required]
        public bool API { get; set; } 
        
        [Display(Name = "Is Single")]
        [Required]
        public bool IsSingle { get; set; }

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
