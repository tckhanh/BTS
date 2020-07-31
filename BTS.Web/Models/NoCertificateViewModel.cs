using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Models
{
    public class NoCertificateViewModel
    {
        [Display(Name = "Mã số")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Mã số")]
        public string Id { get; set; }

        [Display(Name = "Mã số hồ sơ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập mã số hồ sơ")]
        [StringLength(36, ErrorMessage = "Mã hồ sơ không quá 36 ký tự")]
        public string ProfileID { get; set; }

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
        //[RegularExpression(@"^\d+.\d{0,}$", ErrorMessage = "Yêu cầu nhập Kinh độ là số ")]
        public double? Longtitude { get; set; }

        [Display(Name = "Vĩ độ")]
        [DisplayFormat(DataFormatString = "{0:n5}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Vĩ độ")]
        [Range(0, double.MaxValue, ErrorMessage = "Yêu cầu nhập Vĩ độ là số >= 0")]
        public double? Latitude { get; set; }

        [Display(Name = "TH Kiểm định")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập TH Kiểm định")]
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

        [Display(Name = "Lý do không cấp G.CNKĐ")]
        [StringLength(255, ErrorMessage = "Lý do không cấp không quá 255 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu lý do không cấp G. CNKĐ")]
        [DataType(DataType.MultilineText)]
        public string ReasonNoCertificate { get; set; }

        [Display(Name = "Thuộc hồ sơ KĐ")]
        public ICollection<SelectListItem> ProfileList { get; set; }

        [Display(Name = "Thuộc nhà mạng")]
        public ICollection<SelectListItem> OperatorList { get; set; }

        [Display(Name = "Thuộc Tỉnh/ Thành phố")]
        public ICollection<SelectListItem> CityList { get; set; }

        [Display(Name = "Trường hợp Kiểm định")]
        public ICollection<SelectListItem> InCaseOfList { get; set; }

        [Display(Name = "Đơn vị đo KĐ")]
        public ICollection<SelectListItem> LabList { get; set; }

        public virtual Profile Profile { get; set; }

        public virtual Operator Operator { get; set; }

        public virtual City City { get; set; }

        public virtual InCaseOf InCaseOf { get; set; }

        public virtual Lab Lab { get; set; }

        public NoCertificateViewModel()
        {
            Id = Guid.NewGuid().ToString();
            TestReportDate = DateTime.Now;
            ProfileList = new List<SelectListItem>();
            OperatorList = new List<SelectListItem>();
            CityList = new List<SelectListItem>();
            InCaseOfList = new List<SelectListItem>();
            LabList = new List<SelectListItem>();
            
        }
    }
}