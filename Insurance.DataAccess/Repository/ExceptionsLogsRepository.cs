using Insurance.DataAccess.Data;
using Insurance.DataAccess.Repository.IRepository;
using Insurance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insurance.DataAccess.Repository
{
    public class ExceptionsLogsRepository : RepositoryAsync<ExceptionsLogs>, IExceptionsLogsRepository
    {
        private readonly ApplicationDbContext _db;

        public ExceptionsLogsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        
    }
}
