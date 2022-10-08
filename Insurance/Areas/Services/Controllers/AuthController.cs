using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using Insurance.Models.ServiceModels.ViewModel;
using Insurance.UserService;
using Insurance.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Areas.Services.Controllers
{
    [Route("Services/api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService1 _userService;
        private readonly IUnitOfWork _unitOfWork;
        private ApplicationDbContext _db;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthController(UserManager<ApplicationUser> userManager, IEmailService emailService, IUserService1 userService, IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
            _db = db;
            _emailService = emailService;
            _userManager = userManager;
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);
                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Invalid Request");
        }


        [HttpPost("Forgot")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotAsync(ForgotPasswordVM model)
        {

          



            return Ok("Kindly check your email box");
        }

    }
}
