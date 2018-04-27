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
        public int Code { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime? CreatedDate { set; get; }

        [StringLength(256)]
        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        [StringLength(256)]
        public string UpdatedBy { set; get; }

        public DateTime? DeletedDate { set; get; }

        [StringLength(256)]
        public string DeletedBy { set; get; }

        public virtual IEnumerable<BtsViewModel> Btss { get; set; }

        public virtual IEnumerable<CertificateViewModel> Certificates { get; set; }
    }
}