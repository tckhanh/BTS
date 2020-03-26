using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class EquipmentTabVM
    {
        [Display(Name = "Số thứ tự")]
        public int No { get; set; }

        [Display(Name = "Tên thiết bị")]
        [StringLength(100, ErrorMessage = "Tên thiết bị không quá 100 ký tự")]
        public string Name { get; set; }

        [Display(Name = "Số máy phát")]
        public int Tx { get; set; }

        [Display(Name = "Băng tần")]
        [StringLength(30, ErrorMessage = "Băng tần không quá 30 ký tự")]
        public string Band { get; set; }

        [Display(Name = "MOBIFONE Công suất máy phát (W)")]
        public double MOBIFONE { get; set; }

        [Display(Name = "VIETTEL Công suất máy phát (W)")]
        public double VIETTEL { get; set; }

        [Display(Name = "VINAPHONE Công suất máy phát (W)")]
        public double VINAPHONE { get; set; }

        [Display(Name = "VNMOBILE Công suất máy phát (W)")]
        public double VNMOBILE { get; set; }

        [Display(Name = "GTEL Công suất máy phát (W)")]
        public double GTEL { get; set; }

    }
}