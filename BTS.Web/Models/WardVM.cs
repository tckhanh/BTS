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
    public class WardVM
    {
        [Display(Name = "Mã Phường/Xã")]
        [StringLength(5, ErrorMessage = "Mã Phường/Xã không quá 5 ký tự")]
        public string Id { get; set; }

        [Display(Name = "Tên Phường/Xã")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Tên Phường/Xã")]
        [StringLength(50, ErrorMessage = "Tên Phường/Xã không quá 50 ký tự")]
        public string Name { get; set; }
        
        [Display(Name = "Mã số Quận/Huyện")]
        [StringLength(5, ErrorMessage = "Mã số Quận/Huyện không quá 05 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Mã Quận/Huyện")]
        public string DistrictId { get; set; }

        public virtual DistrictVM District { get; set; }

        [Display(Name = "Tên Quận/Huyện")]
        public string DistrictName { get; set; }

        [Display(Name = "Thuộc Quận/Huyện")]
        public ICollection<SelectListItem> DistrictList { get; set; }

        public WardVM()
        {
            DistrictList = new List<SelectListItem>();
        }
    }
}