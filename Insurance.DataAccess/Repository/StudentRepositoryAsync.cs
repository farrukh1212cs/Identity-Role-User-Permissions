using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insurance.DataAccess.Repository
{
    public class StudentRepositoryAsync : RepositoryAsync<Student>, IStudentRepositoryAsync
    {
        private readonly ApplicationDbContext _db;

        public StudentRepositoryAsync(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Student obj)
        {
            var objFromDb = _db.Student.FirstOrDefault(s => s.Id == obj.Id);
            if (objFromDb != null)
            {   
                objFromDb.Name = obj.Name;
                objFromDb.RollNo = obj.RollNo;

            }
        }
    }
}
