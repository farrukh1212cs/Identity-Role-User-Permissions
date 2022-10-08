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
    public class MenuController : Controller
    {
        

        private readonly IUnitOfWork _unitOfWork;

        public MenuController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Menu
        public IActionResult Index()
        {
            @ViewData["Title"] = "Index";
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Menu menu = new Menu();
            if (id == null)
            {
                return PartialView("_UpsertPV", menu);
            }
            //this is for edit
            menu = _unitOfWork.Menu.Get(id.GetValueOrDefault());
            if (menu == null)
            {
                return NotFound();
            }
            return PartialView("_UpsertPV", menu);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Menu menu)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);

            string message = "Data Saved Successful";
            try
            {
                if (menu.Id == 0)
                {
                    if (!menu.IsSingle)
                    {
                        menu.ControllerName = menu.MenuName;
                        menu.ActionName = "";
                        menu.MenuURL = "";
                    }
                    menu.ControllerName = menu.ControllerName;
                    menu.MenuUnder = 0;

                    menu.CreatedBy = userId.Value.ToString();
                    menu.CreatedDate = DateTime.Now;
                    menu.ControllerName = menu.ControllerName;
                    if (!menu.IsSingle)
                    {
                        menu.ActionName = "";
                        menu.MenuURL = "";
                    }

                    menu.MenuUnder = 0;

                    _unitOfWork.Menu.Add(menu);


                }
                else
                {
                    if (!menu.IsSingle)
                    {
                        menu.ControllerName = menu.MenuName;
                        menu.ActionName = "";
                        menu.MenuURL = "";
                    }
                    menu.ControllerName = menu.ControllerName;
                    menu.MenuUnder = 0;
                    menu.ModifiedBy = userId.Value.ToString();
                    menu.ModifiedDate = DateTime.Now;
                    _unitOfWork.Menu.Update(menu);
                    message = "Data Updated Successful";
                }
                _unitOfWork.Save(userId.Value.ToString());
                return Json(new { success = true, message = message });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Error : " + ex.Message.ToString() });
            }
          

        }
        #endregion


        #region SubMenus

        //Sub Menu Index
        public IActionResult SubMenu()
        {
            return View();
        }
        public IActionResult SubMenuUpsert(int? id, int MenuID)
        {


            IEnumerable<Menu> MenuList =  _unitOfWork.Menu.GetAll(x=>x.MenuUnder == 0 && x.IsActive == true);

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
          
                return PartialView("_SubMenuUpsertPV", subMenuVM);
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
            return PartialView("_SubMenuUpsertPV", subMenuVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubMenuUpsert(SubMenuVM menu1)
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
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Menu.GetAll(x=>x.MenuUnder == 0 && x.IsActive == true, includeProperties: "App_Users_Created");
            return Json(new { data = allObj });
        }

        [HttpGet]
        public IActionResult GetAllSubMenu(int Id)
        {
            if(Id == 0)
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
