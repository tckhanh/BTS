using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class ProfileViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Mã đơn vị")]
        [StringLength(50)]
        public string ApplicantID { get; set; }

        [Display(Name = "Số đơn kiểm định")]
        [StringLength(30)]
        public string ProfileNum { get; set; }

        [Display(Name = "Ngày đơn kiểm định")]
        public DateTime ProfileDate { get; set; }

        [Display(Name = "Số trạm BTS đề nghị kiểm định")]
        public int? BtsQuantity { get; set; }

        [Display(Name = "Ngày nhận đơn")]
        [DataType(DataType.Date)]
        public DateTime ApplyDate { get; set; }

        [Display(Name = "Phí Kiểm định")]
        public int? Fee { get; set; }

        [Display(Name = "Số CV thông báo Phí")]
        [StringLength(30)]
        public string FeeAnnounceNum { get; set; }

        [Display(Name = "Ngày CV thông báo Phí")]
        [DataType(DataType.Date)]
        public DateTime? FeeAnnounceDate { get; set; }

        [Display(Name = "Ngày nộp Phí")]
        [DataType(DataType.Date)]
        public DateTime? FeeReceiptDate { get; set; }

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { set; get; }

        [Display(Name = "Người tạo")]
        [StringLength(256, ErrorMessage = "Người tạo không quá 256 ký tự")]
        public string CreatedBy { set; get; }

        [Display(Name = "Ngày cập nhật")]
        [DataType(DataType.Date)]
        public DateTime? UpdatedDate { set; get; }

        [Display(Name = "Người cập nhật")]
        [StringLength(256, ErrorMessage = "Người cập nhật không quá 256 ký tự")]
        public string UpdatedBy { set; get; }

        [Display(Name = "Ngày xóa")]
        [DataType(DataType.Date)]
        public DateTime? DeletedDate { set; get; }

        [Display(Name = "Người xóa")]
        [StringLength(256, ErrorMessage = "Người xóa không quá 256 ký tự")]
        public string DeletedBy { set; get; }

        public virtual Applicant Applicant { get; set; }

        public virtual IEnumerable<Bts> Btss { get; set; }

        public virtual IEnumerable<Certificate> Certificates { get; set; }
    }
}