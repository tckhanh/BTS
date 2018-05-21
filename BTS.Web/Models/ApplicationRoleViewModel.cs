using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class ApplicationRoleViewModel
    {
        [Display(Name = "Mã Quyền")]
        [Required]
        public string ID { set; get; }

        [Display(Name = "Tên Quyền")]
        public string Name { set; get; }

        [Display(Name = "Mô tả quyền")]
        [MaxLength(250, ErrorMessage = "Mô tả quyền không quá 250 ký tự")]
        public string Description { set; get; }
    }
}