using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Models
{
    public class ApplicationUserViewModel
    {
        public string ID { set; get; }

        [Display(Name = "Họ tên người dùng")]
        public string FullName { set; get; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime? BirthDay { set; get; }

        public string Bio { set; get; }

        [Display(Name = "Hộp thư Email")]
        public string Email { set; get; }

        [Display(Name = "Mật khẩu")]
        public string Password { set; get; }

        [Display(Name = "Tài khoản")]
        public string UserName { set; get; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { set; get; }

        [MaxLength(555)]
        [Display(Name = "Tập tin ảnh")]
        public string ImagePath { get; set; }

        public HttpPostedFileBase ImageUpload { get; set; }
        public IEnumerable<ApplicationGroupViewModel> Groups { set; get; }
        public ICollection<SelectListItem> GroupsList { get; set; }
        public ICollection<SelectListItem> RolesList { get; set; }

        public ApplicationUserViewModel()
        {
            ImagePath = "~/AppFiles/Images/default.png";
            GroupsList = new List<SelectListItem>();
            RolesList = new List<SelectListItem>();
        }
    }
}