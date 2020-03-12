using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.Repository
{
    public interface IDistrictRepository : IRepository<District>
    {
        bool IsUsed(string Id);
    }

    public class DistrictRepository : RepositoryBase<District>, IDistrictRepository
    {
        public DistrictRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool IsUsed(string Id)
        {
            var query1 = from item in DbContext.Wards
                         where item.DistrictId == Id
                         select item.Id;
            if (query1.Count() > 0) return true;

            return false;
        }
    }
}