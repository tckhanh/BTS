using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class SubBtsInCertViewModel
    {
        [Display(Name = "Mã Số")]
        public string Id { get; set; }

        [Display(Name = "Số Giấy CNKĐ")]
        [StringLength(16, ErrorMessage = "Số Giấy CNKĐ không quá 16 ký tự")]
        public string CertificateID { get; set; }

        [Display(Name = "Trạm BTS thứ")]
        public int BtsSerialNo { get; set; }

        [Display(Name = "Mã trạm BTS")]
        [StringLength(50, ErrorMessage = "Mã trạm BTS không quá 50 ký tự")]
        public string BtsCode { get; set; }

        [Display(Name = "Mã nhà mạng")]
        [StringLength(20, ErrorMessage = "Mã nhà mạng không quá 10 ký tự")]
        public string OperatorID { get; set; }

        [Display(Name = "Hãng sản xuất")]
        [StringLength(50, ErrorMessage = "Hãng sản xuất không quá 50 ký tự")]
        public string Manufactory { get; set; }

        [Display(Name = "Thiết bị")]
        [StringLength(50, ErrorMessage = "Thiết bị không quá 50 ký tự")]
        public string Equipment { get; set; }

        [Display(Name = "Số Anten")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Số Anten")]
        [Range(1, int.MaxValue, ErrorMessage = "Yêu cầu nhập Số Anten là số nguyên trong phạm vi [1->2147483647]")]
        [RegularExpression(@"[1-9][0-9]*$", ErrorMessage = "Yêu cầu nhập Số Anten là số nguyên")]
        public int AntenNum { get; set; }

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
        public SubBtsInCertViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}