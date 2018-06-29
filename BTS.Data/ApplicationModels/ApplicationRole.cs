using BTS.Model.Abstract;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;

namespace BTS.Data.ApplicationModels
{
    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>, IAuditable
    {
        [StringLength(250)]
        public string Description { set; get; }

        public DateTime? CreatedDate
        { get; set; }

        public string CreatedBy
        { get; set; }

        public DateTime? UpdatedDate
        { get; set; }

        public string UpdatedBy
        { get; set; }

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