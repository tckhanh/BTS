using BTS.Model.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("Applicants")]
    public class Applicant : Auditable
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(30)]
        public string Phone { get; set; }

        [StringLength(30)]
        public string Fax { get; set; }

        [StringLength(100)]
        public string ContactName { get; set; }

        [Required]
        [StringLength(10)]
        public string OperatorID { get; set; }

        [ForeignKey("OperatorID")]
        public virtual Operator Operator { get; set; }

        public virtual IEnumerable<Profile> Profiles { get; set; }
    }
}