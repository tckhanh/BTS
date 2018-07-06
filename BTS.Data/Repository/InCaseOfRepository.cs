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
        bool IsUsed(int ID);
    }

    public class InCaseOfRepository : RepositoryBase<InCaseOf>, IInCaseOfRepository
    {
        public InCaseOfRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool IsUsed(int ID)
        {
            var query1 = from item in DbContext.Btss
                         where item.InCaseOfID == ID
                         select item.ID;
            if (query1.Count() > 0) return true;

            var query2 = from item in DbContext.Certificates
                         where item.InCaseOfID == ID
                         select item.ID;
            if (query2.Count() > 0) return true;

            return false;
        }
    }
}