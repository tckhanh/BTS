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
        public string Name { get; set; }

        public virtual IEnumerable<BtsViewModel> BTSs { get; set; }

        public virtual IEnumerable<CertificateViewModel> Certificates { get; set; }
    }
}