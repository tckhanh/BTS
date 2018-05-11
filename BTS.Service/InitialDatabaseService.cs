using BTS.Common;
using BTS.Data;
using BTS.Model.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Service
{
    public class InitialDatabaseService
    {
        private IApplicationGroupService _appGroupService;
        private IApplicationRoleService _appRoleService;
        private IErrorService _errorService;
        private BTSDbContext context;

        public InitialDatabaseService(IErrorService errorService, IApplicationRoleService appRoleService,
                       IApplicationGroupService appGroupService)
        {
            _appGroupService = appGroupService;
            _appRoleService = appRoleService;
            _errorService = errorService;
        }

        private void CreateConfigTitle()
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
                var _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var newGroup = new ApplicationGroup();
                var newRole = new ApplicationRole();
                var roleList = new string[0];
                var listRoleGroup = new List<ApplicationRoleGroup>();
                ApplicationGroup groupItem;

                // Create Default Roles
                if (!_roleManager.RoleExists(CommonConstants.System_Admin_Role))
                {
                    // first we create Admin rool
                    newRole = new ApplicationRole(CommonConstants.System_Admin_Role, CommonConstants.System_Admin_Role_Description);
                    _roleManager.Create(newRole);
                }

                if (!_roleManager.RoleExists(CommonConstants.Data_CanView_Role))
                {
                    newRole = new ApplicationRole(CommonConstants.Data_CanView_Role, CommonConstants.Data_CanView_Role_Description);
                    _roleManager.Create(newRole);
                }

                if (!_roleManager.RoleExists(CommonConstants.Data_CanViewDetail_Role))
                {
                    newRole = new ApplicationRole(CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanViewDetail_Role_Description);
                    _roleManager.Create(newRole);
                }

                if (!_roleManager.RoleExists(CommonConstants.Data_CanViewChart_Role))
                {
                    newRole = new ApplicationRole(CommonConstants.Data_CanViewChart_Role, CommonConstants.Data_CanViewChart_Role_Description);
                    _roleManager.Create(newRole);
                }

                if (!_roleManager.RoleExists(CommonConstants.Data_CanViewStatitics_Role))
                {
                    newRole = new ApplicationRole(CommonConstants.Data_CanViewStatitics_Role, CommonConstants.Data_CanViewStatitics_Role_Description);
                    _roleManager.Create(newRole);
                }

                if (!_roleManager.RoleExists(CommonConstants.Data_CanAdd_Role))
                {
                    newRole = new ApplicationRole(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanAdd_Role_Description);
                    _roleManager.Create(newRole);
                }

                if (!_roleManager.RoleExists(CommonConstants.Data_CanImport_Role))
                {
                    newRole = new ApplicationRole(CommonConstants.Data_CanImport_Role, CommonConstants.Data_CanImport_Role_Description);
                    _roleManager.Create(newRole);
                }

                if (!_roleManager.RoleExists(CommonConstants.Data_CanExport_Role))
                {
                    newRole = new ApplicationRole(CommonConstants.Data_CanExport_Role, CommonConstants.Data_CanExport_Role_Description);
                    _roleManager.Create(newRole);
                }
                if (!_roleManager.RoleExists(CommonConstants.Data_CanEdit_Role))
                {
                    newRole = new ApplicationRole(CommonConstants.Data_CanEdit_Role, CommonConstants.Data_CanEdit_Role_Description);
                    _roleManager.Create(newRole);
                }

                if (!_roleManager.RoleExists(CommonConstants.Data_CanDisable_Role))
                {
                    newRole = new ApplicationRole(CommonConstants.Data_CanDisable_Role, CommonConstants.Data_CanDisable_Role_Description);
                    _roleManager.Create(newRole);
                }

                if (!_roleManager.RoleExists(CommonConstants.Data_CanDelete_Role))
                {
                    newRole = new ApplicationRole(CommonConstants.Data_CanDelete_Role, CommonConstants.Data_CanDelete_Role_Description);
                    _roleManager.Create(newRole);
                }

                // Create Default Groups

                if (_appGroupService.GetDetail(CommonConstants.SUPERADMIN_GROUP) == null)
                {
                    newGroup = new ApplicationGroup(CommonConstants.SUPERADMIN_GROUP, CommonConstants.SUPERADMIN_GROUP_NAME);
                    _appGroupService.Add(newGroup);
                    _appGroupService.Save();
                }

                if (_appGroupService.GetDetail(CommonConstants.DIRECTOR_GROUP) == null)
                {
                    newGroup = new ApplicationGroup(CommonConstants.DIRECTOR_GROUP, CommonConstants.DIRECTOR_GROUP_NAME);
                    _appGroupService.Add(newGroup);
                    _appGroupService.Save();
                }

                if (_appGroupService.GetDetail(CommonConstants.VERTIFICATIONHEADER_GROUP) == null)
                {
                    newGroup = new ApplicationGroup(CommonConstants.VERTIFICATIONHEADER_GROUP, CommonConstants.VERTIFICATIONHEADER_GROUP_NAME);
                    _appGroupService.Add(newGroup);
                    _appGroupService.Save();
                }

                if (_appGroupService.GetDetail(CommonConstants.VERTIFICATION_GROUP) == null)
                {
                    newGroup = new ApplicationGroup(CommonConstants.VERTIFICATION_GROUP, CommonConstants.VERTIFICATION_GROUP_NAME);
                    _appGroupService.Add(newGroup);
                    _appGroupService.Save();
                }

                if (_appGroupService.GetDetail(CommonConstants.LAB_GROUP) == null)
                {
                    newGroup = new ApplicationGroup(CommonConstants.LAB_GROUP, CommonConstants.LAB_GROUP_NAME);
                    _appGroupService.Add(newGroup);
                    _appGroupService.Save();
                }

                if (_appGroupService.GetDetail(CommonConstants.STTT_GROUP) == null)
                {
                    newGroup = new ApplicationGroup(CommonConstants.STTT_GROUP, CommonConstants.LAB_GROUP_NAME);
                    _appGroupService.Add(newGroup);
                    _appGroupService.Save();
                }

                // Add Default SuperAdmin User

                var newAppUser = new ApplicationUser
                {
                    UserName = CommonConstants.SuperAdmin_Name,
                    FullName = CommonConstants.SuperAdmin_FullName,
                    Email = CommonConstants.SuperAdmin_Email,
                    EmailConfirmed = CommonConstants.SuperAdmin_EmailConfirmed,
                };

                if (_userManager.Users.Count(x => x.UserName == CommonConstants.SuperAdmin_Name) == 0)
                {
                    var chkUser = _userManager.Create(newAppUser, CommonConstants.SuperAdmin_Password);
                    chkUser = _userManager.SetLockoutEnabled(newAppUser.Id, false);
                }

                //Add default SuperAdmin User to Group SuperAdmin

                var userItem = _userManager.Users.FirstOrDefault(x => x.UserName == CommonConstants.SuperAdmin_Name);
                if (userItem != null)
                {
                    groupItem = _appGroupService.GetByID(CommonConstants.SUPERADMIN_GROUP);

                    var listAppUserGroup = new List<ApplicationUserGroup>();
                    listAppUserGroup.Add(new ApplicationUserGroup()
                    {
                        GroupId = groupItem.ID,
                        UserId = userItem.Id
                    });

                    //add role to user
                    var listRole = _appRoleService.GetListRoleByGroupId(groupItem.ID);
                    foreach (var roleItem in listRole)
                    {
                        _userManager.RemoveFromRoleAsync(userItem.Id, roleItem.Name);
                        _userManager.AddToRoleAsync(userItem.Id, roleItem.Name);
                    }

                    _appGroupService.AddUserToGroups(listAppUserGroup, userItem.Id);
                    _appGroupService.Save();
                }

                // Add Default Roles to Groups
                groupItem = _appGroupService.GetByID(CommonConstants.DIRECTOR_GROUP);
                roleList = new string[] {
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role,
                CommonConstants.Data_CanViewStatitics_Role,
                CommonConstants.Data_CanExport_Role
            };
                listRoleGroup = new List<ApplicationRoleGroup>();
                foreach (var roleItem in roleList)
                {
                    listRoleGroup.Add(new ApplicationRoleGroup()
                    {
                        GroupId = groupItem.ID,
                        RoleId = roleItem
                    });
                }
                _appRoleService.AddRolesToGroup(listRoleGroup, groupItem.ID);
                _appRoleService.Save();

                groupItem = _appGroupService.GetByID(CommonConstants.VERTIFICATIONHEADER_GROUP);
                roleList = new string[] {
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role,
                CommonConstants.Data_CanViewStatitics_Role,
                CommonConstants.Data_CanAdd_Role,
                CommonConstants.Data_CanImport_Role,
                CommonConstants.Data_CanExport_Role,
                CommonConstants.Data_CanEdit_Role,
                CommonConstants.Data_CanDisable_Role
            };
                listRoleGroup = new List<ApplicationRoleGroup>();
                foreach (var roleItem in roleList)
                {
                    listRoleGroup.Add(new ApplicationRoleGroup()
                    {
                        GroupId = groupItem.ID,
                        RoleId = roleItem
                    });
                }
                _appRoleService.AddRolesToGroup(listRoleGroup, groupItem.ID);
                _appRoleService.Save();

                groupItem = _appGroupService.GetByID(CommonConstants.VERTIFICATION_GROUP);
                roleList = new string[] {
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role,
                CommonConstants.Data_CanAdd_Role,
                CommonConstants.Data_CanImport_Role,
                CommonConstants.Data_CanEdit_Role
            };
                listRoleGroup = new List<ApplicationRoleGroup>();
                foreach (var roleItem in roleList)
                {
                    listRoleGroup.Add(new ApplicationRoleGroup()
                    {
                        GroupId = groupItem.ID,
                        RoleId = roleItem
                    });
                }
                _appRoleService.AddRolesToGroup(listRoleGroup, groupItem.ID);
                _appRoleService.Save();

                groupItem = _appGroupService.GetByID(CommonConstants.LAB_GROUP_NAME);
                roleList = new string[] {
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role
            };
                listRoleGroup = new List<ApplicationRoleGroup>();
                foreach (var roleItem in roleList)
                {
                    listRoleGroup.Add(new ApplicationRoleGroup()
                    {
                        GroupId = groupItem.ID,
                        RoleId = roleItem
                    });
                }
                _appRoleService.AddRolesToGroup(listRoleGroup, groupItem.ID);
                _appRoleService.Save();

                groupItem = _appGroupService.GetByID(CommonConstants.STTT_GROUP_NAME);
                roleList = new string[] {
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role
            };
                listRoleGroup = new List<ApplicationRoleGroup>();
                foreach (var roleItem in roleList)
                {
                    listRoleGroup.Add(new ApplicationRoleGroup()
                    {
                        GroupId = groupItem.ID,
                        RoleId = roleItem
                    });
                }
                _appRoleService.AddRolesToGroup(listRoleGroup, groupItem.ID);
                _appRoleService.Save();
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
                throw;
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
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
                _errorService.Create(error);
                _errorService.Save();
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