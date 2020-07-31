using BTS.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("Operators")]
    public class Operator : Auditable
    {
        [Key]
        [MaxLength(20)]
        public string Id { get; set; }

        [MaxLength(20)]
        [Required]
        public string RootId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public virtual IEnumerable<Applicant> Applicants { get; set; }
        public virtual IEnumerable<Bts> Btss { get; set; }
        public virtual IEnumerable<Certificate> Certificates { get; set; }
        public virtual IEnumerable<SubBtsInCert> SubBtsInCerts { get; set; }

        public Operator()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}