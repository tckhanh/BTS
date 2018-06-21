using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class BtsViewModel
    {
        [Display(Name = "Mã")]
        public int Id { get; set; }

        [Display(Name = "Mã số hồ sơ")]
        public int ProfileID { get; set; }

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

        [Display(Name = "Giấy CNKĐ đã cấp trước đó")]
        [StringLength(255, ErrorMessage = "Giấy CNKĐ gồm 16 ký tự")]
        public string LastOwnCertificateIDs { get; set; }

        [Display(Name = "Giấy CNKĐ đã cấp cùng trước đó")]
        [StringLength(255, ErrorMessage = "Các Giấy CNKĐ không quá 255 ký tự")]
        public string LastNoOwnCertificateIDs { get; set; }

        [Display(Name = "Giấy Hồ sơ KĐ đang xử lý")]
        [StringLength(255, ErrorMessage = "Các Hồ sơ KĐ không quá 255 ký tự")]
        public string ProFilesInProcess { get; set; }

        [Display(Name = "Các lý do không cấp Giấy CNKĐ")]
        [StringLength(255, ErrorMessage = "Các lý do không cấp Giấy CNKĐ không quá 255 ký tự")]
        public string ReasonsNoCertificate { get; set; }

        public virtual ProfileViewModel Profile { get; set; }

        public virtual OperatorViewModel Operator { get; set; }

        public virtual CityViewModel City { get; set; }

        public virtual InCaseOfViewModel InCaseOf { get; set; }
    }
}