using BTS.Common;
using BTS.Data.ApplicationModels;
using BTS.Model.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace BTS.Data
{
    public static class InitDatabase
    {
        // In this method we will create default User roles and Admin user for login
        public static void GrantDefaultRolesForGroups(BTSDbContext context)
        {
            try
            {
                ApplicationRoleManager _roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));
                List<string> roleList;
                ApplicationGroup groupItem;

                // Add All Roles to SuperAdmin Groups

                groupItem = context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.SUPERADMIN_GROUP);
                IEnumerable<string> queryRoles = from g in context.Roles select g.Name;
                roleList = queryRoles.ToList();

                foreach (string roleItem in roleList)
                {
                    ApplicationRole dbRole = _roleManager.FindByName(roleItem);

                    if (dbRole != null && context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == groupItem.Id && x.RoleId == dbRole.Id) == null)
                    {
                        ApplicationRoleGroup appRoleGroup = new ApplicationRoleGroup()
                        {
                            GroupId = groupItem.Id,
                            RoleId = dbRole.Id
                        };
                        if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == appRoleGroup.GroupId && x.RoleId == appRoleGroup.RoleId) == null)
                        {
                            context.ApplicationRoleGroups.Add(appRoleGroup);
                            context.SaveChanges();
                        }
                    }
                }

                groupItem = context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.DIRECTOR_GROUP);
                roleList = new List<string>()
                {
                CommonConstants.System_CanView_Role,
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role,
                CommonConstants.Data_CanViewStatitics_Role,
                CommonConstants.Data_CanReset_Role,
                CommonConstants.Data_CanSign_Role,
                CommonConstants.Data_CanCancel_Role,
                CommonConstants.Data_CanExport_Role
                 };
                foreach (string roleItem in roleList)
                {
                    ApplicationRole dbRole = _roleManager.FindByName(roleItem);

                    if (dbRole != null && context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == groupItem.Id && x.RoleId == dbRole.Id) == null)
                    {
                        ApplicationRoleGroup appRoleGroup = new ApplicationRoleGroup()
                        {
                            GroupId = groupItem.Id,
                            RoleId = dbRole.Id
                        };
                        if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == appRoleGroup.GroupId && x.RoleId == appRoleGroup.RoleId) == null)
                        {
                            context.ApplicationRoleGroups.Add(appRoleGroup);
                            context.SaveChanges();
                        }
                    }
                }

                groupItem = context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.VERTIFICATIONHEADER_GROUP);
                roleList = new List<string>() {
                CommonConstants.System_CanView_Role,
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role,
                CommonConstants.Data_CanViewReport_Role,
                CommonConstants.Data_CanViewStatitics_Role,
                CommonConstants.Data_CanAdd_Role,
                CommonConstants.Data_CanImport_Role,
                CommonConstants.Data_CanExport_Role,
                CommonConstants.Data_CanEdit_Role,
                CommonConstants.Data_CanReset_Role,
                CommonConstants.Data_CanLock_Role,                
                CommonConstants.Data_CanCancel_Role,
                CommonConstants.Info_CanViewMap_Role,
                CommonConstants.Info_CanViewDetail_Role,
                CommonConstants.Info_CanViewChart_Role,
                CommonConstants.Info_CanViewReport_Role,
                CommonConstants.Info_CanViewStatitics_Role
            };
                foreach (string roleItem in roleList)
                {
                    ApplicationRole dbRole = _roleManager.FindByName(roleItem);
                    if (dbRole != null)
                    {
                        if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == groupItem.Id && x.RoleId == dbRole.Id) == null)
                        {
                            ApplicationRoleGroup appRoleGroup = new ApplicationRoleGroup()
                            {
                                GroupId = groupItem.Id,
                                RoleId = dbRole.Id
                            };
                            if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == appRoleGroup.GroupId && x.RoleId == appRoleGroup.RoleId) == null)
                            {
                                context.ApplicationRoleGroups.Add(appRoleGroup);
                                context.SaveChanges();
                            }
                        }
                    }
                }

                groupItem = context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.VERTIFICATION_GROUP);
                roleList = new List<string>() {
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role,
                CommonConstants.Data_CanViewReport_Role,
                CommonConstants.Data_CanAdd_Role,
                CommonConstants.Data_CanImport_Role,
                CommonConstants.Data_CanEdit_Role,
                CommonConstants.Data_CanReset_Role,
                CommonConstants.Data_CanLock_Role,
                CommonConstants.Data_CanCancel_Role,
            };
                foreach (string roleItem in roleList)
                {
                    ApplicationRole dbRole = _roleManager.FindByName(roleItem);
                    if (dbRole != null)
                    {
                        if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == groupItem.Id && x.RoleId == dbRole.Id) == null)
                        {
                            ApplicationRoleGroup appRoleGroup = new ApplicationRoleGroup()
                            {
                                GroupId = groupItem.Id,
                                RoleId = dbRole.Id
                            };
                            if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == appRoleGroup.GroupId && x.RoleId == appRoleGroup.RoleId) == null)
                            {
                                context.ApplicationRoleGroups.Add(appRoleGroup);
                                context.SaveChanges();
                            }
                        }
                    }
                }

                groupItem = context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.LAB_GROUP);
                roleList = new List<string>() {
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role,
                CommonConstants.Data_CanReset_Role,
            };
                foreach (string roleItem in roleList)
                {
                    ApplicationRole dbRole = _roleManager.FindByName(roleItem);
                    if (dbRole != null)
                    {
                        if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == groupItem.Id && x.RoleId == dbRole.Id) == null)
                        {
                            ApplicationRoleGroup appRoleGroup = new ApplicationRoleGroup()
                            {
                                GroupId = groupItem.Id,
                                RoleId = dbRole.Id
                            };
                            if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == appRoleGroup.GroupId && x.RoleId == appRoleGroup.RoleId) == null)
                            {
                                context.ApplicationRoleGroups.Add(appRoleGroup);
                                context.SaveChanges();
                            }
                        }
                    }
                }

                groupItem = context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.STTT_GROUP);
                roleList = new List<string>() {
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role,
                CommonConstants.Data_CanReset_Role,
            };
                foreach (string roleItem in roleList)
                {
                    ApplicationRole dbRole = _roleManager.FindByName(roleItem);
                    if (dbRole != null)
                    {
                        if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == groupItem.Id && x.RoleId == dbRole.Id) == null)
                        {
                            ApplicationRoleGroup appRoleGroup = new ApplicationRoleGroup()
                            {
                                GroupId = groupItem.Id,
                                RoleId = dbRole.Id
                            };
                            if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == appRoleGroup.GroupId && x.RoleId == appRoleGroup.RoleId) == null)
                            {
                                context.ApplicationRoleGroups.Add(appRoleGroup);
                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                string description = "";
                foreach (DbEntityValidationResult eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (DbValidationError ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }

                LogError(ex, description);
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            finally
            {
            }
        }

        public static void GrantDefaultRolesForRecoveryGroup(BTSDbContext context)
        {
            try
            {                
                ApplicationRoleManager _roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));
                List<string> roleList;                
                ApplicationGroup groupItem;

                // Add All Roles to Recovery Group

                groupItem = context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.RECOVERY_GROUP);
                IEnumerable<string> queryRoles = from g in context.Roles select g.Name;
                roleList = queryRoles.ToList();

                foreach (string roleItem in roleList)
                {
                    ApplicationRole dbRole = _roleManager.FindByName(roleItem);

                    if (dbRole != null && context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == groupItem.Id && x.RoleId == dbRole.Id) == null)
                    {
                        ApplicationRoleGroup appRoleGroup = new ApplicationRoleGroup()
                        {
                            GroupId = groupItem.Id,
                            RoleId = dbRole.Id
                        };
                        if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == appRoleGroup.GroupId && x.RoleId == appRoleGroup.RoleId) == null)
                        {
                            context.ApplicationRoleGroups.Add(appRoleGroup);
                            context.SaveChanges();
                        }
                    }
                }

            }
            catch (DbEntityValidationException ex)
            {
                string description = "";
                foreach (DbEntityValidationResult eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (DbValidationError ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }

                LogError(ex, description);
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            finally
            {
            }
        }

        public static void SetupRolesGroups(BTSDbContext context)
        {
            try
            {
                // Su dung RoleManager khong dung RoleService
                //var _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new BTSDbContext()));
                //var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new BTSDbContext()));

                ApplicationUserManager _userManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context));
                ApplicationRoleManager _roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));

                ApplicationGroup newGroup;
                ApplicationRole newRole;
                List<string[]> roleList;
                List<string[]> groupList;

                roleList = new List<string[]> {
                    new string[]{
                        CommonConstants.System_CanView_Role,
                        CommonConstants.System_CanView_Description },
                    new string[]{
                        CommonConstants.System_CanViewDetail_Role,
                        CommonConstants.System_CanViewDetail_Description },
                    new string[]{
                        CommonConstants.System_CanViewChart_Role,
                        CommonConstants.System_CanViewChart_Description },
                    new string[]{
                        CommonConstants.System_CanViewStatitics_Role,
                        CommonConstants.System_CanViewStatitics_Description },
                    new string[]{
                        CommonConstants.System_CanAdd_Role,
                        CommonConstants.System_CanAdd_Description },
                    new string[]{
                        CommonConstants.System_CanImport_Role,
                        CommonConstants.System_CanImport_Description },
                    new string[]{
                        CommonConstants.System_CanExport_Role,
                        CommonConstants.System_CanExport_Description },
                    new string[]{
                        CommonConstants.System_CanEdit_Role,
                        CommonConstants.System_CanEdit_Description },
                    new string[]{
                        CommonConstants.System_CanReset_Role,
                        CommonConstants.System_CanReset_Description },
                    new string[]{
                        CommonConstants.System_CanLock_Role,
                        CommonConstants.System_CanLock_Description },
                    new string[]{
                        CommonConstants.System_CanDelete_Role,
                        CommonConstants.System_CanDelete_Description }
                };

                foreach (string[] roleItem in roleList)
                {
                    if (!_roleManager.RoleExists(roleItem.ElementAt(0)))
                    {
                        // first we create Admin rool
                        newRole = new ApplicationRole(roleItem.ElementAt(0), roleItem.ElementAt(1));
                        _roleManager.Create(newRole);
                        context.SaveChanges();
                    }
                }

                roleList = new List<string[]> {
                    new string[]{
                        CommonConstants.Data_CanView_Role,
                        CommonConstants.Data_CanView_Description },
                    new string[]{
                        CommonConstants.Data_CanViewDetail_Role,
                        CommonConstants.Data_CanViewDetail_Description },
                    new string[]{
                        CommonConstants.Data_CanViewChart_Role,
                        CommonConstants.Data_CanViewChart_Description },
                    new string[]{
                        CommonConstants.Data_CanViewReport_Role,
                        CommonConstants.Data_CanViewReport_Description },
                    new string[]{
                        CommonConstants.Data_CanViewStatitics_Role,
                        CommonConstants.Data_CanViewStatitics_Description },
                    new string[]{
                        CommonConstants.Data_CanAdd_Role,
                        CommonConstants.Data_CanAdd_Description },
                    new string[]{
                        CommonConstants.Data_CanImport_Role,
                        CommonConstants.Data_CanImport_Description },
                    new string[]{
                        CommonConstants.Data_CanExport_Role,
                        CommonConstants.Data_CanExport_Description },
                    new string[]{
                        CommonConstants.Data_CanEdit_Role,
                        CommonConstants.Data_CanEdit_Description },
                    new string[]{
                        CommonConstants.Data_CanReset_Role,
                        CommonConstants.Data_CanReset_Description },
                    new string[]{
                        CommonConstants.Data_CanLock_Role,
                        CommonConstants.Data_CanLock_Description },
                    new string[]{
                        CommonConstants.Data_CanCancel_Role,
                        CommonConstants.Data_CanCancel_Description },
                    new string[]{
                        CommonConstants.Data_CanSign_Role,
                        CommonConstants.Data_CanSign_Description},

                    new string[]{
                        CommonConstants.Data_CanDelete_Role,
                        CommonConstants.Data_CanDelete_Description },

                    new string[]{
                        CommonConstants.Info_CanViewMap_Role,
                        CommonConstants.Info_CanView_Description },
                    new string[]{
                        CommonConstants.Info_CanViewDetail_Role,
                        CommonConstants.Info_CanViewDetail_Description },
                    new string[]{
                        CommonConstants.Info_CanViewChart_Role,
                        CommonConstants.Info_CanViewChart_Description },
                    new string[]{
                        CommonConstants.Info_CanViewReport_Role,
                        CommonConstants.Info_CanViewReport_Description },
                    new string[]{
                        CommonConstants.Info_CanViewStatitics_Role,
                        CommonConstants.Info_CanViewStatitics_Description },
                };

                foreach (string[] roleItem in roleList)
                {
                    if (!_roleManager.RoleExists(roleItem.ElementAt(0)))
                    {
                        // first we create Admin rool
                        newRole = new ApplicationRole(roleItem.ElementAt(0), roleItem.ElementAt(1));
                        _roleManager.Create(newRole);
                        context.SaveChanges();
                    }
                }

                groupList = new List<string[]> {
                    new string[]{
                        CommonConstants.SUPERADMIN_GROUP,
                        CommonConstants.SUPERADMIN_GROUP_NAME},
                    new string[]{
                        CommonConstants.DIRECTOR_GROUP,
                        CommonConstants.DIRECTOR_GROUP_NAME},
                    new string[]{
                        CommonConstants.VERTIFICATIONHEADER_GROUP,
                        CommonConstants.VERTIFICATIONHEADER_GROUP_NAME},
                    new string[]{
                        CommonConstants.VERTIFICATION_GROUP,
                        CommonConstants.VERTIFICATION_GROUP_NAME},
                    new string[]{
                        CommonConstants.LAB_GROUP,
                        CommonConstants.LAB_GROUP_NAME},
                    new string[]{
                        CommonConstants.STTT_GROUP,
                        CommonConstants.STTT_GROUP_NAME},
                };

                foreach (string[] groupItem in groupList)
                {
                    if (context.ApplicationGroups.FirstOrDefault(x=> x.Name == groupItem.ElementAt(0)) == null )
                    {
                        newGroup = new ApplicationGroup(groupItem.ElementAt(0), groupItem.ElementAt(1));
                        context.ApplicationGroups.Add(newGroup);
                        context.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                string description = "";
                foreach (DbEntityValidationResult eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (DbValidationError ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }

                LogError(ex, description);
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            finally
            {
            }
        }

        public static void SetupRecoveryRolesGroups(BTSDbContext context)
        {
            try
            {
                // Su dung RoleManager khong dung RoleService
                //var _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new BTSDbContext()));
                //var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new BTSDbContext()));

                ApplicationUserManager _userManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context));
                ApplicationRoleManager _roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));

                ApplicationGroup newGroup;
                ApplicationRole newRole;
                List<string[]> roleList;
                List<string[]> groupList;

                roleList = new List<string[]> {
                    new string[]{
                        CommonConstants.System_CanView_Role,
                        CommonConstants.System_CanView_Description },
                    new string[]{
                        CommonConstants.System_CanViewDetail_Role,
                        CommonConstants.System_CanViewDetail_Description },
                    new string[]{
                        CommonConstants.System_CanViewChart_Role,
                        CommonConstants.System_CanViewChart_Description },
                    new string[]{
                        CommonConstants.System_CanViewStatitics_Role,
                        CommonConstants.System_CanViewStatitics_Description },
                    new string[]{
                        CommonConstants.System_CanAdd_Role,
                        CommonConstants.System_CanAdd_Description },
                    new string[]{
                        CommonConstants.System_CanImport_Role,
                        CommonConstants.System_CanImport_Description },
                    new string[]{
                        CommonConstants.System_CanExport_Role,
                        CommonConstants.System_CanExport_Description },
                    new string[]{
                        CommonConstants.System_CanEdit_Role,
                        CommonConstants.System_CanEdit_Description },
                    new string[]{
                        CommonConstants.System_CanReset_Role,
                        CommonConstants.System_CanReset_Description },
                    new string[]{
                        CommonConstants.System_CanLock_Role,
                        CommonConstants.System_CanLock_Description },
                    new string[]{
                        CommonConstants.System_CanDelete_Role,
                        CommonConstants.System_CanDelete_Description }
                };

                foreach (string[] roleItem in roleList)
                {
                    if (!_roleManager.RoleExists(roleItem.ElementAt(0)))
                    {
                        // first we create Admin rool
                        newRole = new ApplicationRole(roleItem.ElementAt(0), roleItem.ElementAt(1));
                        _roleManager.Create(newRole);
                        context.SaveChanges();
                    }
                }

                roleList = new List<string[]> {
                    new string[]{
                        CommonConstants.Data_CanView_Role,
                        CommonConstants.Data_CanView_Description },
                    new string[]{
                        CommonConstants.Data_CanViewDetail_Role,
                        CommonConstants.Data_CanViewDetail_Description },
                    new string[]{
                        CommonConstants.Data_CanViewChart_Role,
                        CommonConstants.Data_CanViewChart_Description },
                    new string[]{
                        CommonConstants.Data_CanViewReport_Role,
                        CommonConstants.Data_CanViewReport_Description },
                    new string[]{
                        CommonConstants.Data_CanViewStatitics_Role,
                        CommonConstants.Data_CanViewStatitics_Description },
                    new string[]{
                        CommonConstants.Data_CanAdd_Role,
                        CommonConstants.Data_CanAdd_Description },
                    new string[]{
                        CommonConstants.Data_CanImport_Role,
                        CommonConstants.Data_CanImport_Description },
                    new string[]{
                        CommonConstants.Data_CanExport_Role,
                        CommonConstants.Data_CanExport_Description },
                    new string[]{
                        CommonConstants.Data_CanEdit_Role,
                        CommonConstants.Data_CanEdit_Description },
                    new string[]{
                        CommonConstants.Data_CanReset_Role,
                        CommonConstants.Data_CanReset_Description },
                    new string[]{
                        CommonConstants.Data_CanLock_Role,
                        CommonConstants.Data_CanLock_Description },
                    new string[]{
                        CommonConstants.Data_CanCancel_Role,
                        CommonConstants.Data_CanCancel_Description },
                    new string[]{
                        CommonConstants.Data_CanSign_Role,
                        CommonConstants.Data_CanSign_Description},
                    new string[]{
                        CommonConstants.Data_CanDelete_Role,
                        CommonConstants.Data_CanDelete_Description },

                    new string[]{
                        CommonConstants.Info_CanViewMap_Role,
                        CommonConstants.Info_CanView_Description },
                    new string[]{
                        CommonConstants.Info_CanViewDetail_Role,
                        CommonConstants.Info_CanViewDetail_Description },
                    new string[]{
                        CommonConstants.Info_CanViewChart_Role,
                        CommonConstants.Info_CanViewChart_Description },
                    new string[]{
                        CommonConstants.Info_CanViewReport_Role,
                        CommonConstants.Info_CanViewReport_Description },
                    new string[]{
                        CommonConstants.Info_CanViewStatitics_Role,
                        CommonConstants.Info_CanViewStatitics_Description },
                };

                foreach (string[] roleItem in roleList)
                {
                    if (!_roleManager.RoleExists(roleItem.ElementAt(0)))
                    {
                        // first we create Admin rool
                        newRole = new ApplicationRole(roleItem.ElementAt(0), roleItem.ElementAt(1));
                        _roleManager.Create(newRole);
                        context.SaveChanges();
                    }
                }

                groupList = new List<string[]> {
                    new string[]{
                        CommonConstants.RECOVERY_GROUP,
                        CommonConstants.RECOVERY_GROUP_NAME},
                    new string[]{
                        CommonConstants.SUPERADMIN_GROUP,
                        CommonConstants.SUPERADMIN_GROUP_NAME},
                    new string[]{
                        CommonConstants.DIRECTOR_GROUP,
                        CommonConstants.DIRECTOR_GROUP_NAME},
                    new string[]{
                        CommonConstants.VERTIFICATIONHEADER_GROUP,
                        CommonConstants.VERTIFICATIONHEADER_GROUP_NAME},
                    new string[]{
                        CommonConstants.VERTIFICATION_GROUP,
                        CommonConstants.VERTIFICATION_GROUP_NAME},
                    new string[]{
                        CommonConstants.LAB_GROUP,
                        CommonConstants.LAB_GROUP_NAME},
                    new string[]{
                        CommonConstants.STTT_GROUP,
                        CommonConstants.STTT_GROUP_NAME},
                };

                foreach (string[] groupItem in groupList)
                {
                    if (!_roleManager.RoleExists(groupItem.ElementAt(0)))
                    {
                        newGroup = new ApplicationGroup(groupItem.ElementAt(0), groupItem.ElementAt(1));
                        context.ApplicationGroups.Add(newGroup);
                        context.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                string description = "";
                foreach (DbEntityValidationResult eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (DbValidationError ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }

                LogError(ex, description);
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            finally
            {
            }
        }

        public static void CreateSuperUser(BTSDbContext context)
        {
            try
            {
                // Su dung RoleManager khong dung RoleService
                //var _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new BTSDbContext()));
                //var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new BTSDbContext()));

                ApplicationUserManager _userManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context));
                ApplicationRoleManager _roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));

                ApplicationGroup groupItem;

                // Add Default SuperAdmin User

                ApplicationUser newAppUser = new ApplicationUser()
                {
                    UserName = CommonConstants.SuperAdmin_Name,
                    FullName = CommonConstants.SuperAdmin_FullName,
                    Email = CommonConstants.SuperAdmin_Email,
                    EmailConfirmed = CommonConstants.SuperAdmin_EmailConfirmed,
                    ImagePath = CommonConstants.SuperAdmin_ImagePath,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    Locked = false
                };

                ApplicationUser userItem = _userManager.FindByName(CommonConstants.SuperAdmin_Name);

                if (userItem == null)
                {
                    IdentityResult chkUser = _userManager.Create(newAppUser, CommonConstants.SuperAdmin_Password);
                    chkUser = _userManager.SetLockoutEnabled(newAppUser.Id, false);
                }

                //Add default SuperAdmin User to Group SuperAdmin

                userItem = _userManager.FindByName(CommonConstants.SuperAdmin_Name);
                groupItem = context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.SUPERADMIN_GROUP);
                if (userItem != null && groupItem != null)
                {
                    ApplicationUserGroup appUserGroup = new ApplicationUserGroup()
                    {
                        GroupId = groupItem.Id,
                        UserId = userItem.Id
                    };
                    if (context.ApplicationUserGroups.FirstOrDefault(x => x.GroupId == appUserGroup.GroupId && x.UserId == appUserGroup.UserId) == null)
                    {
                        context.ApplicationUserGroups.Add(appUserGroup);
                        context.SaveChanges();
                    }

                    //Xóa Roles cũ Tạo Roles mới cho User
                    List<string> userRoles = _userManager.GetRoles(userItem.Id).ToList();
                    foreach (string role in userRoles)
                    {
                        _userManager.RemoveFromRole(userItem.Id, role);
                        context.SaveChanges();
                    }

                    userRoles = _userManager.GetRoles(userItem.Id).ToList();

                    Trace.WriteLine("Số Roles còn lại:" + userRoles.Count());

                    //add role to user
                    IQueryable<string> query = from g in context.Roles
                                               join rg in context.ApplicationRoleGroups
                                               on g.Id equals rg.RoleId
                                               join ug in context.ApplicationUserGroups
                                               on rg.GroupId equals ug.GroupId
                                               where ug.UserId == userItem.Id
                                               select g.Name;
                    string[] roles = query.Distinct().ToArray();
                    _userManager.AddToRoles(userItem.Id, roles);
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                string description = "";
                foreach (DbEntityValidationResult eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (DbValidationError ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }

                LogError(ex, description);
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            finally
            {
            }
        }

        public static void CreateRecoveryUser(BTSDbContext context)
        {
            try
            {
                // Su dung RoleManager khong dung RoleService
                //var _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new BTSDbContext()));
                //var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new BTSDbContext()));

                ApplicationUserManager _userManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context));
                ApplicationRoleManager _roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));

                ApplicationGroup groupItem;

                // Add Default SuperAdmin User

                ApplicationUser newAppUser = new ApplicationUser()
                {
                    UserName = CommonConstants.Recovery_Name,
                    FullName = CommonConstants.Recovery_FullName,
                    Email = CommonConstants.Recovery_Email,
                    EmailConfirmed = CommonConstants.Recovery_EmailConfirmed,
                    ImagePath = CommonConstants.Recovery_ImagePath,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "Recovery",
                    Locked = false
                };

                ApplicationUser userItem = _userManager.FindByName(CommonConstants.Recovery_Name);

                if (userItem == null)
                {
                    IdentityResult chkUser = _userManager.Create(newAppUser, CommonConstants.Recovery_Password);
                    chkUser = _userManager.SetLockoutEnabled(newAppUser.Id, false);
                }

                //Add default Recovery User to Group Recovery

                userItem = _userManager.FindByName(CommonConstants.Recovery_Name);
                groupItem = context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.RECOVERY_GROUP);
                if (userItem != null && groupItem != null)
                {
                    ApplicationUserGroup appUserGroup = new ApplicationUserGroup()
                    {
                        GroupId = groupItem.Id,
                        UserId = userItem.Id
                    };
                    if (context.ApplicationUserGroups.FirstOrDefault(x => x.GroupId == appUserGroup.GroupId && x.UserId == appUserGroup.UserId) == null)
                    {
                        context.ApplicationUserGroups.Add(appUserGroup);
                        context.SaveChanges();
                    }

                    //Xóa Roles cũ Tạo Roles mới cho User
                    List<string> userRoles = _userManager.GetRoles(userItem.Id).ToList();
                    foreach (string role in userRoles)
                    {
                        _userManager.RemoveFromRole(userItem.Id, role);
                        context.SaveChanges();
                    }

                    userRoles = _userManager.GetRoles(userItem.Id).ToList();

                    Trace.WriteLine("Số Roles còn lại:" + userRoles.Count());

                    //add role to user
                    IQueryable<string> query = from g in context.Roles
                                               join rg in context.ApplicationRoleGroups
                                               on g.Id equals rg.RoleId
                                               join ug in context.ApplicationUserGroups
                                               on rg.GroupId equals ug.GroupId
                                               where ug.UserId == userItem.Id
                                               select g.Name;
                    string[] roles = query.Distinct().ToArray();
                    _userManager.AddToRoles(userItem.Id, roles);
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                string description = "";
                foreach (DbEntityValidationResult eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (DbValidationError ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }

                LogError(ex, description);
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            finally
            {
            }
        }

        public static void LogError(Exception e, string description = "")
        {
            try
            {
                Error error = new Error
                {
                    Controller = e.Source,
                    CreatedDate = DateTime.Now,
                    Message = e.Message,
                    Description = description,
                    StackTrace = e.StackTrace
                };
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    foreach (DbValidationError ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Trace.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public static void CreateOperator(BTSDbContext context)
        {
            if (context.Operators.Count() == 0)
            {
                List<Operator> listOperator = new List<Operator>()
            {
                new Operator() { Id ="MOBIFONE", Name="Tổng Công Ty MobiFone"} ,
                new Operator() { Id ="VIETTEL", Name="Tập đoàn VIETTEL"} ,
                new Operator() { Id ="VINAPHONE", Name="Tập đoàn VNPT"}
            };
                context.Operators.AddRange(listOperator);
                context.SaveChanges();
            }
        }

        public static void CreateFooter(BTSDbContext context)
        {
            if (context.Footers.Count(x => x.Id == CommonConstants.DefaultFooterId) == 0)
            {
            }
        }

        public static void CreateSlide(BTSDbContext context)
        {
            if (context.Slides.Count() == 0)
            {
                List<Slide> listSlide = new List<Slide>()
                {
                    new Slide() {
                        Name ="Slide 1",
                        DisplayOrder =1,
                        Status =true,
                        Url ="#",
                        Image ="/Assets/client/images/bag.jpg",
                        Content =@"	<h2>FLAT 50% 0FF</h2>
                                <label>FOR ALL PURCHASE <b>VALUE</b></label>
                                <p>Lorem ipsum dolor sit amet, consectetur
                            adipisicing elit, sed do eiusmod tempor incididunt ut labore et </ p >
                        <span class=""on-get"">GET NOW</span>" },
                    new Slide() {
                        Name ="Slide 2",
                        DisplayOrder =2,
                        Status =true,
                        Url ="#",
                        Image ="/Assets/client/images/bag1.jpg",
                    Content=@"<h2>FLAT 50% 0FF</h2>
                                <label>FOR ALL PURCHASE <b>VALUE</b></label>

                                <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et </ p >

                                <span class=""on-get"">GET NOW</span>"},
                };
                context.Slides.AddRange(listSlide);
                context.SaveChanges();
            }
        }

        public static void CreatePage(BTSDbContext context)
        {
            if (context.Pages.Count() == 0)
            {
                try
                {
                    WebPage page = new WebPage()
                    {
                        Name = "Giới thiệu",
                        Alias = "gioi-thieu",
                        Content = @"Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium ",
                        Status = true
                    };
                    context.Pages.Add(page);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult eve in ex.EntityValidationErrors)
                    {
                        Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                        foreach (DbValidationError ve in eve.ValidationErrors)
                        {
                            Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        }
                    }
                }
            }
        }

        public static void CreateContactDetail(BTSDbContext context)
        {
            if (context.ContactDetails.Count() == 0)
            {
                try
                {
                    ContactDetail contactDetail = new ContactDetail()
                    {
                        Name = "Shop thời trang TEDU",
                        Address = "Ngõ 77 Xuân La",
                        Email = "tedu@gmail.com",
                        Lat = 21.0633645,
                        Lng = 105.8053274,
                        Phone = "095423233",
                        Website = "http://tedu.com.vn",
                        Other = "",
                        Status = true
                    };
                    context.ContactDetails.Add(contactDetail);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult eve in ex.EntityValidationErrors)
                    {
                        Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                        foreach (DbValidationError ve in eve.ValidationErrors)
                        {
                            Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        }
                    }
                }
            }
        }


        public static void CreateConfigTitle(BTSDbContext context)
        {
            if (!context.SystemConfigs.Any(x => x.Code == "HomeTitle"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Code = "HomeTitle",
                    ValueString = "Trang chủ Kiểm định BTS",
                });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "HomeMetaKeyword"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Code = "HomeMetaKeyword",
                    ValueString = "Trang chủ Kiểm định BTS",
                });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "HomeMetaDescription"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Code = "HomeMetaDescription",
                    ValueString = "Trang chủ Kiểm định BTS",
                });
            }
        }

    }
}
