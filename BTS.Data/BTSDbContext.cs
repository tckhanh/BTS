using BTS.Model.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data
{
    public class BTSDbContext: IdentityDbContext<ApplicationUser>
    {
        public BTSDbContext(): base("BTSConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<InCaseOf> InCaseOfs { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Bts> Btss { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<SubBtsInCert> SubBtsInCerts { get; set; }

        public DbSet<Error> Errors { get; set; }
        public DbSet<Footer> Footers { set; get; }
        public DbSet<Menu> Menus { set; get; }
        public DbSet<MenuGroup> MenuGroups { set; get; }
        public DbSet<Page> Pages { set; get; }        
        public DbSet<Slide> Slides { set; get; }
        public DbSet<SupportOnline> SupportOnlines { set; get; }
        public DbSet<SystemConfig> SystemConfigs { set; get; }
        
        public DbSet<VisitorStatistic> VisitorStatistics { set; get; }        
        public DbSet<ContactDetail> ContactDetails { set; get; }
        public DbSet<Feedback> Feedbacks { set; get; }

        public DbSet<ApplicationGroup> ApplicationGroups { set; get; }
        public DbSet<ApplicationRole> ApplicationRoles { set; get; }
        public DbSet<ApplicationRoleGroup> ApplicationRoleGroups { set; get; }
        public DbSet<ApplicationUserGroup> ApplicationUserGroups { set; get; }

        public static BTSDbContext Create()
        {
            return new BTSDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("ApplicationUserRoles");
            builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("ApplicationUserLogins");
            builder.Entity<IdentityRole>().ToTable("ApplicationRoles");
            builder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("ApplicationUserClaims");
        }
    }
}
