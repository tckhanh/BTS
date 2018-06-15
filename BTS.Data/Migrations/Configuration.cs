namespace BTS.Data.Migrations
{
    using ApplicationModels;
    using Common;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model.Models;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BTS.Data.BTSDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BTSDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            CreateRolesandUsers(context);
            CreateOperator(context);
            CreateSlide(context);
            CreatePage(context);
            CreateContactDetail(context);
            CreateConfigTitle(context);
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
        private void CreateRolesandUsers(BTSDbContext context)
        {
            try
            {
                // Su dung RoleManager khong dung RoleService
                //var _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new BTSDbContext()));
                //var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new BTSDbContext()));

                ApplicationUserManager _userManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context));
                ApplicationRoleManager _roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));

                var newGroup = new ApplicationGroup();
                var newRole = new ApplicationRole();
                List<string> roleList = new List<string>();
                List<string[]> roleList2, groupList2;
                var listRoleGroup = new List<ApplicationRoleGroup>();
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
                        CommonConstants.System_CanLock_Role,
                        CommonConstants.System_CanLock_Description },
                    new string[]{
                        CommonConstants.System_CanDelete_Role,
                        CommonConstants.System_CanDelete_Description }
                };

                foreach (var roleItem in roleList2)
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
                        CommonConstants.Data_CanLock_Role,
                        CommonConstants.Data_CanLock_Description },
                    new string[]{
                        CommonConstants.Data_CanDelete_Role,
                        CommonConstants.Data_CanDelete_Description }
                };

                foreach (var roleItem in roleList2)
                {
                    if (!_roleManager.RoleExists(roleItem.ElementAt(0)))
                    {
                        // first we create Admin rool
                        newRole = new ApplicationRole(roleItem.ElementAt(0), roleItem.ElementAt(1));
                        _roleManager.Create(newRole);
                        context.SaveChanges();
                    }
                }

                //if (!_roleManager.RoleExists(CommonConstants.System_CanView_Role))
                //{
                //    // first we create Admin rool
                //    newRole = new ApplicationRole(CommonConstants.System_CanView_Role, CommonConstants.System_CanView_Role_Description);
                //    _roleManager.Create(newRole);
                //}

                //if (!_roleManager.RoleExists(CommonConstants.Data_CanView_Role))
                //{
                //    newRole = new ApplicationRole(CommonConstants.Data_CanView_Role, CommonConstants.Data_CanView_Role_Description);
                //    _roleManager.Create(newRole);
                //}

                //if (!_roleManager.RoleExists(CommonConstants.Data_CanViewDetail_Role))
                //{
                //    newRole = new ApplicationRole(CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanViewDetail_Role_Description);
                //    _roleManager.Create(newRole);
                //}

                //if (!_roleManager.RoleExists(CommonConstants.Data_CanViewChart_Role))
                //{
                //    newRole = new ApplicationRole(CommonConstants.Data_CanViewChart_Role, CommonConstants.Data_CanViewChart_Role_Description);
                //    _roleManager.Create(newRole);
                //}

                //if (!_roleManager.RoleExists(CommonConstants.Data_CanViewStatitics_Role))
                //{
                //    newRole = new ApplicationRole(CommonConstants.Data_CanViewStatitics_Role, CommonConstants.Data_CanViewStatitics_Role_Description);
                //    _roleManager.Create(newRole);
                //}

                //if (!_roleManager.RoleExists(CommonConstants.Data_CanAdd_Role))
                //{
                //    newRole = new ApplicationRole(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanAdd_Role_Description);
                //    _roleManager.Create(newRole);
                //}

                //if (!_roleManager.RoleExists(CommonConstants.Data_CanImport_Role))
                //{
                //    newRole = new ApplicationRole(CommonConstants.Data_CanImport_Role, CommonConstants.Data_CanImport_Role_Description);
                //    _roleManager.Create(newRole);
                //}

                //if (!_roleManager.RoleExists(CommonConstants.Data_CanExport_Role))
                //{
                //    newRole = new ApplicationRole(CommonConstants.Data_CanExport_Role, CommonConstants.Data_CanExport_Role_Description);
                //    _roleManager.Create(newRole);
                //}
                //if (!_roleManager.RoleExists(CommonConstants.Data_CanEdit_Role))
                //{
                //    newRole = new ApplicationRole(CommonConstants.Data_CanEdit_Role, CommonConstants.Data_CanEdit_Role_Description);
                //    _roleManager.Create(newRole);
                //}

                //if (!_roleManager.RoleExists(CommonConstants.Data_CanLock_Role))
                //{
                //    newRole = new ApplicationRole(CommonConstants.Data_CanLock_Role, CommonConstants.Data_CanDisable_Role_Description);
                //    _roleManager.Create(newRole);
                //}

                //if (!_roleManager.RoleExists(CommonConstants.Data_CanDelete_Role))
                //{
                //    newRole = new ApplicationRole(CommonConstants.Data_CanDelete_Role, CommonConstants.Data_CanDelete_Role_Description);
                //    _roleManager.Create(newRole);
                //}
                //context.SaveChanges();

                // Create Default Groups

                //groupList2 = new List<string[]> {
                //    new string[]{
                //        CommonConstants.SUPERADMIN_GROUP,
                //        CommonConstants.SUPERADMIN_GROUP_NAME},
                //    new string[]{
                //        CommonConstants.DIRECTOR_GROUP,
                //        CommonConstants.DIRECTOR_GROUP_NAME},
                //    new string[]{
                //        CommonConstants.VERTIFICATIONHEADER_GROUP,
                //        CommonConstants.VERTIFICATIONHEADER_GROUP_NAME},
                //    new string[]{
                //        CommonConstants.VERTIFICATION_GROUP,
                //        CommonConstants.VERTIFICATION_GROUP_NAME},
                //    new string[]{
                //        CommonConstants.LAB_GROUP,
                //        CommonConstants.LAB_GROUP_NAME},
                //    new string[]{
                //        CommonConstants.STTT_GROUP,
                //        CommonConstants.STTT_GROUP_NAME}
                //};

                //foreach (var item in groupList2)
                //{
                //    if (context.ApplicationGroups.FirstOrDefault(x => x.Name == item.ElementAt(0)) == null)
                //    {
                //        newGroup = new ApplicationGroup(item.ElementAt(0), item.ElementAt(1));
                //        context.ApplicationGroups.Add(newGroup);
                //        context.SaveChanges();
                //    }
                //}

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
                    CommonConstants.System_CanLock_Role,
                    CommonConstants.System_CanDelete_Role,

                    CommonConstants.Data_CanView_Role,
                    CommonConstants.Data_CanViewDetail_Role,
                    CommonConstants.Data_CanViewChart_Role,
                    CommonConstants.Data_CanViewStatitics_Role,
                    CommonConstants.Data_CanAdd_Role,
                    CommonConstants.Data_CanImport_Role,
                    CommonConstants.Data_CanExport_Role,
                    CommonConstants.Data_CanEdit_Role,
                    CommonConstants.Data_CanLock_Role,
                    CommonConstants.Data_CanDelete_Role
                };
                foreach (var roleItem in roleList)
                {
                    var dbRole = _roleManager.FindByName(roleItem);

                    if (dbRole != null && context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == groupItem.Id && x.RoleId == dbRole.Id) == null)
                    {
                        var appRoleGroup = new ApplicationRoleGroup()
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
                CommonConstants.Data_CanExport_Role
                 };
                foreach (var roleItem in roleList)
                {
                    var dbRole = _roleManager.FindByName(roleItem);

                    if (dbRole != null && context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == groupItem.Id && x.RoleId == dbRole.Id) == null)
                    {
                        var appRoleGroup = new ApplicationRoleGroup()
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
                CommonConstants.Data_CanViewStatitics_Role,
                CommonConstants.Data_CanAdd_Role,
                CommonConstants.Data_CanImport_Role,
                CommonConstants.Data_CanExport_Role,
                CommonConstants.Data_CanEdit_Role,
                CommonConstants.Data_CanLock_Role
            };
                foreach (var roleItem in roleList)
                {
                    var dbRole = _roleManager.FindByName(roleItem);
                    if (dbRole != null)
                    {
                        if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == groupItem.Id && x.RoleId == dbRole.Id) == null)
                        {
                            var appRoleGroup = new ApplicationRoleGroup()
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
                CommonConstants.Data_CanAdd_Role,
                CommonConstants.Data_CanImport_Role,
                CommonConstants.Data_CanEdit_Role,
                CommonConstants.Data_CanLock_Role
            };
                foreach (var roleItem in roleList)
                {
                    var dbRole = _roleManager.FindByName(roleItem);
                    if (dbRole != null)
                    {
                        if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == groupItem.Id && x.RoleId == dbRole.Id) == null)
                        {
                            var appRoleGroup = new ApplicationRoleGroup()
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
                CommonConstants.Data_CanViewChart_Role
            };
                foreach (var roleItem in roleList)
                {
                    var dbRole = _roleManager.FindByName(roleItem);
                    if (dbRole != null)
                    {
                        if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == groupItem.Id && x.RoleId == dbRole.Id) == null)
                        {
                            var appRoleGroup = new ApplicationRoleGroup()
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
                CommonConstants.Data_CanViewChart_Role
            };
                foreach (var roleItem in roleList)
                {
                    var dbRole = _roleManager.FindByName(roleItem);
                    if (dbRole != null)
                    {
                        if (context.ApplicationRoleGroups.FirstOrDefault(x => x.GroupId == groupItem.Id && x.RoleId == dbRole.Id) == null)
                        {
                            var appRoleGroup = new ApplicationRoleGroup()
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

                var newAppUser = new ApplicationUser()
                {
                    UserName = CommonConstants.SuperAdmin_Name,
                    FullName = CommonConstants.SuperAdmin_FullName,
                    Email = CommonConstants.SuperAdmin_Email,
                    EmailConfirmed = CommonConstants.SuperAdmin_EmailConfirmed,
                    Locked = false
                };

                var userItem = _userManager.FindByName(CommonConstants.SuperAdmin_Name);

                if (userItem == null)
                {
                    var chkUser = _userManager.Create(newAppUser, CommonConstants.SuperAdmin_Password);
                    chkUser = _userManager.SetLockoutEnabled(newAppUser.Id, false);
                }

                //Add default SuperAdmin User to Group SuperAdmin
                userItem = _userManager.FindByName(CommonConstants.SuperAdmin_Name);
                groupItem = context.ApplicationGroups.FirstOrDefault(x => x.Name == CommonConstants.SUPERADMIN_GROUP);
                if (userItem != null && groupItem != null)
                {
                    var appUserGroup = new ApplicationUserGroup()
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
                    var userRoles = _userManager.GetRoles(userItem.Id);
                    foreach (var role in userRoles)
                    {
                        _userManager.RemoveFromRole(userItem.Id, role);
                        context.SaveChanges();
                    }

                    //add role to user
                    var query = from g in context.Roles
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
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
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

        public void LogError(Exception e, string description = "")
        {
            try
            {
                Error error = new Error();
                error.Controller = e.Source;
                error.CreatedDate = DateTime.Now;
                error.Message = e.Message;
                error.Description = description;
                error.StackTrace = e.StackTrace;
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
                new Operator() { ID ="MOBIFONE", Name="Tổng Công Ty MobiFone"} ,
                new Operator() { ID ="VIETTEL", Name="Tập đoàn VIETTEL"} ,
                new Operator() { ID ="VINAPHONE", Name="Tập đoàn VNPT"}
            };
                context.Operators.AddRange(listOperator);
                context.SaveChanges();
            }
        }

        private void CreateFooter(BTSDbContext context)
        {
            if (context.Footers.Count(x => x.ID == CommonConstants.DefaultFooterId) == 0)
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