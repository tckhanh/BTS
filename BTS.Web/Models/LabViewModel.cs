﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class LabViewModel: AuditableViewModel
    {
        [Display(Name = "Mã Phòng Đo kiểm")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Mã Phòng Đo kiểm")]
        [StringLength(20, ErrorMessage = "Mã Phòng Đo kiểm không quá 20 ký tự")]
        public string Id { get; set; }

        [Display(Name = "Tên Phòng Đo kiểm")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Tên Phòng Đo kiểm")]
        [StringLength(255, ErrorMessage = "Tên Phòng Đo kiểm không quá 255 ký tự")]
        public string Name { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không quá 255 ký tự")]
        public string Address { get; set; }

        [Display(Name = "Số điện thoại")]
        [StringLength(30, ErrorMessage = "Số điện thoại không quá 30 ký tự")]
        public string Phone { get; set; }

        [Display(Name = "Số Fax")]
        [StringLength(30, ErrorMessage = "Số Fax không quá 30 ký tự")]
        public string Fax { get; set; }

        public virtual IEnumerable<CertificateViewModel> Certificates { get; set; }
    }
}