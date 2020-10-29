using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System.Linq;

namespace BTS.Data.Repository
{
    public interface IConfigRepository : IRepository<SystemConfig>
    {
        bool IsUsed(string Id);
    }

    public class ConfigRepository : RepositoryBase<SystemConfig>, IConfigRepository
    {
        public ConfigRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool IsUsed(string Id)
        {
            var query1 = from item in DbContext.Certificates
                         where item.LabID == Id
                         select item.Id;
            if (query1.Count() > 0) return true;

            var query2 = from item in DbContext.NoCertificates
                         where item.LabID == Id
                         select item.Id;
            if (query2.Count() > 0) return true;

            return false;
        }
    }
}