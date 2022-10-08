using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insurance.DataAccess.Repository
{
    public class TeamRepository : RepositoryAsync<Team>, ITeamRepository
    {
        private readonly ApplicationDbContext _db;

        public TeamRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Team obj)
        {
            var objFromDb = _db.Teams.FirstOrDefault(s => s.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.TeamName = obj.TeamName;        
                objFromDb.TeamDescription = obj.TeamDescription;                
                objFromDb.IsActive = obj.IsActive;
               
            }
        }
    }
}
