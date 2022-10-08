using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "TEAM NAME")]
        [Required]
        [MaxLength(50)]
        public string TeamName { get; set; }  
        
        [Display(Name = "TEAM DESCRIPTION")]    
        [MaxLength(500)]
        public string TeamDescription { get; set; }

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
