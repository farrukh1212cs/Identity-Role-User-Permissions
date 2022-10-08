using Insurance.DataAccess.Data;
using Insurance.Models;
using Insurance.Models.ResponseModels;
using Insurance.Models.ServiceModels.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.UserService
{
    public interface IUserService1
    {
        //Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);

        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
        string GetUserId();
    }


    public class UserService1 : IUserService1
    {
        private IConfiguration _configuration;
        private UserManager<Insurance.Models.ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationDbContext _db;
        public UserService1(UserManager<ApplicationUser> userManager, IConfiguration configuration, IHttpContextAccessor context, ApplicationDbContext db)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _db = db;
        }
        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {

            var user = await _userManager.FindByNameAsync(model.Email);

            // || user.Is_Active == false
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid Email or Password",
                    IsSuccess = false,
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            //|| user.Is_Active == false
             if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid Email or Password",
                    IsSuccess = false,
                };
            }

            var roleid = _db.UserRoles.FirstOrDefault(x => x.UserId == user.Id).RoleId;

            var role = _db.Roles.FirstOrDefault(x => x.Id == roleid);
            var claims = new[]
            {
                new Claim("Email",model.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim("Id",user.Id),
                new Claim("Role",role.Name.ToUpper())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            DateTime dateTime;
            if (_configuration["AuthSettings:TokenExpiry"] == "DAY")
            {
                dateTime = DateTime.Now.AddDays(Convert.ToInt32(_configuration["AuthSettings:ExpiryAdded"]));
            }
            else if (_configuration["AuthSettings:TokenExpiry"] == "MIN")
            {
                dateTime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["AuthSettings:ExpiryAdded"]));
            }
            else if (_configuration["AuthSettings:TokenExpiry"] == "HOUR")
            {
                dateTime = DateTime.Now.AddHours(Convert.ToInt32(_configuration["AuthSettings:ExpiryAdded"]));
            }
            else
            {
                dateTime = DateTime.Now.AddMinutes(-1);
            }


            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: dateTime,
               signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                Role = role.Name.ToUpper(),
                ExpireDate = token.ValidTo
            };

        }


        public string GetUserId()
        {
            return _context.HttpContext.User.Claims
                       .First(i => i.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
    }
