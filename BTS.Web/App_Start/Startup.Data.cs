using BTS.Common;
using BTS.Data;
using BTS.Model.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BTS.Web.App_Start
{
    public partial class Startup
    {
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

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        private void CreateRolesandUsers(BTSDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //    var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //    var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();

            string roleName = CommonConstants.System_Admin_Role;
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                roleManager.Create(role);
            }

            var user = userManager.FindByName(CommonConstants.SuperAdmin_Name);
            if (user == null)
            {
                user = new ApplicationUser {
                    UserName = CommonConstants.SuperAdmin_Name,
                    FullName = CommonConstants.SuperAdmin_FullName,
                    Email = CommonConstants.SuperAdmin_Email,
                    EmailConfirmed = CommonConstants.SuperAdmin_EmailConfirmed,
                };
                var result = userManager.Create(user, CommonConstants.SuperAdmin_Password);
                result = userManager.SetLockoutEnabled(user.Id, false);

            }

            var groupManager = new ApplicationGroupManager();
            var newGroup = new ApplicationGroup(CommonConstants.SUPERADMIN_GROUP, CommonConstants.SUPERADMIN_GROUP_NAME);
            groupManager.CreateGroup(newGroup);
            groupManager.SetUserGroups(user.Id, new string[] { newGroup.ID });
            groupManager.SetRoleGroups(newGroup.ID, new string[] { role.Name });

            //********************
            role = roleManager.FindByName(CommonConstants.Data_CanView_Role);
            if (role == null)
            {
                role = new ApplicationRole(CommonConstants.Data_CanView_Role, CommonConstants.Data_CanView_Role_Name);
                roleManager.Create(role);
            }

            role = roleManager.FindByName(CommonConstants.Data_CanViewDetail_Role);
            if (role == null)
            {
                role = new ApplicationRole(CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanViewDetail_Role_Name);
                roleManager.Create(role);
            }

            role = roleManager.FindByName(CommonConstants.Data_CanViewChart_Role);
            if (role == null)
            {
                role = new ApplicationRole(CommonConstants.Data_CanViewChart_Role, CommonConstants.Data_CanViewChart_Role_Name);
                roleManager.Create(role);
            }

            role = roleManager.FindByName(CommonConstants.Data_CanViewStatitics_Role);
            if (role == null)
            {
                role = new ApplicationRole(CommonConstants.Data_CanViewStatitics_Role, CommonConstants.Data_CanViewStatitics_Role_Name);
                roleManager.Create(role);
            }


            role = roleManager.FindByName(CommonConstants.Data_CanAdd_Role);
            if (role == null)
            {
                role = new ApplicationRole(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanAdd_Role_Name);
                roleManager.Create(role);
            }
 
            role = roleManager.FindByName(CommonConstants.Data_CanExport_Role);
            if (role == null)
            {
                role = new ApplicationRole(CommonConstants.Data_CanExport_Role, CommonConstants.Data_CanExport_Role_Name);
                roleManager.Create(role);
            }

            role = roleManager.FindByName(CommonConstants.Data_CanImport_Role);
            if (role == null)
            {
                role = new ApplicationRole(CommonConstants.Data_CanImport_Role, CommonConstants.Data_CanImport_Role_Name);
                roleManager.Create(role);
            }

            role = roleManager.FindByName(CommonConstants.Data_CanEdit_Role);
            if (role == null)
            {
                role = new ApplicationRole(CommonConstants.Data_CanEdit_Role, CommonConstants.Data_CanEdit_Role_Name);
                roleManager.Create(role);
            }

            role = roleManager.FindByName(CommonConstants.Data_CanDisable_Role);
            if (role == null)
            {
                role = new ApplicationRole(CommonConstants.Data_CanDisable_Role, CommonConstants.Data_CanDisable_Role_Name);
                roleManager.Create(role);
            }

            role = roleManager.FindByName(CommonConstants.Data_CanDelete_Role);
            if (role == null)
            {
                role = new ApplicationRole(CommonConstants.Data_CanDelete_Role, CommonConstants.Data_CanDelete_Role_Name);
                roleManager.Create(role);
            }

            newGroup = new ApplicationGroup(CommonConstants.DIRECTOR_GROUP, CommonConstants.DIRECTOR_GROUP_NAME);
            groupManager.CreateGroup(newGroup);
            newGroup = new ApplicationGroup(CommonConstants.VERTIFICATIONHEADER_GROUP, CommonConstants.VERTIFICATIONHEADER_GROUP_NAME);
            groupManager.CreateGroup(newGroup);
            newGroup = new ApplicationGroup(CommonConstants.VERTIFICATION_GROUP, CommonConstants.VERTIFICATION_GROUP_NAME);
            groupManager.CreateGroup(newGroup);
            newGroup = new ApplicationGroup(CommonConstants.LAB_GROUP, CommonConstants.LAB_GROUP_NAME);
            groupManager.CreateGroup(newGroup);
            newGroup = new ApplicationGroup(CommonConstants.STTT_GROUP, CommonConstants.STTT_GROUP_NAME);
            groupManager.CreateGroup(newGroup);
        }
        //private void CreateRolesandUsers(BTSDbContext context)
        //{
        //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        //    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

        //    // In Startup iam creating first Admin Role and creating a default Admin User
        //    if (!roleManager.RoleExists("Admin"))
        //    {
        //        // first we create Admin rool
        //        var role = new IdentityRole();
        //        role.Name = "Admin";
        //        roleManager.Create(role);

        //        //Here we create a Admin super user who will maintain the website

        //        var user = new ApplicationUser();
        //        user.UserName = "admin";
        //        user.Email = "tckhanh.p@gmail.com";
        //        user.EmailConfirmed = true;
        //        user.BirthDay = DateTime.Now;
        //        user.FullName = "Trần Công Khanh";

        //        string userPWD = "admin";

        //        if (userManager.Users.Count(x => x.UserName == user.UserName) != 0)
        //        {
        //            var chkUser = userManager.Create(user, userPWD);
        //            chkUser = userManager.SetLockoutEnabled(user.Id, false);

        //            //Add default User to Role Admin
        //            if (chkUser.Succeeded)
        //            {

        //                var result1 = userManager.AddToRole(user.Id, "Admin");
        //            }
        //        }
        //    }

        //    // creating Creating Manager role
        //    if (!roleManager.RoleExists("Manager"))
        //    {
        //        var role = new IdentityRole();
        //        role.Name = "Manager";
        //        roleManager.Create(role);
        //    }

        //    // creating Creating Employee role
        //    if (!roleManager.RoleExists("Employee"))
        //    {
        //        var role = new IdentityRole();
        //        role.Name = "Employee";
        //        roleManager.Create(role);
        //    }

        //    // creating Creating Guest role
        //    if (!roleManager.RoleExists("Guest"))
        //    {
        //        var role = new IdentityRole();
        //        role.Name = "Guest";
        //        roleManager.Create(role);
        //    }
        //}

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