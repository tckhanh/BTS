﻿using BTS.Common;
using BTS.Data;
using BTS.Data.ApplicationModels;
using BTS.Data.Infrastructure;
using BTS.Data.Repositories;
using BTS.Model.Models;
using BTS.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BTS.Web.App_Start
{
    public partial class Startup
    {
        private IApplicationGroupService _appGroupService;
        private IApplicationRoleService _appRoleService;
        private IErrorService _errorService;
        private BTSDbContext context;

        public Startup(IErrorService errorService, IApplicationRoleService appRoleService,
                       IApplicationGroupService appGroupService)
        {
            _appGroupService = appGroupService;
            _appRoleService = appRoleService;
            _errorService = errorService;
        }

        private void CreateConfigTitle(BTSDbContext context)
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

        // In this method we will create default User roles and Admin user for login
        public static void CreateRolesandUsers(BTSDbContext context)
        {
            try
            {
                // Su dung RoleManager khong dung RoleService
                //var _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new BTSDbContext()));
                //var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new BTSDbContext()));

                ApplicationUserManager _userManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context));
                ApplicationRoleManager _roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));

                ApplicationGroup newGroup = new ApplicationGroup();
                ApplicationRole newRole = new ApplicationRole();
                List<string> roleList = new List<string>();
                List<string[]> roleList2;
                List<ApplicationRoleGroup> listRoleGroup = new List<ApplicationRoleGroup>();
                ApplicationGroup groupItem;

                roleList2 = new List<string[]> {
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

                foreach (string[] roleItem in roleList2)
                {
                    if (!_roleManager.RoleExists(roleItem.ElementAt(0)))
                    {
                        // first we create Admin rool
                        newRole = new ApplicationRole(roleItem.ElementAt(0), roleItem.ElementAt(1));
                        _roleManager.Create(newRole);
                        context.SaveChanges();
                    }
                }

                roleList2 = new List<string[]> {
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
                        CommonConstants.Data_CanDelete_Role,
                        CommonConstants.Data_CanDelete_Description },

                    new string[]{
                        CommonConstants.Info_CanView_Role,
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

                foreach (string[] roleItem in roleList2)
                {
                    if (!_roleManager.RoleExists(roleItem.ElementAt(0)))
                    {
                        // first we create Admin rool
                        newRole = new ApplicationRole(roleItem.ElementAt(0), roleItem.ElementAt(1));
                        _roleManager.Create(newRole);
                        context.SaveChanges();
                    }
                }


                if (context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.SUPERADMIN_GROUP) == null)
                {
                    newGroup = new ApplicationGroup(CommonConstants.SUPERADMIN_GROUP, CommonConstants.SUPERADMIN_GROUP_NAME);
                    context.ApplicationGroups.Add(newGroup);
                    context.SaveChanges();
                }

                if (context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.DIRECTOR_GROUP) == null)
                {
                    newGroup = new ApplicationGroup(CommonConstants.DIRECTOR_GROUP, CommonConstants.DIRECTOR_GROUP_NAME);
                    context.ApplicationGroups.Add(newGroup);
                    context.SaveChanges();
                }

                if (context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.VERTIFICATIONHEADER_GROUP) == null)
                {
                    newGroup = new ApplicationGroup(CommonConstants.VERTIFICATIONHEADER_GROUP, CommonConstants.VERTIFICATIONHEADER_GROUP_NAME);
                    context.ApplicationGroups.Add(newGroup);
                    context.SaveChanges();
                }

                if (context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.VERTIFICATION_GROUP) == null)
                {
                    newGroup = new ApplicationGroup(CommonConstants.VERTIFICATION_GROUP, CommonConstants.VERTIFICATION_GROUP_NAME);
                    context.ApplicationGroups.Add(newGroup);
                    context.SaveChanges();
                }

                if (context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.LAB_GROUP) == null)
                {
                    newGroup = new ApplicationGroup(CommonConstants.LAB_GROUP, CommonConstants.LAB_GROUP_NAME);
                    context.ApplicationGroups.Add(newGroup);
                    context.SaveChanges();
                }

                if (context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.STTT_GROUP) == null)
                {
                    newGroup = new ApplicationGroup(CommonConstants.STTT_GROUP, CommonConstants.STTT_GROUP_NAME);
                    context.ApplicationGroups.Add(newGroup);
                    context.SaveChanges();
                }

                // Add Default Roles to Groups

                groupItem = context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.SUPERADMIN_GROUP);
                roleList = new List<string>(){
                    CommonConstants.System_CanView_Role,
                    CommonConstants.System_CanViewDetail_Role,
                    CommonConstants.System_CanViewChart_Role,
                    CommonConstants.System_CanViewStatitics_Role,
                    CommonConstants.System_CanAdd_Role,
                    CommonConstants.System_CanImport_Role,
                    CommonConstants.System_CanExport_Role,
                    CommonConstants.System_CanEdit_Role,
                    CommonConstants.System_CanReset_Role,
                    CommonConstants.System_CanLock_Role,
                    CommonConstants.System_CanDelete_Role,

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
                    CommonConstants.Data_CanDelete_Role,

                    CommonConstants.Info_CanView_Role,
                    CommonConstants.Info_CanViewDetail_Role,
                    CommonConstants.Info_CanViewChart_Role,
                    CommonConstants.Info_CanViewReport_Role,
                    CommonConstants.Info_CanViewStatitics_Role,
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

                groupItem = context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.DIRECTOR_GROUP);
                roleList = new List<string>()
                {
                CommonConstants.System_CanView_Role,
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role,
                CommonConstants.Data_CanViewStatitics_Role,
                CommonConstants.Data_CanReset_Role,
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
                CommonConstants.Info_CanView_Role,
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
                CommonConstants.Data_CanLock_Role
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
                    IList<string> userRoles = _userManager.GetRoles(userItem.Id);
                    foreach (string role in userRoles)
                    {
                        _userManager.RemoveFromRole(userItem.Id, role);
                        context.SaveChanges();
                    }

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

        private void CreateOperator(BTSDbContext context)
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

        private void CreateFooter(BTSDbContext context)
        {
            if (context.Footers.Count(x => x.Id == CommonConstants.DefaultFooterId) == 0)
            {
                string content = "";
            }
        }

        private void CreateSlide(BTSDbContext context)
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

        private void CreatePage(BTSDbContext context)
        {
            if (context.Pages.Count() == 0)
            {
                try
                {
                    var page = new WebPage()
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
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        }
                    }
                }
            }
        }

        private void CreateContactDetail(BTSDbContext context)
        {
            if (context.ContactDetails.Count() == 0)
            {
                try
                {
                    var contactDetail = new ContactDetail()
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
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        }
                    }
                }
            }
        }
    }
}