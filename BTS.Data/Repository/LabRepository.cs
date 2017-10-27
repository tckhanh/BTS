using BTS.Data.Infrastructure;
using BTS.Model.Models;

namespace BTS.Data.Repository
{
    public interface ILabRepository : IRepository<Lab>
    {
    }

    public class LabRepository : RepositoryBase<Lab>, ILabRepository
    {
        public LabRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}