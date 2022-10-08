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
    public class UserRightActionRepository : Repository<UserRightAction>, IUserRightActionRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRightActionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(UserRightAction userrightactions)
        {
            var objFromDb = _db.UserRightActions.FirstOrDefault(s => s.Id == userrightactions.Id);
            if (objFromDb != null)
            {
                objFromDb.UserRightId = userrightactions.UserRightId;
                objFromDb.ActionId = userrightactions.ActionId;
                objFromDb.IsActive = userrightactions.IsActive;
                objFromDb.ModifiedBy = userrightactions.ModifiedBy;
                objFromDb.ModifiedDate = userrightactions.ModifiedDate;



            }
        }
    }
}
