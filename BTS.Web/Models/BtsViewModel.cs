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
        public int ID { get; set; }

        [Display(Name = "Mã số hồ sơ")]
        public int ProfileID { get; set; }

        [Display(Name = "Mã nhà mạng")]
        [Required]
        [StringLength(10)]
        public string OperatorID { get; set; }

        [Display(Name = "Mã trạm BTS")]
        [Required]
        [StringLength(100)]
        public string BtsCode { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Display(Name = "Mã Tỉnh/Thành phố")]
        [Required]
        [StringLength(3)]
        public string CityID { get; set; }

        [Display(Name = "Kinh độ")]
        public double? Longtitude { get; set; }

        [Display(Name = "Vĩ độ")]
        public double? Latitude { get; set; }

        [Display(Name = "TH Kiểm định")]
        public int InCaseOfID { get; set; }

        [Display(Name = "Giấy CNKĐ")]
        [StringLength(16)]
        public string IssuedCertificateID { get; set; }

        [Display(Name = "Giấy CNKĐ đã cấp trước đó")]
        [StringLength(16)]
        public string LastOwnCertificateID { get; set; }

        [Display(Name = "Nhà mạng được cấp Giấy CNKĐ trước đó")]
        [StringLength(10)]
        public string LastOwnOperatorID { get; set; }

        [Display(Name = "Giấy CNKĐ đã cấp cùng trước đó")]
        [StringLength(16)]
        public string LastNoOwnCertificateID { get; set; }

        [Display(Name = "Nhà mạng được cấp cùng Giấy CNKĐ trước đó")]
        [StringLength(10)]
        public string LastNoOwnOperatorID { get; set; }

        public virtual ProfileViewModel Profile { get; set; }

        public virtual OperatorViewModel Operator { get; set; }

        public virtual CityViewModel City { get; set; }

        public virtual InCaseOfViewModel InCaseOf { get; set; }
    }
}