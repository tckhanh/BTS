using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class InCaseOfViewModel: AuditableViewModel
    {
        [Display(Name = "Mã Trường hợp Kiểm định")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Mã Trường hợp Kiểm định")]
        public int Id { get; set; }

        [Display(Name = "Tên Trường hợp Kiểm định")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Tên trường hợp Kiểm định")]
        [StringLength(50, ErrorMessage = "Tên Trường hợp Kiểm định không quá 50 ký tự")]
        public string Name { get; set; }

        public virtual IEnumerable<BtsViewModel> Btss { get; set; }

        public virtual IEnumerable<CertificateViewModel> Certificates { get; set; }
    }
}