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
    public class AreaTabVM
    {
        [Display(Name = "Số thứ tự")]
        public int No { get; set; }

        [Display(Name = "Mã Phường/Xã")]
        [StringLength(5, ErrorMessage = "Mã Phường/Xã không quá 5 ký tự")]
        public string WardId { get; set; }

        [Display(Name = "Tên Phường/Xã")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Tên Phường/Xã")]
        [StringLength(50, ErrorMessage = "Tên Phường/Xã không quá 50 ký tự")]
        public string WardName { get; set; }
        
        [Display(Name = "Mã số Quận/Huyện")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Mã Quận/Huyện")]
        [StringLength(5, ErrorMessage = "Mã số Quận/Huyện không quá 05 ký tự")]        
        public string DistrictId { get; set; }

        [Display(Name = "Tên Quận/Huyện")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Tên Quận/Huyện")]
        [StringLength(50, ErrorMessage = "Tên Quận/Huyện không quá 50 ký tự")]
        public string DistrictName { get; set; }

        [Display(Name = "Mã Tỉnh/Thành phố")]
        [StringLength(3, ErrorMessage = "Mã số Tỉnh/Thành phố không quá 03 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Mã Tỉnh/Thành phố")]
        public string CityId { get; set; }
    }
}