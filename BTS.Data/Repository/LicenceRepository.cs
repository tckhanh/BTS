using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System.Linq;

namespace BTS.Data.Repository
{
    public interface ILicenceRepository : IRepository<Licence>
    {
        bool IsUsed(string Id);
    }

    public class LicenceRepository : RepositoryBase<Licence>, ILicenceRepository
    {
        public LicenceRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
        public bool IsUsed(string Id)
        {
            Licence Item = GetSingleById(Id);
            if (Item != null) {
                return Item.enable;
            }
            else {
                return false;
            }           
        }
    }
}