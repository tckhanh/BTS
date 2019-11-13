using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Models
{
    public class PrintCertificateViewModel
    {
        [Display(Name = "Số giấy CNKĐ")]
        [StringLength(16, ErrorMessage = "Số Giấy CNKĐ không quá 16 ký tự")]
        public string Id { get; set; }

        [Display(Name = "Mã số hồ sơ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập mã số hồ sơ")]
        public string ProfileID { get; set; }

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

        [Display(Name = "Ngày ban hành")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime IssuedDate { get; set; }

        [Display(Name = "Ngày hết hạn")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpiredDate { get; set; }

        [StringLength(30, ErrorMessage = "Nơi cấp không quá 30 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Nơi cấp")]
        [Display(Name = "Nơi cấp")]
        public string IssuedPlace { get; set; }

        [StringLength(30, ErrorMessage = "Tên Người ký không quá 30 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Người ký tên")]
        [Display(Name = "Người ký tên")]
        public string Signer { get; set; }

        [Display(Name = "Số BTS tại trạm")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Số BTS tại trạm")]
        [Range(1, int.MaxValue, ErrorMessage = "Yêu cầu nhập Số BTS tại trạm là số nguyên trong phạm vi [1->2147483647]")]
        [RegularExpression(@"[1-9][0-9]*$", ErrorMessage = "Yêu cầu nhập Số BTS tại trạm là số nguyên")]
        public int SubBtsQuantity { get; set; }

        [Display(Name = "Danh sách mã trạm BTS")]
        [StringLength(255, ErrorMessage = "Danh sách mã trạm BTS không quá 255 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách mã trạm BTS")]
        [DataType(DataType.MultilineText)]
        public string SubBtsCodes { get; set; }

        [Display(Name = "Danh sách mã nhà mạng của mỗi BTS")]
        [StringLength(150, ErrorMessage = "Danh sách mã nhà mạng không quá 150 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách mã nhà mạng")]
        public string SubBtsOperatorIDs { get; set; }

        [Display(Name = "Danh sách thiết bị của mỗi BTS")]
        [StringLength(255, ErrorMessage = "Danh sách thiết bị không quá 255 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách thiết bị")]
        [DataType(DataType.MultilineText)]
        public string SubBtsEquipments { get; set; }

        [Display(Name = "Danh sách số Anten của mỗi BTS")]
        [StringLength(150, ErrorMessage = "Danh sách số Anten không quá 150 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách số Anten")]
        public string SubBtsAntenNums { get; set; }

        [Display(Name = "Các Anten dùng chung")]
        [StringLength(50, ErrorMessage = "Các Anten dùng chung không quá 150 ký tự")]
        public string SharedAntens { get; set; }

        [MaxLength(150)]
        [Display(Name = "Danh sách cấu hình của mỗi BTS")]
        [StringLength(150, ErrorMessage = "Danh sách cấu hình không quá 150 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách cấu hình")]
        public string SubBtsConfigurations { get; set; }

        [MaxLength(150)]
        [Display(Name = "Danh sách công suất của mỗi BTS")]
        [StringLength(150, ErrorMessage = "Danh sách công suất không quá 150 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách công suất")]
        public string SubBtsPowerSums { get; set; }

        [MaxLength(150)]
        [Display(Name = "Danh sách băng tần của mỗi BTS")]
        [StringLength(150, ErrorMessage = "Danh sách băng tần không quá 150 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách băng tần")]
        public string SubBtsBands { get; set; }

        [MaxLength(150)]
        [Display(Name = "Danh sách độ cao Anten của mỗi BTS")]
        [StringLength(150, ErrorMessage = "Danh sách độ cao Anten không quá 150 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách độ cao Anten")]
        public string SubBtsAntenHeights { get; set; }

        [Display(Name = "Có giới hạn an toàn")]
        public bool IsSafeLimit { get; set; }

        [Display(Name = "Độ cao giới hạn an toàn")]
        [DisplayFormat(DataFormatString = "{0:n5}", ApplyFormatInEditMode = true)]        
        [Range(0, double.MaxValue, ErrorMessage = "Yêu cầu nhập Độ cao là số >= 0")]
        public double SafeLimitHeight { get; set; }

        [Display(Name = "Có công trình trong phạm vi 100m")]
        public bool IsHouseIn100m { get; set; }

        [Display(Name = "Chênh lệch độ cao")]
        [DisplayFormat(DataFormatString = "{0:n5}", ApplyFormatInEditMode = true)]
        public double OffsetHeight { get; set; }

        public virtual ProfileViewModel Profile { get; set; }

        public virtual OperatorViewModel Operator { get; set; }

        public string ApplicantName { get; set; }

        public string OperatorName { get; set; }

        public string SubBtsCode1 { get; set; }
        public string SubBtsOperatorID1 { get; set; }        
        public string SubBtsEquipment1 { get; set; }
        public string SubBtsAntenNum1 { get; set; }
        public string SubBtsConfiguration1 { get; set; }
        public string SubBtsPowerSum1 { get; set; }
        public string SubBtsBand1 { get; set; }
        public string SubBtsAntenHeight1 { get; set; }

        public string SubBtsCode2 { get; set; }
        public string SubBtsOperatorID2 { get; set; }
        public string SubBtsEquipment2 { get; set; }
        public string SubBtsAntenNum2 { get; set; }
        public string SubBtsConfiguration2 { get; set; }
        public string SubBtsPowerSum2 { get; set; }
        public string SubBtsBand2 { get; set; }
        public string SubBtsAntenHeight2 { get; set; }

        public string SubBtsCode3 { get; set; }
        public string SubBtsOperatorID3 { get; set; }
        public string SubBtsEquipment3 { get; set; }
        public string SubBtsAntenNum3 { get; set; }
        public string SubBtsConfiguration3 { get; set; }
        public string SubBtsPowerSum3 { get; set; }
        public string SubBtsBand3 { get; set; }
        public string SubBtsAntenHeight3 { get; set; }

        public string SubBtsCode4 { get; set; }
        public string SubBtsOperatorID4 { get; set; }
        public string SubBtsEquipment4 { get; set; }
        public string SubBtsAntenNum4 { get; set; }
        public string SubBtsConfiguration4 { get; set; }
        public string SubBtsPowerSum4 { get; set; }
        public string SubBtsBand4 { get; set; }
        public string SubBtsAntenHeight4 { get; set; }

        public string SubBtsCode5 { get; set; }
        public string SubBtsOperatorID5 { get; set; }
        public string SubBtsEquipment5 { get; set; }
        public string SubBtsAntenNum5 { get; set; }
        public string SubBtsConfiguration5 { get; set; }
        public string SubBtsPowerSum5 { get; set; }
        public string SubBtsBand5 { get; set; }
        public string SubBtsAntenHeight5 { get; set; }

        public PrintCertificateViewModel()
        {
        }
    }
}