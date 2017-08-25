using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.Repository
{
    public interface IDistrictRepository: IRepository<District>
    {

    }
    public class DistrictRepository : RepositoryBase<District>, IDistrictRepository
    {
        public DistrictRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
