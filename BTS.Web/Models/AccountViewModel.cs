using BTS.Data;
using BTS.Data.ApplicationModels;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class AccountViewModel
    {
        public class ManageUserViewModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage =
                "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage =
                "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public class EditUserViewModel
        {
            public EditUserViewModel()
            {
            }

            // Allow Initialization with an instance of ApplicationUser:
            public EditUserViewModel(ApplicationUser user)
            {
                this.UserName = user.UserName;
                this.FullName = user.FullName;
                this.Email = user.Email;
            }

            [Required]
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FullName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            public string Email { get; set; }
        }

        // Used to display a single role with a checkbox, within a list structure:
        public class SelectRoleEditorViewModel
        {
            public SelectRoleEditorViewModel()
            {
            }

            // Update this to accept an argument of type ApplicationRole:
            public SelectRoleEditorViewModel(ApplicationRole role)
            {
                this.RoleName = role.Name;

                // Assign the new Descrption property:
                this.Description = role.Description;
            }

            public bool Selected { get; set; }

            [Required]
            public string RoleName { get; set; }

            // Add the new Description property:
            public string Description { get; set; }
        }

        public class RoleViewModel
        {
            public string RoleName { get; set; }
            public string Description { get; set; }

            public RoleViewModel()
            {
            }

            public RoleViewModel(ApplicationRole role)
            {
                this.RoleName = role.Name;
                this.Description = role.Description;
            }
        }

        public class EditRoleViewModel
        {
            public string OriginalRoleName { get; set; }
            public string RoleName { get; set; }
            public string Description { get; set; }

            public EditRoleViewModel()
            {
            }

            public EditRoleViewModel(ApplicationRole role)
            {
                this.OriginalRoleName = role.Name;
                this.RoleName = role.Name;
                this.Description = role.Description;
            }
        }

        // Wrapper for SelectGroupEditorViewModel to select user group membership:
        public class SelectUserGroupsViewModel
        {
            public string UserName { get; set; }
            public string FullName { get; set; }
            public string LastName { get; set; }
            public List<SelectGroupEditorViewModel> Groups { get; set; }

            public SelectUserGroupsViewModel()
            {
                this.Groups = new List<SelectGroupEditorViewModel>();
            }

            public SelectUserGroupsViewModel(ApplicationUser user)
                : this()
            {
                this.UserName = user.UserName;
                this.FullName = user.FullName;

                var Db = new BTSDbContext();

                // Add all available groups to the public list:
                var allGroups = Db.ApplicationGroups;
                foreach (var role in allGroups)
                {
                    // An EditorViewModel will be used by Editor Template:
                    var rvm = new SelectGroupEditorViewModel(role);
                    this.Groups.Add(rvm);
                }

                // Set the Selected property to true where user is already a member:
                foreach (var group in user.Groups)
                {
                    var checkUserRole =
                        this.Groups.Find(r => r.GroupName == group.ApplicationGroup.Name);
                    checkUserRole.Selected = true;
                }
            }
        }

        // Used to display a single role group with a checkbox, within a list structure:
        public class SelectGroupEditorViewModel
        {
            public SelectGroupEditorViewModel()
            {
            }

            public SelectGroupEditorViewModel(ApplicationGroup group)
            {
                this.GroupName = group.Name;
                this.GroupId = group.Id;
            }

            public bool Selected { get; set; }

            [Required]
            public string GroupId { get; set; }

            public string GroupName { get; set; }
        }

        public class SelectGroupRolesViewModel
        {
            public SelectGroupRolesViewModel()
            {
                this.Roles = new List<SelectRoleEditorViewModel>();
            }

            // Enable initialization with an instance of ApplicationUser:
            public SelectGroupRolesViewModel(ApplicationGroup group)
                : this()
            {
                this.GroupId = group.Id;
                this.GroupName = group.Name;

                var Db = new BTSDbContext();

                // Add all available roles to the list of EditorViewModels:
                var allRoles = Db.Roles;
                foreach (var role in allRoles)
                {
                    // An EditorViewModel will be used by Editor Template:
                    var rvm = new SelectRoleEditorViewModel(role);
                    this.Roles.Add(rvm);
                }

                // Set the Selected property to true for those roles for
                // which the current user is a member:
                foreach (var groupRole in group.ApplicationRoleGroups)
                {
                    var checkGroupRole =
                        this.Roles.Find(r => r.RoleName == groupRole.ApplicationRole.Name);
                    checkGroupRole.Selected = true;
                }
            }

            public string GroupId { get; set; }
            public string GroupName { get; set; }
            public List<SelectRoleEditorViewModel> Roles { get; set; }
        }

        public class UserPermissionsViewModel
        {
            public UserPermissionsViewModel()
            {
                this.Roles = new List<string>();
            }

            // Enable initialization with an instance of ApplicationUser:
            public UserPermissionsViewModel(ApplicationUser user)
                : this()
            {
                this.UserName = user.UserName;
                this.FullName = user.FullName;
                foreach (var role in user.Roles)
                {
                    this.Roles.Add(role.RoleId);
                }
            }

            public string UserName { get; set; }
            public string FullName { get; set; }
            public string LastName { get; set; }
            public List<string> Roles { get; set; }
        }
    }
}