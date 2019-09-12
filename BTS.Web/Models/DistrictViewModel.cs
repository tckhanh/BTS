using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class DistrictViewModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string CityID { get; set; }

        public virtual ICollection<CertificateViewModel> BTSCertificates { get; set; }

        public virtual CityViewModel City { get; set; }
    }
}