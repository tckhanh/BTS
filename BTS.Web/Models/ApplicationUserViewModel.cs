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
        [Display(Name = "Mã người dùng")]
        public string ID { set; get; }

        [Display(Name = "Họ tên người dùng")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Họ tên người dùng")]
        [StringLength(255, ErrorMessage = "Họ tên người dùng không quá 255 ký tự")]
        public string FullName { set; get; }

        [Display(Name = "Địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không quá 255 ký tự")]
        public string Address { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? BirthDay { set; get; }

        [Display(Name = "Quê quán")]
        [MaxLength(50)]
        public string FatherLand { get; set; }

        [Display(Name = "Trình độ chuyên môn")]
        [MaxLength(50)]
        public string Level { get; set; }

        [Display(Name = "Chuyên ngành đào tạo")]
        [MaxLength(150)]
        public string EducationalField { get; set; }

        [Display(Name = "Ngày vào cơ quan")]
        [DataType(DataType.Date)]
        public DateTime? EntryDate { get; set; }

        [Display(Name = "Ngày vào rời cơ quan")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Các vị trí công việc đã đảm nhiệm")]
        [MaxLength(255)]
        public string JobPositions { get; set; }

        [MaxLength(555)]
        [Display(Name = "Tập tin ảnh")]
        public string ImagePath { get; set; }

        public virtual IEnumerable<ApplicationGroupViewModel> Groups { get; set; }

        [Display(Name = "Hộp thư Email")]
        public string Email { set; get; }

        [Display(Name = "Mật khẩu")]
        public string Password { set; get; }

        [Display(Name = "Tài khoản")]
        public string UserName { set; get; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { set; get; }

        public HttpPostedFileBase ImageUpload { get; set; }
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