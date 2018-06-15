using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class ApplicantViewModel
    {
        [Display(Name = "Mã Đơn vị nộp hồ sơ")]
        [StringLength(50, ErrorMessage = "Mã đơn vị nộp hồ sơ không quá 50 ký tự")]
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Tên đơn vị nộp hồ sơ")]
        [StringLength(255, ErrorMessage = "Tên đơn vị nộp hồ sơ không quá 255 ký tự")]
        [Display(Name = "Tên đơn vị nộp hồ sơ ")]
        public string Name { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không quá 255 ký tự")]
        public string Address { get; set; }

        [Display(Name = "Số điện thoại")]
        [StringLength(30, ErrorMessage = "Số điện thoại không quá 30 ký tự")]
        public string Phone { get; set; }

        [Display(Name = "Số Fax")]
        [StringLength(30, ErrorMessage = "Số Fax không quá 30 ký tự")]
        public string Fax { get; set; }

        [Display(Name = "Đầu mối liên hệ")]
        [StringLength(100, ErrorMessage = "Đầu mối liên hệ không quá 100 ký tự")]
        public string ContactName { get; set; }

        [Display(Name = "Mã nhà mạng")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu phải nhập Mã nhà mạng")]
        [StringLength(10, ErrorMessage = "Mã nhà mạng không quá 10 ký tự")]
        public string OperatorID { get; set; }

        public virtual OperatorViewModel Operator { get; set; }

        public virtual ICollection<ProfileViewModel> Profiles { get; set; }
    }
}