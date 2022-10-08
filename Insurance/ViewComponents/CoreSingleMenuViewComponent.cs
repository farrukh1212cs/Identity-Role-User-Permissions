using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using Insurance.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Insurance.ViewComponents
{
     public class CoreSingleMenuViewComponent : ViewComponent
     {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public CoreSingleMenuViewComponent(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _db = db;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            //UserID 
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //------------------------------------------------------------------
            //get userroles and add in list
            var userrole = _db.UserRoles.Where(x => x.UserId == userId).ToList();
            List<string> roleas = new List<string>();
            foreach (var item in userrole)
            {
                roleas.Add(item.RoleId);

            }
            //------------------------------------------------------------------
            //Check if user is super User
            bool IsSuperUser = false;

            foreach (var item in roleas)
            {
                if (_db.Roles.Where(x => x.Id == item).Select(x => x.Name).FirstOrDefault() == SD.Role_Super_Admin)
                {
                    IsSuperUser = true;
                    break;
                }
            }
            //var Menus =  (IEnumerable<Menu>)null;

            IEnumerable<Menu> Menus;
            if (IsSuperUser)
            {
                Menus = _unitOfWork.Menu.GetAll(x => x.IsActive == true && x.IsSingle == true).ToList();
            }
            else
            {

                //Declare Ids list

                List<int> NewIDS = new List<int>();

                //Get Menus from RoleRight based on Role;
                var RoleMenusIDS = _unitOfWork.RoleRightAction.GetAll(x => roleas.Contains(x.Role_Id) && x.IsActive == true).Select(x => x.MenuId).Distinct();

                foreach (var item in RoleMenusIDS)
                {
                    bool isparrentAdded = false;
                    int parentID = _unitOfWork.Menu.GetAll(x => x.Id == Convert.ToInt32(item) && x.IsActive == true).Select(x => x.MenuUnder).FirstOrDefault();
                    foreach (var i in NewIDS)
                    {
                        if (i == parentID)
                        {
                            isparrentAdded = true;
                        }
                    }
                    if (!isparrentAdded)
                    {
                        NewIDS.Add(Convert.ToInt32(parentID));
                    }
                    NewIDS.Add(Convert.ToInt32(item));
                }
                //Get Menus from UserRight based on User ID;
                var UserRightIDS = _unitOfWork.UserRightAction.GetAll(x => x.Userid == userId && x.IsActive == true).Select(x => x.MenuId).Distinct();

                foreach (var item in UserRightIDS)
                {
                    bool isparrentAdded = false;
                    int parentID = _unitOfWork.Menu.GetAll(x => x.Id == Convert.ToInt32(item) && x.IsActive == true).Select(x => x.MenuUnder).FirstOrDefault();
                    foreach (var i in NewIDS)
                    {
                        if (i == parentID)
                        {
                            isparrentAdded = true;
                        }
                    }
                    if (!isparrentAdded)
                    {
                        NewIDS.Add(Convert.ToInt32(parentID));
                    }
                    NewIDS.Add(Convert.ToInt32(item));
                }


                //Here we have to Implement UserWise Menus
                Menus = _unitOfWork.Menu.GetAll(x => NewIDS.Contains(x.Id) && x.IsActive == true).ToList();



            }

            return View(Menus);
         //   return View();
        }
    }
}
