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
    public class RoleRightController : Controller
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public RoleRightController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        //#region RoleRight
        public IActionResult Index()
        {
            return View();
        }     

     
        public IActionResult EditRolePermission(string iD)
        {
            var role = _db.Roles.Where(x => x.Id == iD).FirstOrDefault();
            var roleactions = _unitOfWork.RoleRightAction.GetAll(x => x.IsActive == true ).ToList();

            ViewBag.Role = role;

            List<TreeViewNode> nodes = new List<TreeViewNode>();
            
            IEnumerable<Menu> menu = _unitOfWork.Menu.GetAll(x => x.IsActive == true);

            IEnumerable<Aaction> aactions = _unitOfWork.Aaction.GetAll(x => x.IsActive == true);

            foreach (var item in menu.Where(x=>x.MenuUnder == 0 && x.IsSingle == false))
            {


                //nodes.Add(new TreeViewNode { id = "head_"+ item.Id.ToString(), parent = "#", text = "Area : " + item.AreaName + "=>"+ item.MenuName  });
                var chkapi = menu.FirstOrDefault(x => x.Id == item.Id && x.IsActive == true && x.API == true);
                if (chkapi != null)
                {

                    var chckacctionallowed = roleactions.FirstOrDefault(x => x.IsActive == true && x.MenuId == chkapi.Id && x.Role_Id == role.Id);
                    if (chckacctionallowed != null)
                    {
                        State checkState1 = new State();
                        checkState1.selected = true;
                        nodes.Add(new TreeViewNode { id = "head_" + item.Id.ToString(), parent = "#", text = item.MenuName, state = checkState1 });
                    }
                    else
                    {
                        nodes.Add(new TreeViewNode { id = "head_" + item.Id.ToString(), parent = "#", text = item.MenuName });
                    }
                }
                else
                {
                    nodes.Add(new TreeViewNode { id = "head_" + item.Id.ToString(), parent = "#", text = item.MenuName });
                }

                foreach (var item2 in menu.Where(x=>x.MenuUnder == Convert.ToInt32(item.Id) && x.IsActive == true && x.MenuUnder != 0) )
                {
                    nodes.Add(new TreeViewNode { id = "Menu_" + item2.Id.ToString() + "-" + item2.Id.ToString(), parent = "head_" + item2.MenuUnder.ToString(), text = item2.MenuName});



                    foreach (var item3 in aactions.Where(x => x.Menu_Ids == Convert.ToInt32(item2.Id)))
                    {
                        RoleRightAction Check =  roleactions.Where(x=> x.Role_Id == role.Id && x.MenuId == Convert.ToInt32(item2.Id) && x.ActionId == Convert.ToInt32(item3.Id)).FirstOrDefault();
                        State checkState = new State();
                        checkState.selected = false;
                        if (Check != null)
                        {
                            checkState.selected = true;
                        }
                       nodes.Add(new TreeViewNode { id = "Action_" + item3.Id.ToString() , parent = "Menu_" + item3.Menu_Ids.ToString() + "-" + item3.Menu_Ids.ToString(), text = item3.ActionName , state = checkState});
                        
                    }
                } 
               
               

            }

            foreach (var item in menu.Where(x => x.MenuUnder == 0 && x.IsSingle == true))
            {
                nodes.Add(new TreeViewNode { id = "head_" + item.Id.ToString(), parent = "#", text = "Area : " + item.AreaName + "=>" + item.MenuName });
              
                nodes.Add(new TreeViewNode { id = "Menu_" + item.Id.ToString() + "-" + item.Id.ToString(), parent = "head_" + item.Id.ToString(), text = item.MenuName });

                foreach (var item3 in aactions.Where(x => x.Menu_Ids == Convert.ToInt32(item.Id)))
                {
                    RoleRightAction Check = roleactions.Where(x => x.Role_Id == role.Id && x.MenuId == Convert.ToInt32(item.Id) && x.ActionId == Convert.ToInt32(item3.Id)).FirstOrDefault();
                    State checkState = new State();
                    checkState.selected = false;
                    if (Check != null)
                    {
                        checkState.selected = true;
                    }
                    nodes.Add(new TreeViewNode { id = "Action_" + item3.Id.ToString(), parent = "Menu_" + item3.Menu_Ids.ToString() + "-" + item3.Menu_Ids.ToString(), text = item3.ActionName, state = checkState });

                }
            }

            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return PartialView("_Permissions");
        }
    
        [HttpPost]
        public IActionResult EditRolePermission(string selectedItems,string roleID)
        {
            string message = "Data Saved Successful";
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);
            IEnumerable<Aaction> aactions = _unitOfWork.Aaction.GetAll(x => x.IsActive == true);

            _unitOfWork.RoleRightAction.RemoveRange(_unitOfWork.RoleRightAction.GetAll(x => x.Role_Id == roleID));
            _unitOfWork.Save(userId.Value.ToString());

            foreach (var item in items)
            {

                if(item.id.Contains("Action_"))
                {
                        RoleRightAction roleRightAction = new RoleRightAction();
                        //Aaction aaction = new Aaction();
                   
                        roleRightAction.RoleRightId = 0;
                        roleRightAction.ActionId = Convert.ToInt32(item.id.Replace("Action_", ""));
                        roleRightAction.IsActive = true;
                        roleRightAction.CreatedBy = userId.Value.ToString();
                        roleRightAction.CreatedDate = DateTime.Now;
                        roleRightAction.MenuId = aactions.Where(x => x.Id == Convert.ToInt32(item.id.Replace("Action_", ""))).Select(x => x.Menu_Ids).FirstOrDefault();
                        roleRightAction.Role_Id = roleID;
                        _unitOfWork.RoleRightAction.Add(roleRightAction);
                        _unitOfWork.Save(userId.Value.ToString());                    
                }
                else if (item.id.Contains("head_"))
                {

                    //--------------check if api link
                    var chk = _unitOfWork.Menu.GetFirstOrDefault(x => x.Id == Convert.ToInt32(item.id.Replace("head_", "")) && x.IsActive == true && x.API == true);
                    if (chk != null)
                    {
                        RoleRightAction roleRightAction = new RoleRightAction();
                        Aaction aaction = new Aaction();

                        roleRightAction.RoleRightId = 0;
                        roleRightAction.ActionId = 0;
                        roleRightAction.IsActive = true;
                        roleRightAction.CreatedBy = userId.Value.ToString();
                        roleRightAction.CreatedDate = DateTime.Now;
                        roleRightAction.MenuId = Convert.ToInt32(item.id.Replace("head_", "")); //aactions.Where(x => x.Id == Convert.ToInt32(item.id.Replace("head_", ""))).Select(x => x.Menu_Ids).FirstOrDefault();
                        roleRightAction.Role_Id = roleID;

                        _unitOfWork.RoleRightAction.Add(roleRightAction);
                        _unitOfWork.Save(userId.Value.ToString());
                    }

                }

            }



            return Json(new { success = true, message = message });
        }
        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _db.Roles.Where(x=>x.Name != SD.Role_Super_Admin).ToList();

           

            return Json(new { data = roles.OrderBy(x => x.Id) });
        }
    }
}
