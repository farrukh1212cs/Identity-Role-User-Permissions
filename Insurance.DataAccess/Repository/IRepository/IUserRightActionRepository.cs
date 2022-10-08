using Insurance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.DataAccess.Repository.IRepository
{
     public interface IUserRightActionRepository : IRepository<UserRightAction>
    {
        void Update(UserRightAction rolerightActions);
    }
}
