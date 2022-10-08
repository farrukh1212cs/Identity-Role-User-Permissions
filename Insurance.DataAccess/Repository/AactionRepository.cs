using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insurance.DataAccess.Repository
{
    public class AactionRepository : Repository<Aaction>, IAactionRepository
    {
        private readonly ApplicationDbContext _db;

        public AactionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Aaction aaction)
        {
            var objFromDb = _db.Aactions.FirstOrDefault(s => s.Id == aaction.Id);
            if (objFromDb != null)
            {
   
                objFromDb.ActionName = aaction.ActionName;
                objFromDb.IsActive = aaction.IsActive;
                objFromDb.ModifiedBy = aaction.ModifiedBy;
                objFromDb.ModifiedDate = aaction.ModifiedDate;
               


                //objFromDb.CreatedBy = menu.CreatedBy;
                //objFromDb.CreatedDate = menu.CreatedDate;


            }
        }
    }
}
