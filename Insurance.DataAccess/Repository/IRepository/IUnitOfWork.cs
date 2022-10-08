using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {


    
  
        IApplicationUserRepository ApplicationUser { get; }
        ISP_Call SP_Call { get; }
       
        IMenuRepository Menu { get; }  
     
        IAactionRepository Aaction { get; }
       
        IUserRightActionRepository UserRightAction { get; }
      
        IRoleRightActionRepository RoleRightAction { get; }
      
        ITeamRepository Team { get; }
        IUserTeamRepository UserTeam { get; }
      
      
        IExceptionsLogsRepository ExceptionsLogs { get; }
     
      
      

        void Save(string UserID);
        void SaveWH();


    }
}
