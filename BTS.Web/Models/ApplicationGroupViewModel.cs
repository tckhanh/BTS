using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class ApplicationGroupViewModel
    {
        [Display(Name = "Mã nhóm")]
        public string ID { get; set; }

        [Display(Name = "Tên nhóm")]
        [Required(ErrorMessage = "Yêu cầu nhập Tên nhóm")]
        [StringLength(250, ErrorMessage = "Tên nhóm không quá 250 ký tự")]
        public string Name { get; set; }

        [Display(Name = "Mô tả nhóm")]
        [StringLength(250, ErrorMessage = "Mô tả nhóm không quá 250 ký tự")]
        public string Description { set; get; }

        public virtual IEnumerable<ApplicationRoleViewModel> ApplicationRoles { get; set; }
        public virtual IEnumerable<ApplicationUserViewModel> ApplicationUsers { get; set; }

        public virtual IEnumerable<ApplicationRoleViewModel> Roles { set; get; }
    }
}