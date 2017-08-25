using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.Repository
{
    public interface ISubBTSRepository: IRepository<SubBTS>
    {

    }
    public class SubBTSRepository : RepositoryBase<SubBTS>, ISubBTSRepository
    {
        public SubBTSRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
