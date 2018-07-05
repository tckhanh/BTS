using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class InCaseOfViewModel
    {
        [Display(Name = "Mã Trường hợp Kiểm định")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Mã Trường hợp Kiểm định")]
        public int Id { get; set; }

        [Display(Name = "Tên Trường hợp Kiểm định")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Tên trường hợp Kiểm định")]
        [StringLength(50, ErrorMessage = "Tên Trường hợp Kiểm định không quá 50 ký tự")]
        public string Name { get; set; }

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { set; get; }

        [Display(Name = "Người tạo")]
        [StringLength(256, ErrorMessage = "Người tạo không quá 256 ký tự")]
        public string CreatedBy { set; get; }

        [Display(Name = "Ngày cập nhật")]
        [DataType(DataType.Date)]
        public DateTime? UpdatedDate { set; get; }

        [Display(Name = "Người cập nhật")]
        [StringLength(256, ErrorMessage = "Người cập nhật không quá 256 ký tự")]
        public string UpdatedBy { set; get; }

        [Display(Name = "Ngày xóa")]
        [DataType(DataType.Date)]
        public DateTime? DeletedDate { set; get; }

        [Display(Name = "Người xóa")]
        [StringLength(256, ErrorMessage = "Người xóa không quá 256 ký tự")]
        public string DeletedBy { set; get; }

        public virtual IEnumerable<BtsViewModel> Btss { get; set; }

        public virtual IEnumerable<CertificateViewModel> Certificates { get; set; }
    }
}