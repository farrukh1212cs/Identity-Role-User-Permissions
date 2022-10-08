using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insurance.DataAccess.Repository
{
    public class UserTeamRepository : RepositoryAsync<UserTeam>, IUserTeamRepository
    {
        private readonly ApplicationDbContext _db;

        public UserTeamRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(UserTeam obj)
        {
            var objFromDb = _db.UserTeam.FirstOrDefault(s => s.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.TeamId = obj.TeamId;        
                objFromDb.UserID = obj.UserID;                
                objFromDb.IsActive = obj.IsActive;
               
            }
        }
    }
}
