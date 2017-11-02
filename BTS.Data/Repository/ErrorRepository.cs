using System;
using BTS.Model.Models;
using BTS.Data.InfraError;
using BTS.Data.Infrastructure;

namespace BTS.Data.Repositories
{
    public interface IErrorRepository : IRepository<Error>
    {
    }

    public class ErrorRepository : ErrorRepositoryBase<Error>, IErrorRepository
    {
        public ErrorRepository(IErrorDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}