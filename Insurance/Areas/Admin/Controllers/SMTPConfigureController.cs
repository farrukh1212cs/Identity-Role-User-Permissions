using Insurance.ActionFilters;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using Insurance.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Insurance.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Super_Admin)]
    [ServiceFilter(typeof(ValidateNameParameterAttribute))]
    public class SMTPConfigureController : Controller
    {


        private readonly IUnitOfWork _unitOfWork;

        public SMTPConfigureController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {

          

            return View("");
        }

        public async Task<IActionResult> Upsert(int? id)
        {



           

           

            return PartialView("_Upsert", "");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(SMTPSettings sts)
        {
            
            return Json(new { success = true, message = "Data Updated" });

        }

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           
            return Json(new { data = "" });
        }

        #endregion
    }
}
