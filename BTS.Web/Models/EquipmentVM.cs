using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class EquipmentVM
    {
        [Display(Name = "Mã số thiết bị")]
        public int Id { set; get; }

        [Display(Name = "Tên thiết bị")]
        [StringLength(100, ErrorMessage = "Tên thiết bị không quá 100 ký tự")]
        public string Name { get; set; }

        [Display(Name = "Số máy phát")]
        public int Tx { get; set; }

        [Display(Name = "Băng tần")]
        [StringLength(30, ErrorMessage = "Băng tần không quá 30 ký tự")]
        public string Band { get; set; }

        [Display(Name = "Thuộc mã nhà mạng")]
        [Required(ErrorMessage = "Yêu cầu nhập Mã nhà mạng")]
        [MaxLength(10, ErrorMessage = "Mã nhà mạng tối đa 10 ký tự")]
        public string OperatorRootID { get; set; }

        [Display(Name = "Công suất máy phát (W)")]
        public double MaxPower { get; set; }

    }
}