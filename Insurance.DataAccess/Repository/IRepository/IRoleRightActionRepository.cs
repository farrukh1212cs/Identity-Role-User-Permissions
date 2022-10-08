using Insurance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.DataAccess.Repository.IRepository
{
     public interface IRoleRightActionRepository : IRepository<RoleRightAction>
    {
        void Update(RoleRightAction rolerightActions);
    }
}
