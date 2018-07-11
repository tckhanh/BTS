using BTS.Model.Abstract;
using BTS.Model.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class AuditableViewModel : IAuditable
    {
        [DefaultDateTimeValue("Now")]
        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { set; get; }

        [Display(Name = "Người tạo")]
        [StringLength(256, ErrorMessage = "Người tạo không quá 256 ký tự")]
        public string CreatedBy { set; get; }

        [Display(Name = "Ngày cập nhật")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedDate { set; get; }

        [Display(Name = "Người cập nhật")]
        [StringLength(256, ErrorMessage = "Người cập nhật không quá 256 ký tự")]
        public string UpdatedBy { set; get; }
    }
}