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
    public class DistrictVM
    {
        [Display(Name = "Mã Quận/Huyện")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Mã Quận/Huyện")]
        [StringLength(5, ErrorMessage = "Mã Quận/Huyện không quá 5 ký tự")]
        public string Id { get; set; }

        [Display(Name = "Tên Quận/Huyện")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Tên Quận/Huyện")]
        [StringLength(50, ErrorMessage = "Tên Quận/Huyện không quá 50 ký tự")]
        public string Name { get; set; }
        
        [Display(Name = "Mã số Tỉnh/Thành phố")]
        [StringLength(3, ErrorMessage = "Mã số Tỉnh/Thành phố không quá 03 ký tự")]
        [MaxLength(3)]
        public string CityId { get; set; }

        public virtual IEnumerable<WardVM> Wards { get; set; }

        [Display(Name = "Thuộc Tỉnh/ Thành phố")]
        public ICollection<SelectListItem> CityList { get; set; }

        public DistrictVM()
        {
            CityList = new List<SelectListItem>();
        }
    }
}