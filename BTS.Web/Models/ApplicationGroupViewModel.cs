using BTS.Data.ApplicationModels;
using BTS.Web.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Models
{
    public class ApplicationGroupViewModel
    {
        [Display(Name = "Mã nhóm")]
        public string Id { get; set; }

        [Display(Name = "Tên nhóm")]
        [Required(ErrorMessage = "Yêu cầu nhập Tên nhóm")]
        [StringLength(250, ErrorMessage = "Tên nhóm không quá 250 ký tự")]
        [Unique(ErrorMessage = "Tên Nhóm đã tồn tại rồi !!", TargetModelType = typeof(ApplicationGroup), TargetPropertyName = "Name")]
        public string Name { get; set; }

        [Display(Name = "Mô tả nhóm")]
        [Required(ErrorMessage = "Yêu cầu nhập Mô tả nhóm")]
        [StringLength(250, ErrorMessage = "Mô tả nhóm không quá 250 ký tự")]
        public string Description { set; get; }

        public virtual IEnumerable<ApplicationRoleGroupViewModel> ApplicationRoleGroups { get; set; }
        public virtual IEnumerable<ApplicationUserGroupViewModel> ApplicationUserGroups { get; set; }

        //public virtual IEnumerable<ApplicationRoleViewModel> Roles { set; get; }

        [Display(Name = "Các quyền được cấp cho nhóm")]
        public ICollection<SelectListItem> RoleList { get; set; }

        [Display(Name = "Danh sách các Người dùng thuộc nhóm")]
        public ICollection<SelectListItem> UserList { get; set; }

        public ApplicationGroupViewModel()
        {
            Id = Guid.NewGuid().ToString();
            RoleList = new List<SelectListItem>();
            UserList = new List<SelectListItem>();
            ApplicationRoleGroups = new List<ApplicationRoleGroupViewModel>();
            ApplicationUserGroups = new List<ApplicationUserGroupViewModel>();
        }

        public ApplicationGroupViewModel(string name) : this()
        {
            Name = name;
        }

        public ApplicationGroupViewModel(string name, string description) : this(name)
        {
            this.Description = description;
        }
    }
}