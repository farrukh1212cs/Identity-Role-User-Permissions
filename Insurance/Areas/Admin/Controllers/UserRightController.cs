using Insurance.ActionFilters;
using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using Insurance.Models.ViewModels;
using Insurance.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
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
    public class UserRightController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public IEnumerable<SelectListItem> UserList { get; set; }
        public UserRightController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _db = db;
            _unitOfWork = unitOfWork;
       
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EditRolePermission(string iD)
        {
            var user = _db.ApplicationUsers.Where(x => x.Id == iD ).FirstOrDefault();

            var useractions = _unitOfWork.UserRightAction.GetAll(x => x.IsActive == true).ToList();

            ViewBag.User = user;

            List<TreeViewNode> nodes = new List<TreeViewNode>();

            IEnumerable<Menu> menu = _unitOfWork.Menu.GetAll(x => x.IsActive == true);

            IEnumerable<Aaction> aactions = _unitOfWork.Aaction.GetAll(x => x.IsActive == true);

            foreach (var item in menu.Where(x => x.MenuUnder == 0))
            {

                var chkapi = menu.FirstOrDefault(x => x.Id == item.Id && x.IsActive == true && x.API == true);

                if (chkapi != null)
                {
                    var chckacctionallowed = useractions.FirstOrDefault(x => x.IsActive == true && x.MenuId == chkapi.Id && x.Userid == user.Id);
                    if (chckacctionallowed != null)
                    {
                        State checkState1 = new State();
                        checkState1.selected = true;
                        nodes.Add(new TreeViewNode { id = "head_" + item.Id.ToString(), parent = "#", text = item.MenuName, state = checkState1 });
                    } else
                    {
                        nodes.Add(new TreeViewNode { id = "head_" + item.Id.ToString(), parent = "#", text = item.MenuName });
                    }
                 
                }else
                {
                    nodes.Add(new TreeViewNode { id = "head_" + item.Id.ToString(), parent = "#", text = item.MenuName });
                }
                

                foreach (var item2 in menu.Where(x => x.MenuUnder == Convert.ToInt32(item.Id) && x.IsActive == true && x.MenuUnder != 0))
                {
                    nodes.Add(new TreeViewNode { id = "Menu_" + item2.Id.ToString() + "-" + item2.Id.ToString(), parent = "head_" + item2.MenuUnder.ToString(), text = item2.MenuName });



                    foreach (var item3 in aactions.Where(x => x.Menu_Ids == Convert.ToInt32(item2.Id)))
                    {
                        UserRightAction Check = useractions.Where(x => x.Userid == user.Id && x.MenuId == Convert.ToInt32(item2.Id) && x.ActionId == Convert.ToInt32(item3.Id)).FirstOrDefault();
                        State checkState = new State();
                        checkState.selected = false;
                        if (Check != null)
                        {
                            checkState.selected = true;
                        }
                        nodes.Add(new TreeViewNode { id = "Action_" + item3.Id.ToString(), parent = "Menu_" + item3.Menu_Ids.ToString() + "-" + item3.Menu_Ids.ToString(), text = item3.ActionName, state = checkState });

                    }
                }


            }
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return PartialView("_Permissions");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _db.ApplicationUsers.ToList().Where(x=> x.Email != "farrukh1212cs@gmail.com" && x.Is_Active == true );
            return Json(new { data = allObj });
        }

        [HttpPost]
        public IActionResult EditRolePermission(string selectedItems, string userid)
        {
            string message = "Data Saved Successful";
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);
            IEnumerable<Aaction> aactions = _unitOfWork.Aaction.GetAll(x => x.IsActive == true);

            _unitOfWork.UserRightAction.RemoveRange(_unitOfWork.UserRightAction.GetAll(x => x.Userid == userid));
            _unitOfWork.Save(userId.Value.ToString());

            foreach (var item in items)
            {

                if (item.id.Contains("Action_"))
                {
                    UserRightAction userRightAction = new UserRightAction();
                    Aaction aaction = new Aaction();

                    userRightAction.UserRightId = 0;
                    userRightAction.ActionId = Convert.ToInt32(item.id.Replace("Action_", ""));
                    userRightAction.IsActive = true;
                    userRightAction.CreatedBy = userId.Value.ToString();
                    userRightAction.CreatedDate = DateTime.Now;
                    userRightAction.MenuId = aactions.Where(x => x.Id == Convert.ToInt32(item.id.Replace("Action_", ""))).Select(x => x.Menu_Ids).FirstOrDefault();
                    userRightAction.Userid = userid;

                    _unitOfWork.UserRightAction.Add(userRightAction);
                    _unitOfWork.Save(userId.Value.ToString());

                }else if(item.id.Contains("head_"))
                {

                    //--------------check if api link
                    var chk = _unitOfWork.Menu.GetFirstOrDefault(x => x.Id == Convert.ToInt32(item.id.Replace("head_", "")) && x.IsActive == true && x.API == true);
                    if(chk != null)
                    {
                        UserRightAction userRightAction = new UserRightAction();
                        Aaction aaction = new Aaction();

                        userRightAction.UserRightId = 0;
                        userRightAction.ActionId = 0;
                        userRightAction.IsActive = true;
                        userRightAction.CreatedBy = userId.Value.ToString();
                        userRightAction.CreatedDate = DateTime.Now;
                        userRightAction.MenuId = Convert.ToInt32(item.id.Replace("head_", "")); //aactions.Where(x => x.Id == Convert.ToInt32(item.id.Replace("head_", ""))).Select(x => x.Menu_Ids).FirstOrDefault();
                        userRightAction.Userid = userid;

                        _unitOfWork.UserRightAction.Add(userRightAction);
                        _unitOfWork.Save(userId.Value.ToString());
                    }
                
                }

            }



            return Json(new { success = true, message = message });
        }


        #endregion
    }
}
