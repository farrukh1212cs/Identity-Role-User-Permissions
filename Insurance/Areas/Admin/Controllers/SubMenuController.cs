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
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Super_Admin)]
    [ServiceFilter(typeof(ValidateNameParameterAttribute))]
    public class SubMenuController : Controller
    {
        

        private readonly IUnitOfWork _unitOfWork;

        public SubMenuController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        #region SubMenus

        //Sub Menu Index
        public IActionResult Index()
        {
            return View();
        } 
        public IActionResult Upsert(int? id, int MenuID)
        {


            IEnumerable<Menu> MenuList =  _unitOfWork.Menu.GetAll(x=>x.MenuUnder == 0 && x.IsActive == true && x.IsSingle == false);

            SubMenuVM subMenuVM = new SubMenuVM()
            {

                //Menui
                Menu = new Menu(),
                MenuList = MenuList.Select(i => new SelectListItem
                {
                    Text = i.MenuName,
                    Value = i.Id.ToString()
                }),
                
            };


 
            if (id == null)
            {
                //this is for create
          
                return PartialView("_UpsertPV", subMenuVM);
            }
            //this is for edit
            if (MenuID != null)
            {
                // ViewBag.MenuName = _unitOfWork.Menu.Get(MenuID).MenuName;
                subMenuVM.Menu.MenuUnder = MenuID;

            }
            if (id > 0)
            {
                subMenuVM.Menu = _unitOfWork.Menu.Get(id.GetValueOrDefault());

            }


            if (subMenuVM.Menu == null)
            {
                return NotFound();
            }
            return PartialView("_UpsertPV", subMenuVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(SubMenuVM menu1)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            string message = "Data Saved Successful";

            if (menu1.Menu.Id == 0)
            {
                menu1.Menu.CreatedBy = userId.Value.ToString();
                menu1.Menu.CreatedDate = DateTime.Now;
                _unitOfWork.Menu.Add(menu1.Menu);
            }
            else
            {
                menu1.Menu.ModifiedBy = userId.Value.ToString();
                menu1.Menu.ModifiedDate = DateTime.Now;
                _unitOfWork.Menu.Update(menu1.Menu);
                message = "Data Updated Successful";
            }
            _unitOfWork.Save(userId.Value.ToString());
            return Json(new { success = true, message = message });



        }
        #endregion



        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(int Id)
        {
            if (Id == 0)
            {
                var allObj = _unitOfWork.Menu.GetAll(x => x.MenuUnder > 0 && x.IsActive == true);
                return Json(new { data = allObj });
            }
            else
            {
                var allObj = _unitOfWork.Menu.GetAll(x => x.MenuUnder == Id && x.IsActive == true);
                return Json(new { data = allObj });
            }
        }

        //[HttpGet]
        //public IActionResult GetAllSubMenu(int Id)
        //{
        //    if(Id == 0)
        //    {
        //        var allObj = _unitOfWork.Menu.GetAll(x => x.MenuUnder > 0 && x.IsActive == true);
        //        return Json(new { data = allObj });
        //    }
        //    else
        //    {
        //        var allObj = _unitOfWork.Menu.GetAll(x => x.MenuUnder == Id && x.IsActive == true);
        //        return Json(new { data = allObj });
        //    }
            
     
        //}
        public IActionResult Delete(int id)
        {
            var Aactions = _unitOfWork.Aaction.GetAll(x => x.Menu_Ids == id);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (Aactions != null)
            {
                foreach (var item in Aactions)
                {
                    item.ModifiedBy = userId.Value.ToString();
                    item.ModifiedDate = DateTime.Now;
                    item.IsActive = false;
                }
                _unitOfWork.Save(userId.Value.ToString());
            }

            var submenu = _unitOfWork.Menu.GetAll(x => x.MenuUnder == id);
            if(submenu != null)
            {

                foreach (var item in submenu)
                {
                    item.ModifiedBy = userId.Value.ToString();
                    item.ModifiedDate = DateTime.Now;
                    item.IsActive = false;
                }             
                _unitOfWork.Save(userId.Value.ToString());
            }
            var objFromDb = _unitOfWork.Menu.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            objFromDb.IsActive = false;
            objFromDb.ModifiedBy = userId.Value.ToString();
            objFromDb.ModifiedDate = DateTime.Now;
            _unitOfWork.Save(userId.Value.ToString());
            return Json(new { success = true, message = "Delete Successful" });

        }
      
        
        #endregion
    }
}
