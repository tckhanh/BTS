using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("Profiles")]
    public class Profile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? ApplicantID { get; set; }

        [StringLength(30)]
        public string ProfileNum { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ProfileDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ApplyDate { get; set; }

        public int? BTSNum { get; set; }

        [StringLength(30)]
        public string AdminNum { get; set; }

        [ForeignKey("ApplicantID")]
        public virtual Applicant Applicant { get; set; }
        
        public virtual ICollection<BTSCertificate> BTSCertificates { get; set; }
    }
}