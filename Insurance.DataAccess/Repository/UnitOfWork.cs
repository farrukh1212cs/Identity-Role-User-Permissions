using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
       
            ApplicationUser = new ApplicationUserRepository(_db);
            SP_Call = new SP_Call(_db);
      
            Menu = new MenuRepository(_db);
        
            Aaction = new AactionRepository(_db);
      
            UserRightAction = new UserRightActionRepository(_db);

            RoleRightAction = new RoleRightActionRepository(_db);
        
            DropDownList = new DropDownListRepository(_db);
          
            Team = new TeamRepository(_db);
            UserTeam = new UserTeamRepository(_db);
          
            ExceptionsLogs = new ExceptionsLogsRepository(_db);

          

        }

      

        public IApplicationUserRepository ApplicationUser { get; private set; }
     
        public ISP_Call SP_Call { get; private set; }

      

        public IMenuRepository Menu { get; set; }
    
        public IAactionRepository Aaction { get; set; }
     
        public IUserRightActionRepository UserRightAction { get; set; }
    
        public IRoleRightActionRepository RoleRightAction { get; set; }
      
        public IDropDownListRepository DropDownList { get; set; }
     
        public ITeamRepository Team { get; set; }
        public IUserTeamRepository UserTeam { get; set; }
    
        public IExceptionsLogsRepository ExceptionsLogs { get; set; }
   
      

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save(string UserID)
        {
            _db.SaveChanges(UserID);
        }
        
        public void SaveWH()
        {
            _db.SaveChanges();
        }
    }
}
