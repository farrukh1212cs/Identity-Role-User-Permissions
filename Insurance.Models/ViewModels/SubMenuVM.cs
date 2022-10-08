using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models.ViewModels
{
    public class SubMenuVM
    {
        public Menu Menu { get; set; }

        public IEnumerable<SelectListItem> MenuList { get; set; }

    }
}
