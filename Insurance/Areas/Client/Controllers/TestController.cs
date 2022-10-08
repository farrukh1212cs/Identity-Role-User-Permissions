using Insurance.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Areas.Client.Controllers
{

    [Area("Client")]    
    [ServiceFilter(typeof(ValidateNameParameterAttribute))]
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
