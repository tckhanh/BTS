using BTS.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<SlideViewModel> Slides { set; get; }
        public IEnumerable<StatisticCertificateByYear> StatisticCertificateByYear { set; get; }
        public IEnumerable<StatisticCertificateByOperatorCity> StatisticCertificateByOperatorCity { set; get; }        
    }
}