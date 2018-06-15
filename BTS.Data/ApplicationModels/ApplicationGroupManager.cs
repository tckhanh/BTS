using BTS.Data;
using BTS.Model.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BTS.Data.ApplicationModels
{
    public class ApplicationGroupManager
    {
        //private ApplicationGroupStore _groupStore;
        //private BTSDbContext _db;
        //private ApplicationUserManager _userManager;
        //private RoleManager<IdentityRole> _roleManager;

        //public ApplicationGroupManager()
        //{
        //    //_db = HttpContext.Current.GetOwinContext().Get<BTSDbContext>();
        //    _db = new BTSDbContext();
        //    //_userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    //_roleManager = HttpContext.Current.GetOwinContext().Get<IdentityRole>();
        //    _groupStore = new ApplicationGroupStore(_db);

        //    _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
        //    _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
        //}

        private ApplicationGroupStore _groupStore;
        private BTSDbContext _db;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ApplicationGroupManager()
        {
            _db = HttpContext.Current.GetOwinContext().Get<BTSDbContext>();
            _userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            _roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            _groupStore = new ApplicationGroupStore(_db);
        }

        public IQueryable<ApplicationGroup> Groups
        {
            get
            {
                return _groupStore.Groups;
            }
        }

        public async Task<IdentityResult> CreateGroupAsync(ApplicationGroup group)
        {
            await _groupStore.CreateAsync(group);
            return IdentityResult.Success;
        }

        public IdentityResult CreateGroup(ApplicationGroup group)
        {
            _groupStore.Create(group);
            return IdentityResult.Success;
        }

        public IdentityResult SetGroupRoles(string groupId, params string[] roleNames)
        {
            // Clear all the roles associated with this group:
            var thisGroup = this.FindById(groupId);
            //thisGroup.ApplicationRoleGroups.Clear();
            _db.SaveChanges();

            // Add the new roles passed in:
            var newRoles = _roleManager.Roles.Where(r => roleNames.Any(n => n == r.Name));
            foreach (var role in newRoles)
            {
                //thisGroup.ApplicationRoleGroups.Add(new ApplicationRoleGroup { GroupId = groupId, RoleId = role.Id });
            }
            _db.SaveChanges();

            // Reset the roles for all affected users:
            foreach (var groupUser in thisGroup.ApplicationUserGroups)
            {
                this.RefreshUserGroupRoles(groupUser.UserId);
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> SetGroupRolesAsync(string groupId, params string[] roleNames)
        {
            // Clear all the roles associated with this group:
            var thisGroup = await this.FindByIdAsync(groupId);
            //thisGroup.ApplicationRoleGroups.Clear();
            await _db.SaveChangesAsync();

            // Add the new roles passed in:
            var newRoles = _roleManager.Roles.Where(r => roleNames.Any(n => n == r.Name));
            foreach (var role in newRoles)
            {
                //thisGroup.ApplicationRoleGroups.Add(new ApplicationRoleGroup { GroupId = groupId, RoleId = role.Id });
            }
            await _db.SaveChangesAsync();

            // Reset the roles for all affected users:
            foreach (var groupUser in thisGroup.ApplicationUserGroups)
            {
                await this.RefreshUserGroupRolesAsync(groupUser.UserId);
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> SetUserGroupsAsync(string userId, params string[] groupIds)
        {
            // Clear current group membership:
            var currentGroups = await this.GetUserGroupsAsync(userId);
            //foreach (var group in currentGroups)
            //{
            //    group.ApplicationUserGroups.
            //        .Remove(group.ApplicationUserGroups.FirstOrDefault(gr => gr.UserId == userId));
            //}
            //await _db.SaveChangesAsync();

            // Add the user to the new groups:
            foreach (string groupId in groupIds)
            {
                var newGroup = await this.FindByIdAsync(groupId);
                //newGroup.ApplicationUserGroups.Add(new ApplicationUserGroup { UserId = userId, GroupId = groupId });
            }
            await _db.SaveChangesAsync();

            await this.RefreshUserGroupRolesAsync(userId);
            return IdentityResult.Success;
        }

        public IdentityResult SetUserGroups(string userId, params string[] groupIds)
        {
            // Clear current group membership:
            var currentGroups = this.GetUserGroups(userId);
            //foreach (var group in currentGroups)
            //{
            //    group.ApplicationUserGroups
            //        .Remove(group.ApplicationUserGroups.FirstOrDefault(gr => gr.UserId == userId));
            //}
            //_db.SaveChanges();

            // Add the user to the new groups:
            foreach (string groupId in groupIds)
            {
                var newGroup = this.FindById(groupId);
                //newGroup.ApplicationUserGroups.Add(new ApplicationUserGroup { UserId = userId, GroupId = groupId });
            }
            _db.SaveChanges();

            this.RefreshUserGroupRoles(userId);
            return IdentityResult.Success;
        }

        public IdentityResult RefreshUserGroupRoles(string userId)
        {
            var user = _userManager.FindById(userId);
            if (user == null)
            {
                throw new ArgumentNullException("User");
            }
            // Remove user from previous roles:
            var oldUserRoles = _userManager.GetRoles(userId);
            if (oldUserRoles.Count > 0)
            {
                _userManager.RemoveFromRoles(userId, oldUserRoles.ToArray());
            }

            // Find teh roles this user is entitled to from group membership:
            var newGroupRoles = this.GetUserGroupRoles(userId);

            // Get the damn role names:
            var allRoles = _roleManager.Roles.ToList();
            var addTheseRoles = allRoles.Where(r => newGroupRoles.Any(gr => gr.RoleId == r.Id));
            var roleNames = addTheseRoles.Select(n => n.Name).ToArray();

            // Add the user to the proper roles
            _userManager.AddToRoles(userId, roleNames);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> RefreshUserGroupRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentNullException("User");
            }
            // Remove user from previous roles:
            var oldUserRoles = await _userManager.GetRolesAsync(userId);
            if (oldUserRoles.Count > 0)
            {
                await _userManager.RemoveFromRolesAsync(userId, oldUserRoles.ToArray());
            }

            // Find the roles this user is entitled to from group membership:
            var newGroupRoles = await this.GetUserGroupRolesAsync(userId);

            // Get the damn role names:
            var allRoles = await _roleManager.Roles.ToListAsync();
            var addTheseRoles = allRoles.Where(r => newGroupRoles.Any(gr => gr.RoleId == r.Id));
            var roleNames = addTheseRoles.Select(n => n.Name).ToArray();

            // Add the user to the proper roles
            await _userManager.AddToRolesAsync(userId, roleNames);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteGroupAsync(string groupId)
        {
            var group = await this.FindByIdAsync(groupId);
            if (group == null)
            {
                throw new ArgumentNullException("User");
            }

            var currentGroupMembers = (await this.GetGroupUsersAsync(groupId)).ToList();
            // remove the roles from the group:
            //group.ApplicationRoleGroups.Clear();

            // Remove all the users:
            //group.ApplicationUserGroups.Clear();

            // Remove the group itself:
            _db.ApplicationGroups.Remove(group);

            await _db.SaveChangesAsync();

            // Reset all the user roles:
            foreach (var user in currentGroupMembers)
            {
                await this.RefreshUserGroupRolesAsync(user.Id);
            }
            return IdentityResult.Success;
        }

        public IdentityResult DeleteGroup(string groupId)
        {
            var group = this.FindById(groupId);
            if (group == null)
            {
                throw new ArgumentNullException("User");
            }

            var currentGroupMembers = this.GetGroupUsers(groupId).ToList();
            // remove the roles from the group:
            //group.ApplicationRoleGroups.Clear();

            // Remove all the users:
            //group.ApplicationUserGroups.Clear();

            // Remove the group itself:
            _db.ApplicationGroups.Remove(group);

            _db.SaveChanges();

            // Reset all the user roles:
            foreach (var user in currentGroupMembers)
            {
                this.RefreshUserGroupRoles(user.Id);
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateGroupAsync(ApplicationGroup group)
        {
            await _groupStore.UpdateAsync(group);
            foreach (var groupUser in group.ApplicationUserGroups)
            {
                await this.RefreshUserGroupRolesAsync(groupUser.UserId);
            }
            return IdentityResult.Success;
        }

        public IdentityResult UpdateGroup(ApplicationGroup group)
        {
            _groupStore.Update(group);
            foreach (var groupUser in group.ApplicationUserGroups)
            {
                this.RefreshUserGroupRoles(groupUser.UserId);
            }
            return IdentityResult.Success;
        }

        public IdentityResult ClearUserGroups(string userId)
        {
            return this.SetUserGroups(userId, new string[] { });
        }

        public async Task<IdentityResult> ClearUserGroupsAsync(string userId)
        {
            return await this.SetUserGroupsAsync(userId, new string[] { });
        }

        public async Task<IEnumerable<ApplicationGroup>> GetUserGroupsAsync(string userId)
        {
            var result = new List<ApplicationGroup>();
            var userGroups = (from g in this.Groups
                              where g.ApplicationUserGroups.Any(u => u.UserId == userId)
                              select g).ToListAsync();
            return await userGroups;
        }

        public IEnumerable<ApplicationGroup> GetUserGroups(string userId)
        {
            var result = new List<ApplicationGroup>();
            var userGroups = (from g in this.Groups
                              where g.ApplicationUserGroups.Any(u => u.UserId == userId)
                              select g).ToList();
            return userGroups;
        }

        public async Task<IEnumerable<ApplicationRole>> GetGroupRolesAsync(string groupId)
        {
            var grp = await _db.ApplicationGroups.FirstOrDefaultAsync(g => g.Id == groupId);
            var roles = await _roleManager.Roles.ToListAsync();
            var groupRoles = (from r in roles
                              where grp.ApplicationRoleGroups.Any(ap => ap.RoleId == r.Id)
                              select r).ToList();
            return (IEnumerable<ApplicationRole>)groupRoles;
        }

        public IEnumerable<ApplicationRole> GetGroupRoles(string groupId)
        {
            var grp = _db.ApplicationGroups.FirstOrDefault(g => g.Id == groupId);
            var roles = _roleManager.Roles.ToList();
            var groupRoles = from r in roles
                             where grp.ApplicationRoleGroups.Any(ap => ap.RoleId == r.Id)
                             select r;
            return (IEnumerable<ApplicationRole>)groupRoles;
        }

        public IEnumerable<ApplicationUser> GetGroupUsers(string groupId)
        {
            var group = this.FindById(groupId);
            var users = new List<ApplicationUser>();
            foreach (var groupUser in group.ApplicationUserGroups)
            {
                var user = _db.Users.Find(groupUser.UserId);
                users.Add((ApplicationUser)user);
            }
            return users;
        }

        public async Task<IEnumerable<ApplicationUser>> GetGroupUsersAsync(string groupId)
        {
            var group = await this.FindByIdAsync(groupId);
            var users = new List<ApplicationUser>();
            foreach (var groupUser in group.ApplicationUserGroups)
            {
                var user = await _db.Users
                    .FirstOrDefaultAsync(u => u.Id == groupUser.UserId);
                users.Add((ApplicationUser)user);
            }
            return users;
        }

        public IEnumerable<ApplicationRoleGroup> GetUserGroupRoles(string userId)
        {
            var userGroups = this.GetUserGroups(userId);
            var userGroupRoles = new List<ApplicationRoleGroup>();
            foreach (var group in userGroups)
            {
                userGroupRoles.AddRange(group.ApplicationRoleGroups.ToArray());
            }
            return userGroupRoles;
        }

        public async Task<IEnumerable<ApplicationRoleGroup>> GetUserGroupRolesAsync(string userId)
        {
            var userGroups = await this.GetUserGroupsAsync(userId);
            var userGroupRoles = new List<ApplicationRoleGroup>();
            foreach (var group in userGroups)
            {
                userGroupRoles.AddRange(group.ApplicationRoleGroups.ToArray());
            }
            return userGroupRoles;
        }

        public async Task<ApplicationGroup> FindByIdAsync(string id)
        {
            return await _groupStore.FindByIdAsync(id);
        }

        public ApplicationGroup FindById(string id)
        {
            return _groupStore.FindById(id);
        }
    }
}