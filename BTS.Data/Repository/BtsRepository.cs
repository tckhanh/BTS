using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.Repository
{
    public interface IBtsRepository : IRepository<Bts>
    {
    }

    public class BtsRepository : RepositoryBase<Bts>, IBtsRepository
    {
        public BtsRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}