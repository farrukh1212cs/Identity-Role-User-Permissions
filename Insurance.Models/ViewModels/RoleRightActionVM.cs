using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models.ViewModels
{
    public  class RoleRightActionVM
    {
        public RoleRightAction RoleRightAction { get; set; }

        public IEnumerable<SelectListItem> ActionsList { get; set; }
        public string RoleName { get; set; }
        public string MenuName { get; set; }
     
    }
}
