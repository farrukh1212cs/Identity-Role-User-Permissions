using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models.ViewModels
{
    public class AactionVM
    {
        public Aaction Aaction { get; set; }

        public IEnumerable<SelectListItem> MenuList { get; set; }
    }
}
