using BTS.Data;
using BTS.Model.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.Data.ApplicationModels
{
    public class ApplicationRoleStore
  : RoleStore<ApplicationRole, string, ApplicationUserRole>,
  IQueryableRoleStore<ApplicationRole, string>,
  IRoleStore<ApplicationRole, string>, IDisposable
    {
        public ApplicationRoleStore()
            : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationRoleStore(BTSDbContext context)
            : base(context)
        {
        }
    }
}