using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Insurance.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Insurance.Utility;
using Insurance.Models;
using System.Security.Claims;
using Insurance.DataAccess.Data;

namespace Insurance.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        public LoginModel(ApplicationDbContext db, SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        //public async Task OnGetAsync(string returnUrl = null)
        //{
        //    //if (!string.IsNullOrEmpty(ErrorMessage))
        //    //{
        //    //    ModelState.AddModelError(string.Empty, ErrorMessage);
        //    //}

        //    //returnUrl = returnUrl ?? Url.Content("~/");

        //    //// Clear the existing external cookie to ensure a clean login process
        //    //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        //    //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        //    ReturnUrl = returnUrl;
        //}

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Admin/Home/Index");

            if (ModelState.IsValid)
            {
                var user = _db.ApplicationUsers.FirstOrDefault(x => x.Is_Active == true && x.Email == Input.Email);
                if (user == null)

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                  
                    _logger.LogInformation("User logged in.");

                    string userid = _db.ApplicationUsers.Where(x => x.Email == Input.Email).Select(x => x.Id).FirstOrDefault();

                    var userrole = _db.UserRoles.Where(x => x.UserId == userid).FirstOrDefault();
                    var role = _db.Roles.Where(x => x.Id == userrole.RoleId).FirstOrDefault(); 
                    //if (role.Name == SD.Role_Super_Admin || role.Name == SD.Role_Admin)
                    //{
                       

                    //}              


                    return LocalRedirect(returnUrl);
                }else
                {

                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
               // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }
            
            // If we got this far, something failed, redisplay form
            return Page();
        }

        //public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    var user = await _userManager.FindByEmailAsync(Input.Email);
        //    if (user == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
        //    }

        //    var userId = await _userManager.GetUserIdAsync(user);
        //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //    var callbackUrl = Url.Page(
        //        "/Account/ConfirmEmail",
        //        pageHandler: null,
        //        values: new { userId = userId, code = code },
        //        protocol: Request.Scheme);
        //    await _emailSender.SendEmailAsync(
        //        Input.Email,
        //        "Confirm your email",
        //        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        //    ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
        //    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        //    return Page();
        //}


        //public async Task<IActionResult> AccessDenied(string ReturnUrl)
        //{
        //    return BadRequest("Not Allowed");
        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            // workaround
            if (Request.Cookies["Identity.External"] != null)
            {
                return RedirectToAction("/", returnUrl);
            }
            return RedirectToAction("/");

        }
    }
}
