using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Insurance.Models
{
    public class Profile 
    {
        [Display(Name = "FIRST NAME")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "LAST NAME")]

        [MaxLength(50)]
        public string LastName { get; set; }

        [Display(Name = "INSPECTOR ID")]

        [MaxLength(50)]
        public string InspectorID { get; set; }

        [Display(Name = "PHONE NUMBER")]

        [MaxLength(50)]
        public string PhoneNo { get; set; }


        [Display(Name = "POSITION")]

        [MaxLength(50)]
        public string POSITION { get; set; }

        [Display(Name = "COMPANY")]

        [MaxLength(50)]
        public string COMPANY { get; set; }

        [Display(Name = "COMPANY PHONE NUMBER")]

        [MaxLength(50)]
        public string COMPANY_PHONE_NO { get; set; }
        
        [Display(Name = "COMPANY EMAIL")]
        [MaxLength(50)]
        [EmailAddress]
        public string COMPANY_EMAIL { get; set; }
        //---------------------------------------------------------
        //---------------ADDITIONAL INFORMATION

        [Display(Name = "CURRENT ADDRESS")]
        [MaxLength(500)]
        public string StreetAddress { get; set; }

        [Display(Name = "CURRENT CITY")]
        [MaxLength(500)]
        public string City { get; set; }


        [Display(Name = "CURRENT STATE")]
        [MaxLength(500)]
        public string State { get; set; }

        [Display(Name = "CURRENT ZIP CODE")]
        [MaxLength(500)]
        public string PostalCode { get; set; }

        [Display(Name = "DATE OF BIRTH")]
        public DateTime DOB { get; set; } = DateTime.Now.AddDays(-1);

        [Display(Name = "XACTWARE ID")]
        [MaxLength(150)]
        public string XACTWARE_ID { get; set; }   
        
        [Display(Name = "XACTNET ADDRESS")]
        [MaxLength(150)]
        public string XACTWARE_ADDRESS { get; set; }
        //--------------------------------------------------------
        //-------------------------PREFERENCES

        [Display(Name = "STORIES")]
        [MaxLength(150)]
        public string STORIES { get; set; }
       

        [Display(Name = "Pitch")]
  
        public virtual DropDownList Pitch { get; set; }


        [Display(Name = "HAAG CERTIFIED")]
        public virtual DropDownList HAAG_CERTIFIED { get; set; }

        //--------------------------------------------------------
        //-------------------Availability


        [Display(Name = "SUNDAY START TIME")]
        public string SundayStartTime { get; set; }
        
        [Display(Name = "SUNDAY END TIME")]
        public string SundayEndTime { get; set; } 

        
        [Display(Name = "MONDAY START TIME")]
        public string MondayStartTime { get; set; }
        
        [Display(Name = "MONDAY END TIME")]
        public string MondayEndTime { get; set; }


        [Display(Name = "TUESDAY START TIME")]
        public string TuesdayStartTime { get; set; }
        
        [Display(Name = "TUESDAY END TIME")]
        public string TuesdayEndTime { get; set; } 
        
        [Display(Name = "WEDNESDAY START TIME")]
        public string WednesdayStartTime { get; set; }
        
        [Display(Name = "WEDNESDAY END TIME")]
        public string WednesdayEndTime { get; set; }


        [Display(Name = "THURSDAY START TIME")]
        public string ThursdayStartTime { get; set; }
        
        [Display(Name = "THURSDAY END TIME")]
        public string ThursdayEndTime { get; set; }   
        
        [Display(Name = "FRIDAY START TIME")]
        public string FridayStartTime { get; set; }
        
        [Display(Name = "FRIDAY END TIME")]
        public string FridayEndTime { get; set; } 
        
        [Display(Name = "SATURDAY START TIME")]
        public string SaturdayStartTime { get; set; }

        [Display(Name = "SATURDAY END TIME")]
        public string SaturdayEndTime { get; set; }

        //-------------------Availability
        //--------------------------------------------------------


        //--------------------------------------------------------
        //---------------------EXPERIENCE

        [Display(Name = "HAIL CLAIMS")]
        public int HAIL_CLAIMS { get; set; }

        [Display(Name = "FIRE CLAIMS")]
        public int FIRE_CLAIMS { get; set; }
        
        [Display(Name = "FLOOD CLAIMS")]
        public int FLOOD_CLAIMS { get; set; }  
        
        [Display(Name = "WATER CLAIMS")]
        public int WATER_CLAIMS { get; set; }

        [Display(Name = "WIND CLAIMS")]
        public int WIND_CLAIMS { get; set; }

        [Display(Name = "COMMERCIAL CLAIMS")]
        public int COMMERCIAL_CLAIMS { get; set; }

        [Display(Name = "VANDALISM CLAIMS")]
        public int VANDALISM_CLAIMS { get; set; }
        
        [Display(Name = "STRUCTURAL CLAIMS")]
        public int STRUCTURAL_CLAIMS { get; set; }

         [Display(Name = "THEFT CLAIMS")]
        public int THEFT_CLAIMS { get; set; }

        [Display(Name = "OTHER RELEVANT EXPERIENCE")]
        [MaxLength(500)]
        public string OTHER_RELEVANT_EXPERIENCE { get; set; }


        //--------------------------------------------------------
        //-----------------Description

        [Display(Name = "DESCRIPTION")]
        [MaxLength(500)]
        public string DESCRIPTION { get; set; }

        //-------------------------------------------------------

        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; }

        public List<string> UserZips { get; set; }

        public string Role { get; set; }

    }
}
