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
    public interface IApplicationRoleRepository : IRepository<ApplicationRole>
    {
        IEnumerable<ApplicationRole> GetRolesByGroupId(string groupId);

        IEnumerable<ApplicationRole> GetLogicRolesByUserId(string userId);
    }

    public class ApplicationRoleRepository : RepositoryBase<ApplicationRole>, IApplicationRoleRepository
    {
        public ApplicationRoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<ApplicationRole> GetRolesByGroupId(string groupId)
        {
            var query = from g in DbContext.Roles
                        join ug in DbContext.ApplicationRoleGroups
                        on g.Id equals ug.RoleId
                        where ug.GroupId == groupId
                        select g;
            return query.Distinct().OrderBy(x => x.Name);
        }

        public IEnumerable<ApplicationRole> GetLogicRolesByUserId(string userId)
        {
            var query = from g in DbContext.Roles
                        join rg in DbContext.ApplicationRoleGroups
                        on g.Id equals rg.RoleId
                        join ug in DbContext.ApplicationUserGroups
                        on rg.GroupId equals ug.GroupId
                        where ug.UserId == userId
                        select g;
            return query.Distinct().OrderBy(x => x.Name);
        }
    }
}