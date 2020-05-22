using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Models
{
    public class ProfileViewModel : AuditableViewModel
    {
        [Display(Name = "Mã hồ sơ")]
        [StringLength(36, ErrorMessage = "Mã hồ sơ không quá 36 ký tự")]
        public string Id { get; set; }

        [Display(Name = "Mã đơn vị")]
        [StringLength(50, ErrorMessage = "Mã đơn vị không quá 50 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Mã đơn vị nộp hồ sơ")]
        public string ApplicantID { get; set; }

        [Display(Name = "Số Đơn KĐ")]
        [StringLength(30, ErrorMessage = "Số Đơn KĐ không quá 30 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Số Đơn KĐ")]
        public string ProfileNum { get; set; }

        [Display(Name = "Ngày Đơn KĐ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Ngày Đơn KĐ")]
        public DateTime ProfileDate { get; set; }

        [Display(Name = "Số BTS nộp")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Số Trạm BTS nộp")]
        [Range(1, int.MaxValue, ErrorMessage = "Yêu cầu nhập Phí Kiểm định là số nguyên trong phạm vi [1->2147483647]")]
        [RegularExpression(@"[1-9][0-9]*$", ErrorMessage = "Yêu cầu nhập Số Trạm BTS nộp là số nguyên")]
        public int BtsQuantity { get; set; }

        [Display(Name = "Số BTS tiếp nhận")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Số BTS tiếp nhận")]
        [Range(1, int.MaxValue, ErrorMessage = "Yêu cầu nhập Phí Kiểm định là số nguyên trong phạm vi [1->2147483647]")]
        [RegularExpression(@"[1-9][0-9]*$", ErrorMessage = "Yêu cầu nhập Số Trạm BTS tiếp nhận là số nguyên")]
        public int AcceptedBtsQuantity { get; set; }

        [Display(Name = "Ngày nộp Đơn KĐ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Ngày nộp Đơn KĐ")]
        public DateTime ApplyDate { get; set; }

        [Display(Name = "Phí Kiểm định")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Yêu cầu nhập Số Phí Kiểm định")]
        [Range(1, long.MaxValue, ErrorMessage = "Yêu cầu nhập Phí Kiểm định là số nguyên trong phạm vi [1->2147483647]")]
        [RegularExpression(@"[1-9][0-9]*$", ErrorMessage = "Yêu cầu nhập Số Trạm BTS tiếp nhận là số nguyên")]
        //Ap dung cho so Double
        //[RegularExpression(@"^\d+.\d{0,}$", ErrorMessage = "Yêu cầu nhập Số Phí Kiểm định là số nguyên")]
        public long Fee { get; set; }

        [Display(Name = "Số CV Báo phí")]
        [StringLength(30, ErrorMessage = "Số CV Báo phí không quá 30 ký tự")]
        public string FeeAnnounceNum { get; set; }

        [Display(Name = "Ngày Báo phí")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FeeAnnounceDate { get; set; }

        [Display(Name = "Ngày nộp phí")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FeeReceiptDate { get; set; }

        [Display(Name = "Danh sách các Đơn vị")]
        public ICollection<SelectListItem> ApplicantList { get; set; }

        public virtual Applicant Applicant { get; set; }

        public virtual IEnumerable<Bts> Btss { get; set; }

        public virtual IEnumerable<Certificate> Certificates { get; set; }

        public ProfileViewModel()
        {
            Id = Guid.NewGuid().ToString();
            ApplicantList = new List<SelectListItem>();
            ProfileDate = DateTime.Now;
            ApplyDate = DateTime.Now;
        }
    }
}