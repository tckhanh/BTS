using System;
using BTS.Model.Models;
using BTS.Data.Logs;
using BTS.Data.Infrastructure;

namespace BTS.Data.Repositories
{
    public interface IAuditRepository : IRepository<Audit>
    {
    }

    public class AuditRepository : LogRepositoryBase<Audit>, IAuditRepository
    {
        public AuditRepository(ILogDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}