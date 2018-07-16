using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.Repository
{
    public interface IProfileRepository : IRepository<Profile>
    {
        bool IsUsed(int Id);

        Profile findProfile(string applicationID, string profileNum, DateTime profileDate);

        IEnumerable<Profile> findProfilesBtsInProcess(string btsCode, string operatorID);

        IEnumerable<Profile> findProfilesBTSNoCertificate(string btsCode, string operatorID);
    }

    public class ProfileRepository : RepositoryBase<Profile>, IProfileRepository
    {
        public ProfileRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public Profile findProfile(string applicantID, string profileNum, DateTime profileDate)
        {
            return GetSingleByCondition(x => x.ApplicantID == applicantID && x.ProfileNum == profileNum && x.ProfileDate == profileDate);
        }

        public IEnumerable<Profile> findProfilesBtsInProcess(string btsCode, string operatorID)
        {
            btsCode = btsCode.Trim().ToUpper();
            var query = from pf in DbContext.Profiles
                        join bts in DbContext.Btss
                        on pf.Id equals bts.ProfileID
                        where bts.BtsCode.Trim().ToUpper() == btsCode && bts.OperatorID == operatorID
                        orderby pf.FeeReceiptDate descending
                        select pf;
            return query;
        }

        public IEnumerable<Profile> findProfilesBTSNoCertificate(string btsCode, string operatorID)
        {
            btsCode = btsCode.Trim().ToUpper();
            var query = from pf in DbContext.Profiles
                        join cer in DbContext.NoCertificates
                        on pf.Id equals cer.ProfileID
                        where cer.BtsCode.Trim().ToUpper() == btsCode && cer.OperatorID == operatorID
                        orderby pf.FeeReceiptDate descending
                        select pf;
            return query;
        }

        public bool IsUsed(int Id)
        {
            var query1 = from item in DbContext.Btss
                         where item.ProfileID == Id
                         select item.Id;
            if (query1.Count() > 0) return true;

            var query2 = from item in DbContext.Certificates
                         where item.ProfileID == Id
                         select item.Id;
            if (query2.Count() > 0) return true;

            var query3 = from item in DbContext.NoCertificates
                         where item.ProfileID == Id
                         select item.Id;
            if (query3.Count() > 0) return true;

            return false;
        }
    }
}