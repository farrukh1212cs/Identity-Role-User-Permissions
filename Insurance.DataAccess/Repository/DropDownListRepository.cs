using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insurance.DataAccess.Repository
{
    public class DropDownListRepository : RepositoryAsync<DropDownList>, IDropDownListRepository
    {
        private readonly ApplicationDbContext _db;

        public DropDownListRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(DropDownList dropDownList)
        {
            var objFromDb = _db.DropDownList.FirstOrDefault(s => s.Id == dropDownList.Id);
            //if (objFromDb != null)
            //{
            //    objFromDb.Name = product.Name;        
            //    objFromDb.IsActive = product.IsActive;
               
            //}
        }
    }
}
