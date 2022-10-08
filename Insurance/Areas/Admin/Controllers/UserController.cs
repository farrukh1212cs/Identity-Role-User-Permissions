using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Insurance.ActionFilters;
using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using Insurance.Models.ViewModels;
using Insurance.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Insurance.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Super_Admin )]
    [ServiceFilter(typeof(ValidateNameParameterAttribute))]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        ILogger<UserController> _logger;

        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,           
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment hostEnvironment, 
            ILogger<UserController> logger)
        {
            _db = db;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {

       
            return View();
        }

        public async Task<IActionResult> Upsert(string? id)
        {

            var roles = _db.Roles.Where(x=>x.Name != SD.Role_Super_Admin).ToList();

            var teams = await _unitOfWork.Team.GetAllAsync(x => x.IsActive == true);


            var user = new UserRegisterInputModel
            {
            
                Email = null,
                StreetAddress = null,
                City = null,
                State = null,
                PostalCode = null,
                Name = null,
                PhoneNumber = null,
                Role = null,
                Id = null
            };

            UserVM userVM = new UserVM()
            {
                User = user,
                RoleList = roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                TeamsList = teams.Select(i => new SelectListItem
                {
                    Text = i.TeamName,
                    Value = i.Id.ToString()
                })

            };



            if (id == null)
            {
                return PartialView("_Upsert", userVM);
            }
            if (id != null)
            {        
                var uuser = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);

                var userRole = _db.UserRoles.ToList();
                var userteam = await _unitOfWork.UserTeam.GetFirstOrDefaultAsync(x => x.UserID == id);
                userVM.User.Id = uuser.Id;
                userVM.User.Name = uuser.Name;
                userVM.User.Email = uuser.Email;
                userVM.User.StreetAddress = uuser.StreetAddress;
                userVM.User.City = uuser.City;
                userVM.User.State = uuser.State;
                userVM.User.PostalCode = uuser.PostalCode;
                userVM.User.PhoneNumber = uuser.PhoneNumber;
                userVM.User.Is_Active = uuser.Is_Active;
                userVM.User.Role = userRole.Where(x => x.UserId == id).Select(x=>x.RoleId).FirstOrDefault();
                userVM.UserTeamID = userteam.TeamId;




            }


            if (userVM.User == null)
            {
                return NotFound();
            }
            return PartialView("_Upsert", userVM);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(UserVM userVm1)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);

            string message = "Data Saved Successful";
               

            if (userVm1.User.Id == null)
            {
                string Password = string.Empty;
                bool EmailConfirm = false;

                string Is_UseDefaultPassword = "YES"; //  _db.Setups.Where(x => x.Key == "IsUseDefaultPassword").Select(x => x.Value).FirstOrDefault();
                string Is_EmailConfirmed = "YES"; //  _db.Setups.Where(x => x.Key == "EmailConfirmed").Select(x => x.Value).FirstOrDefault();
                string Is_SendEmailConfirmed = "NO"; //   _db.Setups.Where(x => x.Key == "IsSendEmailConfirmed").Select(x => x.Value).FirstOrDefault();
                string Is_IsSendPasswordOnEmail = "NO"; //  _db.Setups.Where(x => x.Key == "IsSendPasswordOnEmail").Select(x => x.Value).FirstOrDefault();

                if(Is_EmailConfirmed.ToUpper() == "YES")
                {
                    EmailConfirm = true;
                }

                if(Is_UseDefaultPassword.ToUpper() == "YES")
                {
                    Password = "Bok@12345"; //_db.Setups.Where(x => x.Key == "DefaultPassword").Select(x => x.Value).FirstOrDefault();
                }else
                {
                    Password = PasswordProvider.Generate();
                }


                string roleid0 = "";
                if (userVm1.User.Role == null)
                {
                    roleid0 = _db.Roles.Where(x => x.Id == SD.Role_Client).Select(x => x.Id).FirstOrDefault();
                }
                else
                {
                    roleid0 = _db.Roles.Where(x => x.Id == userVm1.User.Role).Select(x => x.Id).FirstOrDefault();
                }

              

                var result=    _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = userVm1.User.Email,
                    Email = userVm1.User.Email,
                    EmailConfirmed = EmailConfirm,
                    Name = userVm1.User.Name,
                    StreetAddress = userVm1.User.StreetAddress,
                    State = userVm1.User.State,
                    City = userVm1.User.City,
                    PhoneNumber = userVm1.User.PhoneNumber,
                    Is_Active = userVm1.User.Is_Active,
                    JoiningDate = DateTime.Now,
                    CreatedDate = DateTime.Now,                 
                    ActualRole = roleid0
                }, Password).GetAwaiter().GetResult();


                _logger.LogInformation("User created a new account with password.");

                if (result.Succeeded)
                {
                    ApplicationUser user = _db.ApplicationUsers.Where(u => u.Email == userVm1.User.Email).FirstOrDefault();
                    
                    string roleid = "" ;
                    if (userVm1.User.Role == null)
                    {
                        roleid = _db.Roles.Where(x => x.Id == SD.Role_Client).Select(x => x.Id).FirstOrDefault();
                        _userManager.AddToRoleAsync(user, SD.Role_Client).GetAwaiter().GetResult();
                    }else
                    {
                       string role = _db.Roles.Where(x => x.Id == userVm1.User.Role).Select(x => x.Name).FirstOrDefault();
                        roleid = _db.Roles.Where(x => x.Id == userVm1.User.Role).Select(x => x.Id).FirstOrDefault();
                        _userManager.AddToRoleAsync(user, role).GetAwaiter().GetResult();
                    }
                    //Email Confirmation New User
                    if (Is_SendEmailConfirmed.ToUpper() == "YES")
                    {
                        GenerateConfirmationEmail(user, Password);

                    }
                    //SendPassword On Email
                    if (Is_IsSendPasswordOnEmail.ToUpper() == "YES")
                    {
                        GeneratePasswordEmail(user, Password);
                    }

                    UserTeam team = new UserTeam();
                    team.UserID = user.Id;
                    team.TeamId = userVm1.UserTeamID;
                    team.IsActive = true;
                    team.CreatedBy = userId.Value.ToString();
                    team.CreatedDate = DateTime.Now;
                    team.RoleID = roleid;

                    _unitOfWork.UserTeam.AddAsync(team);
                    _unitOfWork.Save(userId.Value.ToString());



                }
               
                
            }
            else
            {
                ApplicationUser uu = _userManager.Users.Where(x => x.Id == userVm1.User.Id).FirstOrDefault();

                    uu.UserName = userVm1.User.Email;
                    uu.Email = userVm1.User.Email;
                    uu.EmailConfirmed = true;
                    uu.Name = userVm1.User.Name;
                    uu.StreetAddress = userVm1.User.StreetAddress;
                    uu.City = userVm1.User.City;
                    uu.State = userVm1.User.State;
                    uu.PhoneNumber = userVm1.User.PhoneNumber;
                    uu.Role = userVm1.User.Role;
                    uu.Is_Active = userVm1.User.Is_Active;
                    uu.ModfiedDate = DateTime.Now;

                var result2 = _userManager.UpdateAsync(uu).GetAwaiter().GetResult();               

                if (result2.Succeeded)
                {
                    ApplicationUser user = _db.ApplicationUsers.Where(u => u.Email == userVm1.User.Email).FirstOrDefault();



                    if (userVm1.User.Role == null)
                    {
                       
                    }
                    else
                    {
                        var oldroles = _db.UserRoles.Where(x => x.UserId == user.Id);

                        _db.UserRoles.RemoveRange(oldroles);
                        _db.SaveChanges(user.Id);
                        var ifroleExists = _db.UserRoles.Where(x => x.RoleId == userVm1.User.Role && x.UserId == user.Id).FirstOrDefault();
                        if(ifroleExists == null)
                        {
                            string role = _db.Roles.Where(x => x.Id == userVm1.User.Role).Select(x => x.Name).FirstOrDefault();

                            _userManager.AddToRoleAsync(user, role).GetAwaiter().GetResult();
                        }
                       
                    }

                    UserTeam userteam1 = await _unitOfWork.UserTeam.GetFirstOrDefaultAsync(x => x.UserID == userVm1.User.Id);
                    userteam1.TeamId = userVm1.UserTeamID;
                    _unitOfWork.Save(userId.Value.ToString());


                }

                message = "Data Updated Successful";
            }

            return Json(new { success = true, message = message });

        }
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _db.ApplicationUsers.Where(x=>x.Is_Active == true && x.Email != "farrukh1212cs@gmail.com").ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach(var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                string [] Rola = userRole.Where(x => x.UserId == user.Id).Select(x => x.RoleId).ToArray();
                string Rss = string.Empty;
                foreach (var item in Rola)
                {
                    if(Rola[Rola.Count() - 1] != item)
                    {
                        Rss += roles.FirstOrDefault(x => x.Id == item).Name + " , ";
                    }
                    else
                    {
                        Rss += roles.FirstOrDefault(x => x.Id == item).Name;
                    }
                   
                }                
                user.Role = Rss;
            }

            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if(objFromDb.LockoutEnd!=null && objFromDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked, we will unlock them
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
           _db.SaveChanges();
            return Json(new { success = true, message = "Operation Successful." });
        }

        [HttpPost]
        public async Task<IActionResult> PasswordReset([FromBody] string id)
        {


            ApplicationUser applicationUser = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
            var code = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
 

                string password = string.Empty;


            //if (_db.Setups.Where(x=>x.Key == "IsUseDefaultPassword").Select(x=>x.Value).FirstOrDefault().ToUpper() == "YES")
            //{
            //    password = _db.Setups.Where(x => x.Key == "DefaultPassword").Select(x => x.Value).FirstOrDefault();
            //}else
            //{
            //    password = PasswordProvider.PasssCode();
            //}

            var result = await _userManager.ResetPasswordAsync(applicationUser, code, password);
          
            return Json(new { success = true, message = "Operation Successful." });
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == id);
            if (user != null)
            {


                user.Is_Active = false;

                _unitOfWork.Save(userId.Value.ToString());
            }

      

            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion


        private async Task<bool> GeneratePasswordEmail(ApplicationUser user,string Password)
        {
            var callbackUrl = Url.Page(
                      "/Identity/Account/Login",
                      pageHandler: null,
                      null,
                      protocol: Request.Scheme);


            //var PathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
            //    + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
            //    + Path.DirectorySeparatorChar.ToString() + "Confirm_Account_Registration.html";

            var subject = "New Password";
            string HtmlBody = "";
            //using (StreamReader streamReader = System.IO.File.OpenText(PathToFile))
            //{
            //    HtmlBody = streamReader.ReadToEnd();
            //}

            //{0} : Subject  
            //{1} : DateTime  
            //{2} : Name  
            //{3} : Email  
            //{4} : Message  
            //{5} : callbackURL  

            string Message = $"Your New Password Is : " + Password;

            string messageBody = string.Format(HtmlBody,
                subject,
                String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                user.Name,
                user.Email,
                Message,
                callbackUrl
                );


            await _emailSender.SendEmailAsync(user.Email, "New Password", messageBody);
            return true;
        }

        private async Task<bool> GenerateConfirmationEmail(ApplicationUser user, string Password)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code },
                protocol: Request.Scheme);


            var PathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
                + Path.DirectorySeparatorChar.ToString() + "Confirm_Account_Registration.html";

            var subject = "Confirm Account Registration";
            string HtmlBody = "";
            using (StreamReader streamReader = System.IO.File.OpenText(PathToFile))
            {
                HtmlBody = streamReader.ReadToEnd();
            }

            //{0} : Subject  
            //{1} : DateTime  
            //{2} : Name  
            //{3} : Email  
            //{4} : Message  
            //{5} : callbackURL  

            string Message = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";

            string messageBody = string.Format(HtmlBody,
                subject,
                String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                user.Name,
                user.Email,
                Message,
                callbackUrl
                );


            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", messageBody);

            return true;
        }
    }


   
}