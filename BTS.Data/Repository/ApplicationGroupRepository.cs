using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTS.Data.Infrastructure;
using BTS.Model.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using BTS.Data.ApplicationModels;

namespace BTS.Data.Repositories
{
    public interface IApplicationGroupRepository : IRepository<ApplicationGroup>
    {
        IEnumerable<ApplicationGroup> GetGroupsByUserId(string userId);
        IEnumerable<ApplicationGroup> GetGroupsByRoleId(string roleId);
        ICollection<ApplicationUser> GetUsersByGroupId(string groupId);
        IEnumerable<ApplicationUserGroup> GetLogicUsersByRoleId(string roleId);
        ICollection<ApplicationUser> GetUsersByGroupIds(string[] groupIds);
        IEnumerable<ApplicationRole> GetRolesByGroupId(string groupId);
        IEnumerable<ApplicationRole> GetLogicRolesByUserId(string userId);
    }

    public class ApplicationGroupRepository : RepositoryBase<ApplicationGroup>, IApplicationGroupRepository
    {
        public ApplicationGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<ApplicationGroup> GetGroupsByUserId(string userId)
        {
            var query = from g in DbContext.ApplicationGroups
                        join ug in DbContext.ApplicationUserGroups
                        on g.Id equals ug.GroupId
                        where ug.UserId == userId
                        select g;
            return query;
        }

        public ICollection<ApplicationUser> GetUsersByGroupId(string groupId)
        {
            var query = from g in DbContext.ApplicationGroups
                        join ug in DbContext.ApplicationUserGroups
                        on g.Id equals ug.GroupId
                        join u in DbContext.Users
                        on ug.UserId equals u.Id
                        where ug.GroupId == groupId
                        select u;
            return query.ToList();
        }

        public IEnumerable<ApplicationUserGroup> GetLogicUsersByRoleId(string roleId)
        {
            var query = from ug in DbContext.ApplicationUserGroups
                        join rg in DbContext.ApplicationRoleGroups
                        on ug.GroupId equals rg.GroupId
                        where rg.RoleId == roleId
                        select ug;
            return query;
        }

        public ICollection<ApplicationUser> GetUsersByGroupIds(string[] groupIds)
        {
            var query = from g in DbContext.ApplicationGroups
                        join ug in DbContext.ApplicationUserGroups
                        on g.Id equals ug.GroupId
                        join u in DbContext.Users
                        on ug.UserId equals u.Id
                        where groupIds.Contains(ug.GroupId)
                        select u;
            return query.ToList();
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

        public IEnumerable<ApplicationGroup> GetGroupsByRoleId(string roleId)
        {
            var query = from g in DbContext.ApplicationGroups
                        join rg in DbContext.ApplicationRoleGroups
                        on g.Id equals rg.GroupId
                        where rg.RoleId == roleId
                        select g;
            return query;
        }
    }
}