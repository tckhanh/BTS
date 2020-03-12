using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.Repository
{
    public interface IWardRepository : IRepository<Ward>
    {
        bool IsUsed(string Id);
    }

    public class WardRepository : RepositoryBase<Ward>, IWardRepository
    {
        public WardRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool IsUsed(string Id)
        {
            //var query1 = from item in DbContext.Wards
            //             where item.Id == Id
            //             select item.Id;
            //if (query1.Count() > 0) return true;

            return false;
        }
    }
}