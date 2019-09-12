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
        bool IsUsed(string Id);

        IEnumerable<Bts> getAll(DateTime startDate, DateTime endDate);
        IEnumerable<Operator> getAllOperator();

        IEnumerable<Profile> getAllProfile();

        IEnumerable<Profile> getAllProfileInProcess();

        IEnumerable<City> getAllCity();

        IEnumerable<InCaseOf> getAllInCaseOf();
    }

    public class BtsRepository : RepositoryBase<Bts>, IBtsRepository
    {
        public BtsRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Bts> getAll(DateTime startDate, DateTime endDate)
        {
            var query = from item in DbContext.Btss
                        join profile in DbContext.Profiles on item.ProfileID equals profile.Id
                        where profile.ApplyDate >= startDate && profile.ApplyDate <= endDate
                        select item;
            return query;
        }

        public IEnumerable<City> getAllCity()
        {
            var query = from item in DbContext.Cities
                        select item;
            return query;
        }

        public IEnumerable<InCaseOf> getAllInCaseOf()
        {
            var query = from item in DbContext.InCaseOfs
                        select item;
            return query;
        }

        public IEnumerable<Operator> getAllOperator()
        {
            var query = from item in DbContext.Operators
                        select item;
            return query;
        }

        public IEnumerable<Profile> getAllProfile()
        {
            var query = from item in DbContext.Profiles
                        select item;
            return query;
        }

        public IEnumerable<Profile> getAllProfileInProcess()
        {
            var query = from item in DbContext.Profiles
                        join bts in DbContext.Btss on item.Id equals bts.ProfileID
                        select item;
            return query.Distinct();
        }

        public bool IsUsed(string Id)
        {
            var query1 = from item in DbContext.Profiles
                         where item.ApplicantID == Id
                         select item.Id;
            if (query1.Count() > 0) return true;

            return false;
        }
    }
}