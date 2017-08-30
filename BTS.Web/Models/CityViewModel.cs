using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class CityViewModel
    {
        public string ID { get; set; }
        
        public string Name { get; set; }

        public virtual ICollection<BTSCertificateViewModel> BTSCertificates { get; set; }

        public virtual ICollection<DistrictViewModel> Districts { get; set; }
    }
}