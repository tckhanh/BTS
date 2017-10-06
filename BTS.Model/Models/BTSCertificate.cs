using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("BTSCertificates")]
    public class BTSCertificate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? ProfileID { get; set; }

        public double? Longtitude { get; set; }

        public double? Latitude { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(3)]
        public string CityID { get; set; }

        public int? DistrictID { get; set; }

        public int? SubBTSNum { get; set; }

        [StringLength(10)]
        public string InCaseOf { get; set; }

        [StringLength(30)]
        public string ReportNum { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReportDate { get; set; }

        [StringLength(16)]
        public string CertificateNum { get; set; }

        public double? SafeLimit { get; set; }

        [StringLength(30)]
        public string IssuedPlace { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IssuedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiredDate { get; set; }

        [StringLength(30)]
        public string Signer { get; set; }

        [StringLength(10)]
        public string OperatorID { get; set; }

        [ForeignKey("CityID")]
        public virtual City City { get; set; }

        [ForeignKey("DistrictID")]
        public virtual District District { get; set; }

        [ForeignKey("OperatorID")]
        public virtual Operator Operator { get; set; }


        [ForeignKey("ProfileID")]
        public virtual Profile Profile { get; set; }

        public virtual IEnumerable<SubBTS> SubBTSs { get; set; }
    }
}