using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models
{
    public class SMTPSettings
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "Email")]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [MaxLength(500)]
        public string Password { get; set; } 
        
    
        [Display(Name = "Sender Display Name")]
        [MaxLength(100)]
        public string SenderDisplayName { get; set; }

        [Display(Name = "Sender Address")]
        [MaxLength(500)]
        public string SenderAddress { get; set; }

        [Display(Name = "Host")]
        [MaxLength(500)]
        public string Host { get; set; }

        [Display(Name = "Port")]       
        public int Port { get; set; }

        [Display(Name = "EnableSSL")]       
        public bool EnableSSL { get; set; }

        [Display(Name = "UseDefaultCredentials")]       
        public bool UseDefaultCredentials { get; set; }
        
        [Display(Name = "IsBodyHTML")]       
        public bool IsBodyHTML { get; set; }



    }
}
