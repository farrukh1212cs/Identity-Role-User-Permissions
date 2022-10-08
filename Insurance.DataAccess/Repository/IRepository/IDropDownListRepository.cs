using Insurance.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.DataAccess.Repository.IRepository
{
    public interface IDropDownListRepository : IRepositoryAsync<DropDownList>
    {
        void Update(DropDownList list);
    }
}
