namespace BTS.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BTS.Data.BTSDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BTS.Data.BTSDbContext context)
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
            CreateUser(context);
            CreateOperator(context);
        }

        private void CreateUser(BTSDbContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new BTSDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new BTSDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "khanh",
                Email = "tckhanh.p@gmail.com",
                EmailConfirmed = true,
                BirthDay = DateTime.Now,
                FullName = "Trần Công Khanh"

            };
            if (manager.Users.Count(x => x.UserName == "khanh") == 0)
            {
                manager.Create(user, "abc1234567");

                if (!roleManager.Roles.Any())
                {
                    roleManager.Create(new IdentityRole { Name = "Admin" });
                    roleManager.Create(new IdentityRole { Name = "User" });
                }

                var adminUser = manager.FindByEmail("tckhanh.p@gmail.com");

                manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
            }

        }

        private void CreateOperator(BTSDbContext context)
        {
            if (context.Operators.Count() == 0)
            {
                List<Operator> listOperator = new List<Operator>()
            {
                new Operator() { ID ="MOBIFONE", Name="Tổng Công Ty MobiFone", Address="Hà Mội" } ,
                new Operator() { ID ="VIETTEL", Name="Tập đoàn VIETTEL", Address="Hà Mội" } ,
                new Operator() { ID ="VINAPHONE", Name="Tập đoàn VNPT", Address="Hà Mội" }
            };
                context.Operators.AddRange(listOperator);
                context.SaveChanges();
            }
        }

    }
}
