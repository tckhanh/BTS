using BTS.Model.Models;
using BTS.Web.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class CityViewModel
    {
        [Display(Name = "Mã Tỉnh/Thành phố")]
        [StringLength(3, ErrorMessage = "Mã Tỉnh/Thành phố không quá 3 ký tự")]
        public string Id { get; set; }

        [Display(Name = "Tên Tỉnh/Thành phố")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Tên Tỉnh/Thành phố")]
        [StringLength(50, ErrorMessage = "Tên Tỉnh/Thành phố không quá 50 ký tự")]
        [Unique(ErrorMessage = "Tên Tỉnh/Thành phố đã tồn tại rồi !!", TargetModelType = typeof(City), TargetPropertyName = "Name")]
        public string Name { get; set; }

        public virtual IEnumerable<BtsViewModel> BTSs { get; set; }

        public virtual IEnumerable<CertificateViewModel> Certificates { get; set; }
    }
}