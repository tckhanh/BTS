using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class ApplicantViewModel
    {
        [StringLength(50)]
        [Display(Name = "Mã Đơn vị")]
        public string ID { get; set; }

        [Display(Name = "Mã nhà mạng")]
        public string OperatorID { get; set; }

        [Display(Name = "Tên nhà mạng")]
        public string Name { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Số điện thoại")]
        public string Telephone { get; set; }

        [Display(Name = "Số Fax")]
        public string Fax { get; set; }

        public virtual OperatorViewModel Operator { get; set; }

        public virtual ICollection<ProfileViewModel> Profiles { get; set; }
    }
}