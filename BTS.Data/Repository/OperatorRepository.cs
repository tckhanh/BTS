using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.Repository
{
    public interface IOperatorRepository : IRepository<Operator>
    {

    }

    public class OperatorRepository : RepositoryBase<Operator>, IOperatorRepository
    {
        public OperatorRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
