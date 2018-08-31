using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class CertificateViewModel
    {
        [Display(Name = "Số giấy CNKĐ")]
        [StringLength(16, ErrorMessage = "Số Giấy CNKĐ không quá 16 ký tự")]
        public string Id { get; set; }

        [Display(Name = "Mã số hồ sơ")]
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
        public string Address { get; set; }

        [Display(Name = "Mã Tỉnh/Thành phố")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập mã Tỉnh/Thành phố")]
        [StringLength(3, ErrorMessage = "Mã Tỉnh/Thành phố BTS không quá 3 ký tự")]
        public string CityID { get; set; }

        [Display(Name = "Kinh độ")]
        public double? Longtitude { get; set; }

        [Display(Name = "Vĩ độ")]
        public double? Latitude { get; set; }

        [Display(Name = "TH Kiểm định")]
        public int InCaseOfID { get; set; }

        [StringLength(20, ErrorMessage = "Mã phòng đo kiểm không quá 20 ký tự")]
        [Display(Name = "Mã Phòng đo kiểm")]
        public string LabID { get; set; }

        [StringLength(30, ErrorMessage = "Số KQ đo kiểm không quá 30 ký tự")]
        [Display(Name = "Số KQ đo kiểm")]
        public string TestReportNo { get; set; }

        [Display(Name = "Ngày KQ đo kiểm")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TestReportDate { get; set; }

        [Display(Name = "Ngày ban hành")]
        [DataType(DataType.Date)]
        public DateTime? IssuedDate { get; set; }

        [Display(Name = "Ngày hết hạn")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiredDate { get; set; }

        [StringLength(30, ErrorMessage = "Nơi cấp không quá 30 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Nơi cấp")]
        [Display(Name = "Nơi cấp")]
        public string IssuedPlace { get; set; }

        [StringLength(30, ErrorMessage = "Nơi cấp không quá 30 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu Người ký tên")]
        [Display(Name = "Người ký tên")]
        public string Signer { get; set; }

        [Display(Name = "Số trạm BTS")]
        public int SubBtsQuantity { get; set; }

        [Display(Name = "Danh sách mã trạm BTS")]
        [StringLength(255, ErrorMessage = "Danh sách mã trạm BTS không quá 255 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách mã trạm BTS")]
        public string SubBtsCodes { get; set; }

        [Display(Name = "Danh sách mã nhà mạng")]
        [StringLength(150, ErrorMessage = "Danh sách mã nhà mạng không quá 150 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách mã nhà mạng")]
        public string SubBtsOperatorIDs { get; set; }

        [Display(Name = "Danh sách thiết bị")]
        [StringLength(255, ErrorMessage = "Danh sách thiết bị không quá 255 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách thiết bị")]
        public string SubBtsEquipments { get; set; }

        [Display(Name = "Danh sách số Anten")]
        [StringLength(150, ErrorMessage = "Danh sách số Anten không quá 150 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách số Anten")]
        public string SubBtsAntenNums { get; set; }

        [Display(Name = "Các Anten dùng chung")]
        [StringLength(50, ErrorMessage = "Các Anten dùng chung không quá 150 ký tự")]
        public string SharedAntens { get; set; }

        [MaxLength(150)]
        [Display(Name = "Danh sách cấu hình")]
        [StringLength(150, ErrorMessage = "Danh sách cấu hình không quá 150 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách cấu hình")]
        public string SubBtsConfigurations { get; set; }

        [MaxLength(150)]
        [Display(Name = "Danh sách công suất")]
        [StringLength(150, ErrorMessage = "Danh sách công suất không quá 150 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách công suất")]
        public string SubBtsPowerSums { get; set; }

        [MaxLength(150)]
        [Display(Name = "Danh sách băng tần")]
        [StringLength(150, ErrorMessage = "Danh sách băng tần không quá 150 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách băng tần")]
        public string SubBtsBands { get; set; }

        [MaxLength(150)]
        [Display(Name = "Danh sách độ cao Anten")]
        [StringLength(150, ErrorMessage = "Danh sách độ cao Anten không quá 150 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách độ cao Anten")]
        public string SubBtsAntenHeights { get; set; }

        [Display(Name = "Độ cao Anten thấp nhất")]
        public double? MinAntenHeight { get; set; }

        [Display(Name = "Độ cao công trình cao nhất trong phạm vi 100m")]
        public double? MaxHeightIn100m { get; set; }

        [Display(Name = "Chênh lệch độ cao")]
        public double? OffsetHeight { get; set; }

        [Display(Name = "Có đo phơi nhiễm")]
        public bool MeasuringExposure { get; set; } = false;

        [Display(Name = "Có giới hạn an toàn")]
        public double? SafeLimit { get; set; }

        public virtual ProfileViewModel Profile { get; set; }

        public virtual OperatorViewModel Operator { get; set; }

        public virtual CityViewModel City { get; set; }

        public virtual InCaseOfViewModel InCaseOf { get; set; }

        public virtual Lab LabViewModel { get; set; }
        public virtual IEnumerable<SubBtsInCertViewModel> SubBtsInCerts { get; set; }

        public ICollection<SubBtsInCertViewModel> SubBtsList { get; set; }

        public CertificateViewModel()
        {
            SubBtsList = new List<SubBtsInCertViewModel>();
        }
    }
}