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
        public IEnumerable<BTS.Common.ViewModels.StatisticCertificate> StatisticCertificateByYear { set; get; }
        public IEnumerable<BTS.Common.ViewModels.StatisticCertificate> StatisticCertificateByOperatorCity { set; get; }
    }
}