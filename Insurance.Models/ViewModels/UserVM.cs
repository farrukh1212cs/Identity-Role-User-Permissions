using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models.ViewModels
{
    public class UserVM
    {
        public UserRegisterInputModel User { get; set; }

        public int UserTeamID { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; }
        public IEnumerable<SelectListItem> TeamsList { get; set; }
    }
}
