using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.Repository
{
    public interface IInCaseOfRepository : IRepository<InCaseOf>
    {
    }

    public class InCaseOfRepository : RepositoryBase<InCaseOf>, IInCaseOfRepository
    {
        public InCaseOfRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
