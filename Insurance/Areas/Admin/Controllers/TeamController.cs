using Insurance.ActionFilters;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using Insurance.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Insurance.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Super_Admin)]
    [ServiceFilter(typeof(ValidateNameParameterAttribute))]
    public class TeamController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
       

        public TeamController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
         
        }

        public async Task<IActionResult> Index()
        {
           
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Team team = new Team();
            if (id == null)
            {
                //this is for create
       
                return PartialView("_Upsert", team);
            }
            //this is for edit
            team = await _unitOfWork.Team.GetAsync(id.GetValueOrDefault());
            if (team == null)
            {
                return NotFound();
            }
            return PartialView("_Upsert", team);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Team team)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
         
            string message = "Data Saved Successful";
                if (team.Id == 0)
                {
                team.CreatedBy = userId.Value.ToString();
                team.CreatedDate = DateTime.Now;
                await  _unitOfWork.Team.AddAsync(team);

                }
                else
                {
                    _unitOfWork.Team.Update(team);
                    message = "Data Updated Successful";
                }
                _unitOfWork.Save(userId.Value.ToString());
            return Json(new { success = true, message = message });

        }




        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _unitOfWork.Team.GetAllAsync(x => x.IsActive == true);
            return Json(new { data = allObj });
        }

        #endregion
    }
}
