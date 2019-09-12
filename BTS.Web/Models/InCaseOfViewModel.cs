using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BTS.Web.Infrastructure.Extensions;
using System.Linq;
using System.Web;
using BTS.Model.Models;

namespace BTS.Web.Models
{
    public class InCaseOfViewModel: AuditableViewModel
    {
        [Display(Name = "Mã Trường hợp Kiểm định")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Mã Trường hợp Kiểm định")]
        [Range(1, int.MaxValue, ErrorMessage = "Yêu cầu nhập Mã Trường hợp Kiểm định là số nguyên trong phạm vi [1->2147483647]")]
        [RegularExpression(@"[1-9][0-9]*$", ErrorMessage = "Yêu cầu nhập Mã Trường hợp Kiểm định là số nguyên")]
        public int Id { get; set; }

        [Display(Name = "Tên Trường hợp Kiểm định")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Tên trường hợp Kiểm định")]
        [StringLength(50, ErrorMessage = "Tên Trường hợp Kiểm định không quá 50 ký tự")]
        [Unique(ErrorMessage = "Tên Trường hợp Kiểm định đã tồn tại rồi !!", TargetModelType = typeof(InCaseOf), TargetPropertyName = "Name")]
        public string Name { get; set; }

        public virtual IEnumerable<BtsViewModel> Btss { get; set; }

        public virtual IEnumerable<CertificateViewModel> Certificates { get; set; }
    }
}