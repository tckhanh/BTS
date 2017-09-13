using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class OperatorViewModel
    {
        [Required]
        public string ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public string Telephone { get; set; }

        public string Fax { get; set; }

        public virtual ICollection<ApplicantViewModel> Applicants { get; set; }

        public virtual ICollection<BTSCertificateViewModel> BTSCertificates { get; set; }

        public virtual ICollection<SubBTSViewModel> SubBTSs { get; set; }
    }
}