using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class OperatorViewModel
    {
        [Required(ErrorMessage = "Yêu cầu nhập Mã Doanh nghiệp")]
        [MaxLength(10, ErrorMessage = "Mã Doanh nghiệp tối đa 10 ký tự")]
        public string ID { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập Tên Doanh nghiệp")]
        [MaxLength(255, ErrorMessage = "Tên Doanh nghiệp tối đa 255 ký tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yêu cầu Địa chỉ Doanh nghiệp")]
        [MaxLength(255, ErrorMessage = "Địa chỉ Doanh nghiệp tối đa 255 ký tự")]
        public string Address { get; set; }

        [MaxLength(30, ErrorMessage = "Số điện thoại tối đa 30 ký tự")]
        public string Telephone { get; set; }

        [MaxLength(30, ErrorMessage = "Số Fax tối đa 30 ký tự")]
        public string Fax { get; set; }

        public virtual ICollection<ApplicantViewModel> Applicants { get; set; }

        public virtual ICollection<BTSCertificateViewModel> BTSCertificates { get; set; }

        public virtual ICollection<SubBTSViewModel> SubBTSs { get; set; }
    }
}