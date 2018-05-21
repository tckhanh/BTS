using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class ApplicationUserGroupViewModel
    {
        [Display(Name = "Mã người dùng")]
        public string UserId { set; get; }

        [Display(Name = "Mã nhóm")]
        public string GroupId { set; get; }

        public virtual ApplicationUserViewModel ApplicationUser { set; get; }

        public virtual ApplicationGroupViewModel ApplicationGroup { set; get; }
    }
}