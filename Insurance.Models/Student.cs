using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Student Name")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Father Name")]

        [MaxLength(50)]
        public string FatherName { get; set; }


        [Display(Name = "Roll No")]
  
        [MaxLength(10)]
        public string RollNo { get; set; }


    }
}
