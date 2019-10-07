using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTS.Common.Exceptions;
using BTS.Data.Infrastructure;
using BTS.Data.Repositories;
using BTS.Model.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using BTS.Data.ApplicationModels;

namespace BTS.Service
{
    public interface IApplicationGroupService
    {
        ApplicationGroup GetDetail(string id);

        IEnumerable<ApplicationGroup> GetAll(int page, int pageSize, out int totalRow, string filter);

        IEnumerable<ApplicationGroup> GetAll();

        ApplicationGroup GetByID(string id);

        ApplicationGroup Add(ApplicationGroup appGroup);

        void Update(ApplicationGroup appGroup);

        ApplicationGroup Delete(string id);

        bool AddUserToGroups(IEnumerable<ApplicationGroup> groups, string userId);

        bool AddUserGroups(IEnumerable<ApplicationUserGroup> userGroups);

        bool AddRolesToGroup(IEnumerable<ApplicationRole> roles, string groupId);

        bool AddRoleGroups(IEnumerable<ApplicationRoleGroup> roleGroups);

        bool DeleteRolesFromGroup(string groupId);

        bool DeleteRoleFromGroups(string roleId);

        bool DeleteUserFromGroups(string userId);

        bool DeleteUsersFromGroup(string groupId);

        IEnumerable<ApplicationGroup> GetGroupsByUserId(string userId);

        IEnumerable<ApplicationGroup> GetGroupsByRoleId(string roleId);

        ICollection<ApplicationUser> GetUsersByGroupId(string groupId);

        IEnumerable<ApplicationUserGroup> GetLogicUsersByRoleId(string roleId);

        ICollection<ApplicationUser> GetUsersByGroupIds(string[] groupId);

        //Get list role by group id
        IEnumerable<ApplicationRole> GetRolesByGroupId(string groupId);

        IEnumerable<ApplicationRole> GetLogicRolesByUserId(string userId);

        void Save();
    }

    public class ApplicationGroupService : IApplicationGroupService
    {
        private IApplicationGroupRepository _appGroupRepository;
        private IApplicationUserGroupRepository _appUserGroupRepository;
        private IApplicationRoleGroupRepository _appRoleGroupRepository;
        private IUnitOfWork _unitOfWork;

        public ApplicationGroupService(IUnitOfWork unitOfWork,
            IApplicationUserGroupRepository appUserGroupRepository,
            IApplicationRoleGroupRepository appRoleGroupRepository,
            IApplicationGroupRepository appGroupRepository)
        {
            _appGroupRepository = appGroupRepository;
            _appUserGroupRepository = appUserGroupRepository;
            _appRoleGroupRepository = appRoleGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public ApplicationGroup Add(ApplicationGroup appGroup)
        {
            if (_appGroupRepository.CheckContains(x => x.Name == appGroup.Name))
                throw new NameDuplicatedException("Tên không được trùng");
            return _appGroupRepository.Add(appGroup);
        }

        public ApplicationGroup Delete(string id)
        {
            var appGroup = this._appGroupRepository.GetSingleById(id);
            return _appGroupRepository.Delete(appGroup);
        }

        public IEnumerable<ApplicationGroup> GetAll()
        {
            return _appGroupRepository.GetAll();
        }

        public IEnumerable<ApplicationGroup> GetAll(int page, int pageSize, out int totalRow, string filter = null)
        {
            var query = _appGroupRepository.GetAll();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));

            totalRow = query.Count();
            return query.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);
        }

        public ApplicationGroup GetDetail(string id)
        {
            return _appGroupRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ApplicationGroup appGroup)
        {
            if (_appGroupRepository.CheckContains(x => x.Name == appGroup.Name && x.Id != appGroup.Id))
                throw new NameDuplicatedException("Tên không được trùng");
            _appGroupRepository.Update(appGroup);
        }

        public bool AddUserGroups(IEnumerable<ApplicationUserGroup> userGroups)
        {
            foreach (var userGroup in userGroups)
            {
                _appUserGroupRepository.Add(userGroup);
            }
            return true;
        }

        public bool DeleteUserFromGroups(string userId)
        {
            _appUserGroupRepository.DeleteMulti(x => x.UserId == userId);
            return true;
        }

        public bool DeleteUsersFromGroup(string groupId)
        {
            _appUserGroupRepository.DeleteMulti(x => x.GroupId == groupId);
            return true;
        }

        public bool AddUserToGroups(IEnumerable<ApplicationGroup> groups, string userId)
        {
            foreach (var group in groups)
            {
                _appUserGroupRepository.Add(new ApplicationUserGroup()
                {
                    GroupId = group.Id,
                    UserId = userId,
                });
            }
            return true;
        }

        public bool AddRoleGroups(IEnumerable<ApplicationRoleGroup> roleGroups)
        {
            foreach (var roleGroup in roleGroups)
            {
                _appRoleGroupRepository.Add(roleGroup);
            }
            return true;
        }

        public bool AddRolesToGroup(IEnumerable<ApplicationRole> roles, string groupId)
        {
            foreach (var role in roles)
            {
                _appRoleGroupRepository.Add(new ApplicationRoleGroup()
                {
                    RoleId = role.Id,
                    GroupId = groupId
                });
            }
            return true;
        }

        public bool DeleteRolesFromGroup(string groupId)
        {
            _appRoleGroupRepository.DeleteMulti(x => x.GroupId == groupId);
            return true;
        }

        public bool DeleteRoleFromGroups(string roleId)
        {
            _appRoleGroupRepository.DeleteMulti(x => x.RoleId == roleId);
            return true;
        }
        public IEnumerable<ApplicationGroup> GetGroupsByUserId(string userId)
        {
            return _appGroupRepository.GetGroupsByUserId(userId);
        }

        public ICollection<ApplicationUser> GetUsersByGroupId(string groupId)
        {
            return _appGroupRepository.GetUsersByGroupId(groupId);
        }

        public IEnumerable<ApplicationUserGroup> GetLogicUsersByRoleId(string roleId)
        {
            return _appGroupRepository.GetLogicUsersByRoleId(roleId);
        }

        public ICollection<ApplicationUser> GetUsersByGroupIds(string[] groupIds)
        {
            return _appGroupRepository.GetUsersByGroupIds(groupIds);
        }

        public ApplicationGroup GetByID(string id)
        {
            return _appGroupRepository.GetSingleById(id);
        }

        public IEnumerable<ApplicationRole> GetRolesByGroupId(string groupId)
        {
            return _appGroupRepository.GetRolesByGroupId(groupId);
        }

        public IEnumerable<ApplicationRole> GetLogicRolesByUserId(string userId)
        {
            return _appGroupRepository.GetLogicRolesByUserId(userId);
        }

        public IEnumerable<ApplicationGroup> GetGroupsByRoleId(string roleId)
        {
            return _appGroupRepository.GetGroupsByRoleId(roleId);
        }
    }
}