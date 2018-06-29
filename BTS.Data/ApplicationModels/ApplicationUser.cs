using BTS.Model.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.ApplicationModels
{
    // Must be expressed in terms of our custom Role and other types:
    public class ApplicationUser
    : IdentityUser<string, ApplicationUserLogin,
    ApplicationUserRole, ApplicationUserClaim>, IAuditable
    {
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        public DateTime? BirthDay { get; set; }

        [MaxLength(50)]
        public string FatherLand { get; set; }

        [MaxLength(50)]
        public string Level { get; set; }

        [MaxLength(150)]
        public string EducationalField { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? OfficialDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(50)]
        public string WorkingDuration { get; set; }

        [MaxLength(255)]
        public string JobPositions { get; set; }

        [MaxLength(555)]
        public string ImagePath { get; set; }

        [DefaultValue("true")]
        public bool Locked { get; set; } = true;

        public virtual ICollection<ApplicationUserGroup> Groups { get; set; }

        public DateTime? CreatedDate
        { get; set; }

        public string CreatedBy
        { get; set; }

        public DateTime? UpdatedDate
        { get; set; }

        public string UpdatedBy
        { get; set; }

        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            Groups = new HashSet<ApplicationUserGroup>();
            // Add any custom User properties/code here
            //Groups = new List<ApplicationUserGroup>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, string> manager)
        {
            var userIdentity = await manager
                .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}