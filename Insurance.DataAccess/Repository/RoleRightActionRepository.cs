using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.DataAccess.Repository
{
    public class RoleRightActionRepository : Repository<RoleRightAction> , IRoleRightActionRepository
    {
        private readonly ApplicationDbContext _db;
        public RoleRightActionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(RoleRightAction rightactions)
        {
            var objFromDb = _db.RoleRightActions.FirstOrDefault(s => s.Id == rightactions.Id);
            if (objFromDb != null)
            {
                objFromDb.RoleRightId = rightactions.RoleRightId;
                objFromDb.ActionId = rightactions.ActionId;
                objFromDb.IsActive = rightactions.IsActive;
                objFromDb.ModifiedBy = rightactions.ModifiedBy;
                objFromDb.ModifiedDate = rightactions.ModifiedDate;               



            }
        }
    }
}
