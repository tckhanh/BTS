using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("ApplicationGroups")]
    public class ApplicationGroup
    {
        [Key]
        public string ID { set; get; }

        [StringLength(250)]
        public string Name { set; get; }

        [StringLength(250)]
        public string Description { set; get; }

        public virtual IEnumerable<ApplicationRoleGroup> ApplicationRoles { get; set; }
        public virtual IEnumerable<ApplicationUserGroup> ApplicationUsers { get; set; }

        public ApplicationGroup()
        {
            this.ID = Guid.NewGuid().ToString();
            this.ApplicationRoles = new List<ApplicationRoleGroup>();
            this.ApplicationUsers = new List<ApplicationUserGroup>();
        }

        public ApplicationGroup(string name) : this()
        {
            Name = name;
        }

        public ApplicationGroup(string name, string description) : this(name)
        {
            this.Description = description;
        }
    }
}