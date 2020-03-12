using BTS.Model.Models;
using BTS.Web.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Models
{
    public class ApplicantTabVM
    {
        [Display(Name = "Số thứ tự")]
        public int No { get; set; }

        [Display(Name = "Mã đơn vị nộp hồ sơ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Mã đơn vị nộp hồ sơ")]
        [StringLength(50, ErrorMessage = "Mã đơn vị nộp hồ sơ không quá 50 ký tự")]
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Tên đơn vị nộp hồ sơ")]
        [StringLength(255, ErrorMessage = "Tên đơn vị nộp hồ sơ không quá 255 ký tự")]
        [Display(Name = "Tên đơn vị nộp hồ sơ")]
        [DataType(DataType.MultilineText)]
        [Unique(ErrorMessage = "Tên đơn vị nộp hồ sơ đã tồn tại rồi !!", TargetModelType = typeof(Applicant), TargetPropertyName = "Name")]
        public string Name { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không quá 255 ký tự")]
        [DataType(DataType.MultilineText)]
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
    }
}