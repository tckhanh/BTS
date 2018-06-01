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

        public virtual IEnumerable<ApplicantViewModel> Applicants { get; set; }
        public virtual IEnumerable<BtsViewModel> Btss { get; set; }
        public virtual IEnumerable<CertificateViewModel> Certificates { get; set; }
        public virtual IEnumerable<SubBtsInCertViewModel> SubBtsInCerts { get; set; }
    }
}