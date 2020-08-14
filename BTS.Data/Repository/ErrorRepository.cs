using System;
using BTS.Model.Models;
using BTS.Data.Logs;
using BTS.Data.Infrastructure;

namespace BTS.Data.Repositories
{
    public interface IErrorRepository : IRepository<Error>
    {
    }

    public class ErrorRepository : LogRepositoryBase<Error>, IErrorRepository
    {
        public ErrorRepository(ILogDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}