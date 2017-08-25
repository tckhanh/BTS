using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data
{
    public class BTSDbContext: DbContext
    {
        public BTSDbContext(): base("BTSConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<BTSCertificate> BTSCertificates { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<SubBTS> SubBTSs { get; set; }
        public DbSet<Error> Errors { get; set; }

        public static BTSDbContext Create()
        {
            return new BTSDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
        }
    }
}
