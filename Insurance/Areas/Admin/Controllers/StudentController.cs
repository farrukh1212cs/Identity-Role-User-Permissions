using Insurance.ActionFilters;
using Insurance.DataAccess.Repository;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using Insurance.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Super_Admin)]
    //[ServiceFilter(typeof(ValidateNameParameterAttribute))]
    public class StudentController : Controller
    {
        public IUnitOfWork _unitofwork { get; set; }


        public StudentController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        //ADO.Net
        //LINQ
        //Dapper

       // -----------------------------


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> GetAll()
        {
            var students = await _unitofwork.Student.GetAllAsync();
            return Json(new { data = students }); ;
       }



       
    }
}
