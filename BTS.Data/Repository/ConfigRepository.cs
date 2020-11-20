using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System.Linq;

namespace BTS.Data.Repository
{
    public interface IConfigRepository : IRepository<SystemConfig>
    {
        bool IsUsed(int Id);
    }

    public class ConfigRepository : RepositoryBase<SystemConfig>, IConfigRepository
    {
        public ConfigRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool IsUsed(int Id)
        {
            return false;
        }
    }
}