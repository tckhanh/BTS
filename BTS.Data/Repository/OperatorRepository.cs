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
        bool IsUsed(string Id);
    }

    public class OperatorRepository : RepositoryBase<Operator>, IOperatorRepository
    {
        public OperatorRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool IsUsed(string Id)
        {
            var query0 = from item in DbContext.Applicants
                         where item.OperatorID == Id
                         select item.Id;
            if (query0.Count() > 0) return true;

            var query1 = from item in DbContext.Btss
                         where item.OperatorID == Id
                         select item.Id;
            if (query1.Count() > 0) return true;

            var query2 = from item in DbContext.Certificates
                         where item.OperatorID == Id
                         select item.Id;
            if (query2.Count() > 0) return true;

            var query3 = from item in DbContext.NoCertificates
                         where item.OperatorID == Id
                         select item.Id;
            if (query3.Count() > 0) return true;

            return false;
        }
    }
}