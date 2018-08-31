using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTS.Data.Infrastructure;
using BTS.Model.Models;
using BTS.Data.ApplicationModels;

namespace BTS.Data.Repositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        void FindById(string Id);
    }

    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public void FindById(string Id)
        {
        }
    }
}