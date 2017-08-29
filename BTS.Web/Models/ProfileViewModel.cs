using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class ProfileViewModel
    {
        public int ID { get; set; }

        public int? ApplicantID { get; set; }
        
        public string ProfileNum { get; set; }

        public DateTime? ProfileDate { get; set; }

        public DateTime? ApplyDate { get; set; }

        public int? BTSNum { get; set; }

        public string AdminNum { get; set; }

        public virtual ApplicantViewModel Applicant { get; set; }

        public virtual ICollection<BTSCertificateViewModel> BTSCertificates { get; set; }
    }
}