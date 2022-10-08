using Insurance.ActionFilters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Areas.Client.Controllers
{

    [Area("Client")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ServiceFilter(typeof(ApiAuthroization))]
    public class APITestController : Controller
    {
        [HttpGet("Test")]
        public async Task<string> Test()
        {
           // var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            return "abc";
        }

        [HttpGet]
        public IActionResult GetAll()
        {
           
            return Json(new { data = "Welcome" });
        }
    }
}
