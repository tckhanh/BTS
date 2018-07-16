using BTS.Model.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("SubBtsInCerts")]
    public class SubBtsInCert : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(16)]
        public string CertificateID { get; set; }

        [StringLength(50)]
        public string BtsCode { get; set; }

        [StringLength(10)]
        public string OperatorID { get; set; }

        [StringLength(50)]
        public string Manufactory { get; set; }

        [StringLength(50)]
        public string Equipment { get; set; }

        public int? AntenNum { get; set; }

        [StringLength(30)]
        public string Configuration { get; set; }

        [StringLength(30)]
        public string PowerSum { get; set; }

        [StringLength(30)]
        public string Band { get; set; }

        [StringLength(30)]
        public string AntenHeight { get; set; }

        [ForeignKey("CertificateID")]
        public virtual Certificate Certificates { get; set; }

        [ForeignKey("OperatorID")]
        public virtual Operator Operator { get; set; }
    }
}