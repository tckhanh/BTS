using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Models
{
    public class ApplicationRoleViewModel
    {
        [Display(Name = "Mã Quyền")]
        public string Id { set; get; }

        [Display(Name = "Tên Quyền")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Tên Quyền")]
        public string Name { set; get; }

        [Display(Name = "Mô tả quyền")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Mô tả quyền")]
        [MaxLength(250, ErrorMessage = "Mô tả quyền không quá 250 ký tự")]
        public string Description { set; get; }

        [Display(Name = "Danh sách các nhóm được cấp quyền")]
        public ICollection<SelectListItem> GroupList { get; set; }

        [Display(Name = "Danh sách các người dùng được cấp quyền")]
        public ICollection<SelectListItem> UserList { get; set; }

        public ApplicationRoleViewModel()
        {
            GroupList = new List<SelectListItem>();
            UserList = new List<SelectListItem>();
        }
    }
}