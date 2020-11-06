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
    public class NoRequiredBtsVM
    {
        [Display(Name = "Số nhận dạng Bts")]
        [StringLength(36, ErrorMessage = "Số nhận dạng Bts không quá 36 ký tự")]
        public string Id { get; set; }

        [Display(Name = "Mã nhà mạng")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập mã nhà mạng")]
        [StringLength(20, ErrorMessage = "Mã nhà mạng không quá 10 ký tự")]
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
        [DisplayFormat(DataFormatString = "{0:n5}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Kinh độ")]
        [Range(0, double.MaxValue, ErrorMessage = "Yêu cầu nhập Kinh độ là số >= 0")]
        public double? Longtitude { get; set; }

        [Display(Name = "Vĩ độ")]
        [DisplayFormat(DataFormatString = "{0:n5}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Vĩ độ")]
        [Range(0, double.MaxValue, ErrorMessage = "Yêu cầu nhập Vĩ độ là số >= 0")]
        public double? Latitude { get; set; }

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
        [StringLength(512, ErrorMessage = "Danh sách thiết bị không quá 512 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách thiết bị")]
        [DataType(DataType.MultilineText)]
        public string SubBtsEquipments { get; set; }

        [Display(Name = "Danh sách số Anten của mỗi BTS")]
        [StringLength(150, ErrorMessage = "Danh sách số Anten không quá 150 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách số Anten")]
        public string SubBtsAntenNums { get; set; }

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

        [MaxLength(256)]
        [Display(Name = "Danh sách băng tần của mỗi BTS")]
        [StringLength(256, ErrorMessage = "Danh sách băng tần không quá 256 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách băng tần")]
        public string SubBtsBands { get; set; }

        [MaxLength(150)]
        [Display(Name = "Danh sách độ cao Anten của mỗi BTS")]
        [StringLength(150, ErrorMessage = "Danh sách độ cao Anten không quá 150 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Danh sách độ cao Anten")]
        public string SubBtsAntenHeights { get; set; }

        [Display(Name = "Ngày công bố")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? AnnouncedDate { get; set; }

        [Display(Name = "Văn bản công bố")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Văn bản công bố")]
        [StringLength(50, ErrorMessage = "Văn bản công bố không quá 50 ký tự")]
        public string AnnouncedDoc { get; set; }

        [Display(Name = "Trạm BTS công bố bị hủy bỏ")]
        public bool IsCanceled { get; set; }

        [Display(Name = "Ngày hủy bỏ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime? CanceledDate { get; set; }

        [Display(Name = "Lý do bị hủy bỏ")]
        [StringLength(150, ErrorMessage = "Lý do bị hủy bỏ công bố")]
        public string CanceledReason { get; set; }


        [Display(Name = "Thuộc nhà mạng")]
        public ICollection<SelectListItem> OperatorList { get; set; }

        [Display(Name = "Thuộc Tỉnh/ Thành phố")]
        public ICollection<SelectListItem> CityList { get; set; }

        public virtual OperatorViewModel Operator { get; set; }

        public virtual CityViewModel City { get; set; }

        public virtual IEnumerable<SubBtsInNoRequiredBtsVM> SubBtsInNoRequiredBtss { get; set; }

        public NoRequiredBtsVM()
        {
            Id = Guid.NewGuid().ToString();
            OperatorList = new List<SelectListItem>();
            CityList = new List<SelectListItem>();
        }
    }
}