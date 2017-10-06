using BTS.Data.Infrastructure;
using BTS.Model.Models;

namespace BTS.Data.Repository
{
    public interface IDistrictRepository : IRepository<District>
    {
    }

    public class DistrictRepository : RepositoryBase<District>, IDistrictRepository
    {
        public DistrictRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}