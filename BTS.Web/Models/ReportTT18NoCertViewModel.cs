using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Web.Models
{
    public class ReportTT18NoCertViewModel
    {

        [Display(Name = "Mã nhà mạng")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập mã nhà mạng")]
        [StringLength(10, ErrorMessage = "Mã nhà mạng không quá 10 ký tự")]
        public string OperatorID { get; set; }

        [Display(Name = "Mã trạm BTS")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập mã trạm BTS")]
        [StringLength(100)]
        public string BtsCode { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập địa chỉ trạm BTS")]
        [StringLength(255, ErrorMessage = "Địa chỉ trạm BTS không quá 255 ký tự")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "Mã Tỉnh/Thành phố")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập mã Tỉnh/Thành phố")]
        [StringLength(3, ErrorMessage = "Mã Tỉnh/Thành phố BTS không quá 3 ký tự")]
        public string CityID { get; set; }

        [Display(Name = "Kinh độ")]
        public double? Longtitude { get; set; }

        [Display(Name = "Vĩ độ")]
        public double? Latitude { get; set; }

        [Display(Name = "Số trạm BTS")]
        public int SubBtsQuantity { get; set; }

        // SubBtsInCert Field
        [Display(Name = "Mã trạm BTS")]
        [StringLength(50, ErrorMessage = "Mã trạm BTS không quá 50 ký tự")]
        public string SubBtsCode { get; set; }

        [Display(Name = "Mã nhà mạng")]
        [StringLength(10, ErrorMessage = "Mã nhà mạng không quá 10 ký tự")]
        public string SubOperatorID { get; set; }

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

        [StringLength(20)]
        public string LabID { get; set; }

        [StringLength(30)]
        public string TestReportNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime TestReportDate { get; set; }

        [StringLength(255)]
        [Required]
        public string ReasonNoCertificate { get; set; }
    }
}