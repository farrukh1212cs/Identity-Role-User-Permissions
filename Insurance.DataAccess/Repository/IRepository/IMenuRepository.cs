using Insurance.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.DataAccess.Repository.IRepository
{
    public interface IMenuRepository : IRepository<Menu>
    {
        void Update(Menu menu);
    }
}
