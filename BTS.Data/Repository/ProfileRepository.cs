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
        Profile findProfile(string applicationID, string profileNum, DateTime profileDate);
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
    }
}