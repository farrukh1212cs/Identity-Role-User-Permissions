using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insurance.DataAccess.Repository
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private readonly ApplicationDbContext _db;

        public MenuRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Menu menu)
        {
            var objFromDb = _db.Menus.FirstOrDefault(s => s.Id == menu.Id);
            if (objFromDb != null)
            {
                objFromDb.ControllerName = menu.ControllerName;
                objFromDb.ActionName = menu.ActionName;
                objFromDb.IsActive = menu.IsActive;
                objFromDb.ModifiedBy = menu.ModifiedBy;
                objFromDb.ModifiedDate = menu.ModifiedDate;
                objFromDb.AreaName = menu.AreaName;
                objFromDb.MenuName = menu.MenuName;
                objFromDb.MenuURL = menu.MenuURL;
                objFromDb.MenuUnder = menu.MenuUnder;
                objFromDb.MenuOrder = menu.MenuOrder;
                objFromDb.MenuIcon = menu.MenuIcon;
                
                
                //objFromDb.CreatedBy = menu.CreatedBy;
                //objFromDb.CreatedDate = menu.CreatedDate;
                           
               
            }
        }
    }
}
