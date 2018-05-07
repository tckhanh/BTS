using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class ApplicationRoleViewModel
    {
        public string ID { set; get; }
        [Display(Name = "Tên Quyền")]
        public string Name { set; get; }
        [Display(Name = "Mô tả")]
        [MaxLength(250)]
        public string Description { set; get; }
    }
}