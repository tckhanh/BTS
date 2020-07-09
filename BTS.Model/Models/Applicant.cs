using BTS.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("Applicants")]
    public class Applicant : Auditable
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(30)]
        public string Phone { get; set; }

        [MaxLength(30)]
        public string Fax { get; set; }

        [MaxLength(100)]
        public string ContactName { get; set; }

        [Required]
        [MaxLength(10)]
        public string OperatorID { get; set; }

        [ForeignKey("OperatorID")]
        public virtual Operator Operator { get; set; }

        public virtual IEnumerable<Profile> Profiles { get; set; }

        public Applicant()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}