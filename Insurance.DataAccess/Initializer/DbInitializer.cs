using Insurance.DataAccess.Data;
using Insurance.Models;
using Insurance.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insurance.DataAccess.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch(Exception)
            {
                
            }


            if (_db.Roles.Any(r => r.Name == SD.Role_Super_Admin)) return;
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Company_User)).GetAwaiter().GetResult();
           
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Super_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Company_User)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Managers)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Quality_Control)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Schedulers)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Team_Leader)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Inspector)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_1099)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Enterprise)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Client)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Guest)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "farrukh1212cs@gmail.com",
                Email = "farrukh1212cs@gmail.com",
                EmailConfirmed = true,
                Is_Active = true,
                Name = "Farrukh Rehman"
            },"Admin@123").GetAwaiter().GetResult() ;

            ApplicationUser user = _db.ApplicationUsers.Where(u => u.Email == "farrukh1212cs@gmail.com").FirstOrDefault();

            _userManager.AddToRoleAsync(user, SD.Role_Super_Admin).GetAwaiter().GetResult();

            string Created_by = _db.ApplicationUsers.Where(x => x.Email == "farrukh1212cs@gmail.com").Select(x => x.Id).FirstOrDefault();

            //========================================================================================================
            //---------------------Content Management
            Menu menu = new Menu
            {
                ControllerName = "",
                ActionName = "",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Admin",
                MenuName = "Content Management",
                MenuURL = "",
                MenuUnder = 0,
                MenuOrder = 1,
                MenuIcon = "fas fa-tachometer-alt"

            };
            _db.Menus.Add(menu);
            _db.SaveChanges();
            //---------------------Menus
            int MenuUnder = _db.Menus.Where(x => x.MenuName == "Content Management").Select(x => x.Id).FirstOrDefault();
            Menu menu2 = new Menu
            {
                ControllerName = "Menu",
                ActionName = "Index",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Admin",
                MenuName = "Menus",
                MenuURL = "/Admin/Menu/Index",
                MenuUnder = MenuUnder,
                MenuOrder = 1,
                MenuIcon = "far fa-circle "

            };
            _db.Menus.Add(menu2);
            //---------------------Sub Menu
            Menu menu3 = new Menu
            {
                ControllerName = "SubMenu",
                ActionName = "Index",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Admin",
                MenuName = "Sub Menus",
                MenuURL = "/Admin/SubMenu/Index",
                MenuUnder = MenuUnder,
                MenuOrder = 2,
                MenuIcon = "far fa-circle "

            };
            _db.Menus.Add(menu3);
            //---------------------Actions
            Menu menu4 = new Menu
            {
                ControllerName = "Aaction",
                ActionName = "Index",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Admin",
                MenuName = "Actions",
                MenuURL = "/Admin/Aaction/Index",
                MenuUnder = MenuUnder,
                MenuOrder = 3,
                MenuIcon = "far fa-circle "

            };
            _db.Menus.Add(menu4);
            _db.SaveChanges();

            //=========================================================================================================
            //---------------------Users
            Menu menu5 = new Menu
            {
                ControllerName = "",
                ActionName = "",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Admin",
                MenuName = "Users",
                MenuURL = "",
                MenuUnder = 0,
                MenuOrder = 2,
                MenuIcon = "fas fa-tachometer-alt"

            };
            _db.Menus.Add(menu5);
            _db.SaveChanges();
            //---------------------Application Users
            int MenuUnder1 = _db.Menus.Where(x => x.MenuName == "Users").Select(x => x.Id).FirstOrDefault();
            Menu menu6 = new Menu
            {
                ControllerName = "User",
                ActionName = "Index",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Admin",
                MenuName = "Application Users",
                MenuURL = "/Admin/User/Index",
                MenuUnder = MenuUnder1,
                MenuOrder = 1,
                MenuIcon = "far fa-circle "

            };
            _db.Menus.Add(menu6);
            _db.SaveChanges(); 
            
            Menu Teams = new Menu
            {
                ControllerName = "Team",
                ActionName = "Index",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Admin",
                MenuName = "Teams",
                MenuURL = "/Admin/Team/Index",
                MenuUnder = MenuUnder1,
                MenuOrder = 2,
                MenuIcon = "far fa-circle "

            };
            _db.Menus.Add(Teams);
            _db.SaveChanges();

            //=========================================================================================================
            //---------------------Rights
            Menu menu7 = new Menu
            {
                ControllerName = "",
                ActionName = "",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Admin",
                MenuName = "Rights",
                MenuURL = "",
                MenuUnder = 0,
                MenuOrder = 3,
                MenuIcon = "fas fa-tachometer-alt"

            };
            _db.Menus.Add(menu7);
            _db.SaveChanges();
      
            int MenuUnder2 = _db.Menus.Where(x => x.MenuName == "Rights").Select(x => x.Id).FirstOrDefault();
            //---------------------Role Rights
            Menu menu8 = new Menu
            {
                ControllerName = "RoleRight",
                ActionName = "Index",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Admin",
                MenuName = "Role Rights",
                MenuURL = "/Admin/RoleRight/Index",
                MenuUnder = MenuUnder2,
                MenuOrder = 1,
                MenuIcon = "far fa-circle "

            };
            _db.Menus.Add(menu8);
            _db.SaveChanges();
            //---------------------User Rights
            Menu menu9 = new Menu
            {
                ControllerName = "UserRight",
                ActionName = "Index",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Admin",
                MenuName = "User Rights",
                MenuURL = "/Admin/UserRight/Index",
                MenuUnder = MenuUnder2,
                MenuOrder = 2,
                MenuIcon = "far fa-circle "

            };
            _db.Menus.Add(menu9);
            _db.SaveChanges();


            //--------------------
            //---------------------Dashboard
            Menu Dashboard = new Menu
            {
                ControllerName = "",
                ActionName = "",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Admin",
                MenuName = "Dashboard",
                MenuURL = "",
                MenuUnder = 0,
                MenuOrder = 1,
                MenuIcon = "fas fa-tachometer-alt"

            };
            _db.Menus.Add(Dashboard);
            _db.SaveChanges();
            //---------------------Home
            int HomeUnder = _db.Menus.Where(x => x.MenuName == "Dashboard" && x.AreaName == "Admin").Select(x => x.Id).FirstOrDefault();
            Menu Home = new Menu
            {
                ControllerName = "Home",
                ActionName = "Index",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Admin",
                MenuName = "Home",
                MenuURL = "/Admin/Home/Index",
                MenuUnder = HomeUnder,
                MenuOrder = 1,
                MenuIcon = "far fa-circle "

            };
            _db.Menus.Add(Home);
            _db.SaveChanges();


            //----------------------------------------------------------------------------------------
            //---------------------testing

            Menu testing = new Menu
            {
                ControllerName = "",
                ActionName = "",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Client",
                MenuName = "Testing",
                MenuURL = "",
                MenuUnder = 0,
                MenuOrder = 1,
                MenuIcon = "fas fa-tachometer-alt"

            };
            _db.Menus.Add(testing);
            _db.SaveChanges();

            int testingUnder = _db.Menus.Where(x => x.MenuName == "Testing" && x.AreaName == "Client").Select(x => x.Id).FirstOrDefault();
            //---------------------Test controller
            Menu Test = new Menu
            {
                ControllerName = "Test",
                ActionName = "Index",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Client",
                MenuName = "Test",
                MenuURL = "/Client/Test/Index",
                MenuUnder = testingUnder,
                MenuOrder = 1,        
                MenuIcon = "far fa-circle "

            };
            _db.Menus.Add(Test);
            _db.SaveChanges();
            //--------------------------------Add Action
            Aaction aaction = new Aaction
            {
                 ActionName = "Index",
                 IsActive = true,
                 CreatedBy = Created_by,
                 CreatedDate = DateTime.Now,
                 Menu_Ids = Test.Id

            };
            _db.Aactions.Add(aaction);
            _db.SaveChanges();

            //---------------------API Test controller
            Menu APITtest = new Menu
            {
                ControllerName = "APITest",
                ActionName = "GetAll",
                IsActive = true,
                CreatedBy = Created_by,
                CreatedDate = DateTime.Now,
                AreaName = "Client",
                MenuName = "API=>Client=>APITest=>GetAll",
                MenuURL = "/Client/APITest/GetAll",
                MenuUnder = 0,
                MenuOrder = 1,
                API = true,
                MenuIcon = "far fa-circle "

            };
            _db.Menus.Add(APITtest);
            _db.SaveChanges();
        }
    }
}
