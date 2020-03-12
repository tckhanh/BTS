using BTS.Data.ApplicationModels;
using BTS.Model.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BTS.Data
{
    // Must be expressed in terms of our custom types:    
    public class BTSDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole,
        string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public DbSet<InCaseOf> InCaseOfs { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Bts> Btss { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<SubBtsInCert> SubBtsInCerts { get; set; }
        public DbSet<NoCertificate> NoCertificates { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<Footer> Footers { set; get; }
        public DbSet<Menu> Menus { set; get; }
        public DbSet<MenuGroup> MenuGroups { set; get; }
        public DbSet<WebPage> Pages { set; get; }
        public DbSet<Slide> Slides { set; get; }
        public DbSet<SupportOnline> SupportOnlines { set; get; }
        public DbSet<SystemConfig> SystemConfigs { set; get; }

        public DbSet<VisitorStatistic> VisitorStatistics { set; get; }
        public DbSet<ContactDetail> ContactDetails { set; get; }
        public DbSet<Feedback> Feedbacks { set; get; }
        public DbSet<ApplicationGroup> ApplicationGroups { set; get; }
        public DbSet<ApplicationRoleGroup> ApplicationRoleGroups { set; get; }
        public DbSet<ApplicationUserGroup> ApplicationUserGroups { set; get; }
        public DbSet<Licence> Licences { set; get; }
        public BTSDbContext() : base("BTSConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        static BTSDbContext()
        {
            //For No Init Database SQL Server
            //Database.SetInitializer<BTSDbContext>(new IdentityDbInit());
            //For InitDatabase SQL Server
            Database.SetInitializer<BTSDbContext>(new MyDbInitializer());            
        }

        public static BTSDbContext Create()
        {
            return new BTSDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("ApplicationUserRoles");
            modelBuilder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("ApplicationUserLogins");
            //modelBuilder.Entity<IdentityRole>().ToTable("ApplicationRoles");
            modelBuilder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("ApplicationUserClaims");
        }
    }

    public class IdentityDbInit : NullDatabaseInitializer<BTSDbContext>
    {
    }

    public class MyDbInitializer: CreateDatabaseIfNotExists<BTSDbContext>, IDatabaseInitializer<BTSDbContext>
    {
        protected override void Seed(BTSDbContext context)
        {
            // create 3 students to seed the database
            //context.Students.Add(new Student { ID = 1, FirstName = "Mark", LastName = "Richards", EnrollmentDate = DateTime.Now });
            //context.Students.Add(new Student { ID = 2, FirstName = "Paula", LastName = "Allen", EnrollmentDate = DateTime.Now });
            //context.Students.Add(new Student { ID = 3, FirstName = "Tom", LastName = "Hoover", EnrollmentDate = DateTime.Now });

            base.Seed(context);
        }

        public override void InitializeDatabase(BTSDbContext context)
        {
            if (!context.Database.Exists())
            {
                // if database did not exist before - create it
                context.Database.Create();
                InitDatabase.SetupRolesGroups(context);
                InitDatabase.GrantDefaultRolesForGroups(context);
                InitDatabase.CreateSuperUser(context);

                InitDatabase.CreateOperator(context);
                InitDatabase.CreateSlide(context);
                InitDatabase.CreatePage(context);
                InitDatabase.CreateContactDetail(context);
                InitDatabase.CreateConfigTitle(context);
            }
            else
            {
                // query to check if MigrationHistory table is present in the database 
                var migrationHistoryTableExists = ((IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<int>(
                string.Format(
                  "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{0}' AND table_name = '__MigrationHistory'",
                  "bts"));

                // if MigrationHistory table is not there (which is the case first time we run) - create it
                if (migrationHistoryTableExists.FirstOrDefault() == 0)
                {
                    context.Database.Delete();
                    context.Database.Create();
                    InitDatabase.GrantDefaultRolesForGroups(context);
                    InitDatabase.CreateOperator(context);
                    InitDatabase.CreateSlide(context);
                    InitDatabase.CreatePage(context);
                    InitDatabase.CreateContactDetail(context);
                    InitDatabase.CreateConfigTitle(context);
                }
            }
        }
    }

}