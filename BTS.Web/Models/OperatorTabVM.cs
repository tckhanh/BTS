using BTS.Model.Models;
using BTS.Web.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class OperatorTabVM
    {
        [Display(Name = "Số thứ tự")]
        public int No { get; set; }

        [Display(Name = "Mã tên nhà mạng")]
        [Required(ErrorMessage = "Yêu cầu nhập Mã tên nhà mạng")]
        [MaxLength(10, ErrorMessage = "Mã tên nhà mạng tối đa 10 ký tự")]
        public string Id { get; set; }

        [Display(Name = "Thuộc mã nhà mạng")]
        [Required(ErrorMessage = "Yêu cầu nhập Mã nhà mạng")]
        [MaxLength(10, ErrorMessage = "Mã nhà mạng tối đa 10 ký tự")]
        public string RootId { get; set; }

        [Display(Name = "Tên Nhà mạng")]
        [Required(ErrorMessage = "Yêu cầu nhập Tên Nhà mạng")]
        [MaxLength(255, ErrorMessage = "Tên Nhà mạng tối đa 255 ký tự")]
        [DataType(DataType.MultilineText)]
        [Unique(ErrorMessage = "Tên Nhà mạng đã tồn tại rồi !!", TargetModelType = typeof(Operator), TargetPropertyName = "Name")]
        public string Name { get; set; }
    }
}