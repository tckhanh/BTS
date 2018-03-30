using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class InCaseOfViewModel
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual IEnumerable<BtsViewModel> Btss { get; set; }

        public virtual IEnumerable<CertificateViewModel> Certificates { get; set; }
    }
}