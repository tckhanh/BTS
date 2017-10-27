using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.Repository
{
    public interface ISubBTSinCertRepository : IRepository<SubBtsInCert>
    {

    }
    public class SubBTSinCertRepository : RepositoryBase<SubBtsInCert>, ISubBTSinCertRepository
    {
        public SubBTSinCertRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
