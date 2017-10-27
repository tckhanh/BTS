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

        [StringLength(50)]
        public string ApplicantID { get; set; }

        [StringLength(30)]
        public string ProfileNum { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ProfileDate { get; set; }

        public int? BtsQuantity { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ApplyDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValidDate { get; set; }        

        public int? Fee { get; set; }

        [StringLength(30)]
        public string FeeAnnounceNum { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FeeAnnounceDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FeeReceiptDate { get; set; }

        [ForeignKey("ApplicantID")]
        public virtual Applicant Applicant { get; set; }
        
        public virtual IEnumerable<Bts> Btss { get; set; }

        public virtual IEnumerable<Certificate> Certificates { get; set; }
    }
}