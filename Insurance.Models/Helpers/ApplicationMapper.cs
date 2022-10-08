using AutoMapper;
using Insurance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Models.Helpers
{
    public class ApplicationMapper : Profile
    {

        public ApplicationMapper()
        {
            //CreateMap<ApplicationUser, UserDataVM>()               
            //    .ReverseMap();  
          
            //CreateMap<UserZip, UserDataVM>().ReverseMap();
        }
    }
}
