using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;

namespace BTS.Model.Models
{
    public class ApplicationRole : IdentityRole
    {
        [StringLength(250)]
        public string Description { set; get; }

        public ApplicationRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ApplicationRole(string name) : this()
        {
            Name = name;
        }

        public ApplicationRole(string name, string description) : this(name)
        {
            Description = description;
        }

    }
}