using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class CertificateViewModel
    {
        [Key]
        [Display(Name = "Số giấy CNKĐ")]
        public string ID { get; set; }

        public int? ProfileID { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "Nhà mạng")]
        public string OperatorID { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Mã trạm BTS")]
        public string BtsCode { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Required]
        [MaxLength(3)]
        [Display(Name = "Mã tỉnh/TP")]
        public string CityID { get; set; }

        [Display(Name = "Kinh độ")]
        public double? Longtitude { get; set; }

        [Display(Name = "Vĩ độ")]
        public double? Latitude { get; set; }

        public int InCaseOfID { get; set; }

        [MaxLength(20)]
        [Display(Name = "Phòng đo kiểm")]
        public string LabID { get; set; }

        [MaxLength(30)]
        [Display(Name = "Số KQ đo kiểm")]
        public string TestReportNo { get; set; }

        [Display(Name = "Ngày KQ đo kiểm")]
        public DateTime TestReportDate { get; set; }

        [Display(Name = "Ngày ban hành")]
        public DateTime? IssuedDate { get; set; }

        [Display(Name = "Ngày hết hạn")]
        public DateTime? ExpiredDate { get; set; }

        [MaxLength(30)]
        [Required]
        [Display(Name = "Nơi cấp")]
        public string IssuedPlace { get; set; }

        [MaxLength(30)]
        [Required]
        [Display(Name = "Người ký tên")]
        public string Signer { get; set; }

        [Display(Name = "Số trạm BTS")]
        public int SubBtsQuantity { get; set; }

        [MaxLength(255)]
        [Required]
        public string SubBtsCodes { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsOperatorIDs { get; set; }

        [MaxLength(255)]
        [Required]
        public string SubBtsEquipments { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsAntenNums { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsConfigurations { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsPowerSums { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsBands { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsAntenHeights { get; set; }

        public double? MinAntenHeight { get; set; }

        public double? MaxHeightIn100m { get; set; }

        public double? OffsetHeight { get; set; }

        public double? SafeLimit { get; set; }
    }
}