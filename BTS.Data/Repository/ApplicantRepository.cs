using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.Repository
{
    public interface IApplicantRepository: IRepository<Applicant>
    {
        bool IsUsed(string ID);
        IEnumerable<Operator> GetAllOperator();
    }
    public class ApplicantRepository : RepositoryBase<Applicant>, IApplicantRepository
    {
        public ApplicantRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Operator> GetAllOperator()
        {
            var query = from oper in DbContext.Operators
                        select oper;
            return query;
        }

        public bool IsUsed(string ID)
        {
            var query1 = from item in DbContext.Profiles
                         where item.ApplicantID == ID
                         select item.ID;
            if (query1.Count() > 0) return true;

            return false;
        }
    }
}
