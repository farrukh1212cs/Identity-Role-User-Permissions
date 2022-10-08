using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models.ViewModels
{
    public  class UserRightActionVM
    {
        public UserRightAction UserRightAction { get; set; }

        public IEnumerable<SelectListItem> ActionsList { get; set; }
        public string UserName { get; set; }
        public string MenuName { get; set; }
     
    }
}
