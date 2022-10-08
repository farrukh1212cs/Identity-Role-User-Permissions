using Insurance.ActionFilters;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using Insurance.Models.ViewModels;
using Insurance.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Insurance.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin+ ","+ SD.Role_Super_Admin)]
    [ServiceFilter(typeof(ValidateNameParameterAttribute))]
    public class AactionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AactionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            //Farrukh

            IEnumerable<Menu> MenuList = _unitOfWork.Menu.GetAll(x => x.MenuUnder != 0 && x.IsActive == true);

            AactionVM aActionVM = new AactionVM()
            {

               
                Aaction = new Aaction(),
                MenuList = MenuList.Select(i => new SelectListItem
                {
                    Text = i.MenuName,
                    Value = i.Id.ToString()
                }),

            };



            if (id == null)
            {
               return PartialView("_AUpsertPV", aActionVM);
            }         
            if (id > 0)
            {
                aActionVM.Aaction = _unitOfWork.Aaction.Get(id.GetValueOrDefault());

            }


            if (aActionVM.Aaction == null)
            {
                return NotFound();
            }
            return PartialView("_AUpsertPV", aActionVM);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(AactionVM action1)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);

            var checkifalreadyexists = _unitOfWork.Aaction.GetFirstOrDefault(x => x.Menu_Ids == action1.Aaction.Menu_Ids && x.ActionName == action1.Aaction.ActionName);

            //if (checkifalreadyexists != null)
            //{
            //    checkifalreadyexists.IsActive = true;
            //    _unitOfWork.Save();
            //    return Json(new { success = true, message = "Data Updated Successful" });
            //}

            string message = "Data Saved Successful";
            if (action1.Aaction.Id == 0 && checkifalreadyexists == null)
            {
                action1.Aaction.CreatedBy = userId.Value.ToString();
                action1.Aaction.CreatedDate = DateTime.Now; 
                _unitOfWork.Aaction.Add(action1.Aaction);
            }
            else
            {
                action1.Aaction.ModifiedBy = userId.Value.ToString();
                action1.Aaction.ModifiedDate = DateTime.Now;
                _unitOfWork.Aaction.Update(action1.Aaction);
                message = "Data Updated Successful";
            }
            _unitOfWork.Save(userId.Value.ToString());
            return Json(new { success = true, message = message });

        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Aaction.GetAll(x => x.IsActive == true, includeProperties: "Menu").OrderBy(x=>x.Menu_Ids);
            return Json(new { data = allObj });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var Aactions = _unitOfWork.Aaction.Get(id);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (Aactions != null)
            {

                Aactions.ModifiedBy = userId.Value.ToString();
                Aactions.ModifiedDate = DateTime.Now;
                Aactions.IsActive = false;
             
                _unitOfWork.Save(userId.Value.ToString());
            }

           //_unitOfWork.Save(userId.Value.ToString());
           
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
