using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class SubBtsInCertViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Số Giấy CNKĐ")]
        [StringLength(16, ErrorMessage = "Số Giấy CNKĐ không quá 16 ký tự")]
        public string CertificateID { get; set; }

        [Display(Name = "Mã trạm BTS")]
        [StringLength(50, ErrorMessage = "Mã trạm BTS không quá 50 ký tự")]
        public string BtsCode { get; set; }

        [Display(Name = "Mã nhà mạng")]
        [StringLength(10, ErrorMessage = "Mã nhà mạng không quá 10 ký tự")]
        public string OperatorID { get; set; }

        [Display(Name = "Hãng sản xuất")]
        [StringLength(50, ErrorMessage = "Hãng sản xuất không quá 50 ký tự")]
        public string Manufactory { get; set; }

        [Display(Name = "Thiết bị")]
        [StringLength(50, ErrorMessage = "Thiết bị không quá 50 ký tự")]
        public string Equipment { get; set; }

        [Display(Name = "Số Anten")]
        public int? AntenNum { get; set; }

        [Display(Name = "Cấu hình máy phát")]
        [StringLength(30, ErrorMessage = "Cấu hình máy phát không quá 30 ký tự")]
        public string Configuration { get; set; }

        [Display(Name = "Công suất máy phát")]
        [StringLength(30, ErrorMessage = "Công suất máy phát không quá 30 ký tự")]
        public string PowerSum { get; set; }

        [Display(Name = "Băng tần")]
        [StringLength(30, ErrorMessage = "Băng tần không quá 30 ký tự")]
        public string Band { get; set; }

        [Display(Name = "Độ cao Anten")]
        [StringLength(30, ErrorMessage = "Độ cao Anten không quá 30 ký tự")]
        public string AntenHeight { get; set; }

        public virtual CertificateViewModel Certificates { get; set; }

        public virtual OperatorViewModel Operator { get; set; }
    }
}