using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Model.Models
{
    // You will not likely need to customize there, but it is necessary/easier to create our own 
    // project-specific implementations, so here they are:
    public class ApplicationUserLogin : IdentityUserLogin<string> { }
    public class ApplicationUserClaim : IdentityUserClaim<string> { }
    public class ApplicationUserRole : IdentityUserRole<string> { }

    // Must be expressed in terms of our custom Role and other types:
    public class ApplicationUser: IdentityUser
    {
        [MaxLength(255)]
        [Required]
        public string FullName { get; set; }
        [MaxLength(255)]
        public string Address { get; set;}
        public DateTime? BirthDay { get; set; }
        [MaxLength(555)]
        public string ImagePath { get; set; }
        public virtual ICollection<ApplicationUserGroup> Groups { get; set; }

        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            Groups = new HashSet<ApplicationUserGroup>();
            // Add any custom User properties/code here
        }

        public async Task<ClaimsIdentity>GenerateUserIdentityAsync(UserManager<ApplicationUser, string> manager)
        {
            var userIdentity = await manager
                .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
       

    }
}
