using Insurance.ActionFilters;
using Insurance.DataAccess.Repository;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using Insurance.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
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
        public async Task<IActionResult> Upsert(int? id)
        {
            Student student = new Student();
            if (id == null)
            {
                //this is for create

                return PartialView("_Upsert", student);
            }
            //this is for edit
            student = await _unitofwork.Student.GetAsync(id.GetValueOrDefault());
            if (student == null)
            {
                return NotFound();
            }
            return PartialView("_Upsert", student);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Student student)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);

            string message = "Data Saved Successful";
            if (student.Id == 0)
            {
               
                await _unitofwork.Student.AddAsync(student);

            }
            else
            {
                _unitofwork.Student.Update(student);
                message = "Data Updated Successful";
            }
            _unitofwork.Save(userId.Value.ToString());
            return Json(new { success = true, message = message });

        }

        public async Task<IActionResult> GetAll()
        {
            var students = await _unitofwork.Student.GetAllAsync();
            return Json(new { data = students });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _unitofwork.Student.GetAsync(id);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (student != null)
            {

                await _unitofwork.Student.RemoveAsync(student);

                _unitofwork.Save(userId.Value.ToString());
            }

            return Json(new { success = true, message = "Delete Successful" });

        }



    }
}
