using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class ApplicationRoleGroupViewModel
    {
        [Display(Name = "Mã nhóm")]
        public string GroupId { set; get; }

        [Display(Name = "Mã quyền")]
        public string RoleId { set; get; }

        public virtual ApplicationRoleViewModel ApplicationRole { set; get; }

        public virtual ApplicationGroupViewModel ApplicationGroup { set; get; }
        public ApplicationRoleGroupViewModel()
        {
            GroupId = Guid.NewGuid().ToString();
        }
    }
}