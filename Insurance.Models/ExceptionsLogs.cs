using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models
{
    public class ExceptionsLogs
    {
        public int Id { get; set; }

        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string LoginUser { get; set; }
        public DateTime ErrorDateTime { get; set; } = DateTime.Now;
        public string ErrorLineNumber { get; set; }
        public string Error { get; set; }


    }
}
