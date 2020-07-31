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
        public string Id { get; set; }

        [MaxLength(16)]
        public string CertificateID { get; set; }

        public int BtsSerialNo { get; set; }

        [MaxLength(50)]
        public string BtsCode { get; set; }

        [MaxLength(20)]
        public string OperatorID { get; set; }

        [MaxLength(50)]
        public string Manufactory { get; set; }

        [MaxLength(50)]
        public string Equipment { get; set; }

        public int AntenNum { get; set; }

        [MaxLength(30)]
        public string Configuration { get; set; }

        [MaxLength(30)]
        public string PowerSum { get; set; }

        [MaxLength(30)]
        public string Band { get; set; }

        [MaxLength(30)]
        public string AntenHeight { get; set; }

        [ForeignKey("CertificateID")]
        public virtual Certificate Certificates { get; set; }

        [ForeignKey("OperatorID")]
        public virtual Operator Operator { get; set; }

        public SubBtsInCert()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}