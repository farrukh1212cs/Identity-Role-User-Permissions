using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using Insurance.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Insurance.ActionFilters
{
    public class ValidateNameParameterAttribute : ActionFilterAttribute
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _db;

        public ValidateNameParameterAttribute(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
            IDictionary<string, string> numberNames = new Dictionary<string, string>();
            numberNames.Add(((object[])filterContext.RouteData.Values.Keys)[0].ToString(), ((object[])filterContext.RouteData.Values.Values)[0].ToString());
            numberNames.Add(((object[])filterContext.RouteData.Values.Keys)[1].ToString(), ((object[])filterContext.RouteData.Values.Values)[1].ToString());
            numberNames.Add(((object[])filterContext.RouteData.Values.Keys)[2].ToString(), ((object[])filterContext.RouteData.Values.Values)[2].ToString());


            string Area = numberNames.Where(x => x.Key == "area").Select(x=>x.Value).FirstOrDefault();
            string Controller = numberNames.Where(x => x.Key == "controller").Select(x=>x.Value).FirstOrDefault();
            string Action = numberNames.Where(x => x.Key == "action").Select(x=>x.Value).FirstOrDefault();

            int menuID = 0;

             menuID = _unitOfWork.Menu.GetAll(x => x.IsActive == true && x.AreaName == Area && x.ControllerName == Controller && x.MenuUnder != 0 && x.ActionName == Action).Select(x => x.Id).FirstOrDefault();
            if(menuID == 0)
            {
              menuID = _unitOfWork.Menu.GetAll(x => x.IsActive == true && x.AreaName == Area && x.ControllerName == Controller && x.MenuUnder != 0).Select(x => x.Id).FirstOrDefault();

            }

            int actionID = _unitOfWork.Aaction.GetAll(x => x.IsActive == true && x.Menu_Ids == menuID && x.ActionName == Action).Select(x => x.Id).FirstOrDefault();
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);         

            //get login UserID
           
            if (actionID == 0)
            {
                Aaction newaction = new Aaction();
                newaction.ActionName = Action;
                newaction.Menu_Ids = menuID;
                newaction.CreatedBy = userId;
                newaction.CreatedDate = DateTime.Now;
                newaction.ModifiedBy = null;
                newaction.IsActive = true;
                _unitOfWork.Aaction.Add(newaction);
                _unitOfWork.Save(userId);

            }

            //=================================================================================================================================
            //Role Rights Work
            //=================================================================================================================================

            //get userroles and add in list
            var userrole = _db.UserRoles.Where(x => x.UserId == userId).ToList();
            //creating list for role ids
            List<string> roleas = new List<string>();
            //loop and add in roles ids list
            foreach (var item in userrole)
            {
                roleas.Add(item.RoleId);

            }
            //Check if user is super User
            bool IsSuperUser = false;

            //loop role if user is super user then allow him every thing
            foreach (var item in roleas)
            {
                if (_db.Roles.Where(x => x.Id == item).Select(x => x.Name).FirstOrDefault() == SD.Role_Super_Admin)
                {
                    IsSuperUser = true;
                    return;
                }
            }
            
            //get all allowed things in roles
           // var allMenusInRole = _unitOfWork.RoleRight.GetAll(x => roleas.Contains(x.Role_Id) && x.MenuId == menuID && x.IsActive == true).ToList();
            int isInRoleAllowed = 0;
            //if (allMenusInRole.Count() != 0)
            //{
                //create list for all role rights id
                //List<int> allowedRoleRightsids = new List<int>();
                //loop role rights and add ids in list
                //foreach (var item in allMenusInRole)
                //{
                //    allowedRoleRightsids.Add(item.Id);

                //}
                //get allowed action in role to allow him/her
                isInRoleAllowed = _unitOfWork.RoleRightAction.GetAll(x => roleas.Contains(x.Role_Id) && x.ActionId == actionID && x.IsActive == true).Count();
            //}
            
            //=================================================================================================================================
            //Role Rights Work END
            //=================================================================================================================================

            //=================================================================================================================================
            //User Rights Work END
            //=================================================================================================================================
            //get allowed menu to user 
           // var allMenusToUser = _unitOfWork.UserRightAction.GetAll(x => x.Userid == userId && x.MenuId == menuID && x.IsActive == true).ToList();

            int isInUserAllowed = 0;
            //if (allMenusToUser.Count() != 0)
            //{
            //    //create list for menu ids
            //    List<int> allowedUserRightsids = new List<int>();
            //    //loop user rights and add ids in list
            //    foreach (var item in allMenusToUser)
            //    {
            //        allowedUserRightsids.Add(item.Id);
            //    }
                //get allowed User action to allow him/her
                isInUserAllowed = _unitOfWork.UserRightAction.GetAll(x => x.MenuId == menuID && x.ActionId == actionID && x.IsActive == true).Count();

            //}


            if (isInRoleAllowed > 0 || isInUserAllowed > 0)
            {

            }else
            {
                filterContext.Result = new BadRequestObjectResult("URL NOT ALLOWED");

            }

            //if (_unitOfWork.Menu.GetAll(x => x.ControllerName == Controller && x.IsActive == true).Count() == 0)
            //{
               
            //}



           
        }
    }

}