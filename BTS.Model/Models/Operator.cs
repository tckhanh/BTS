using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("Operators")]
    public class Operator
    {
        [Key]        
        [StringLength(10)]
        public string ID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Telephone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        public virtual ICollection<Applicant> Applicants { get; set; }

        public virtual ICollection<BTSCertificate> BTSCertificates { get; set; }

        public virtual ICollection<SubBTS> SubBTSs { get; set; }
    }
}