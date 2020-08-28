using BTS.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("Certificates")]
    public class Certificate : Auditable
    {
        [Key]
        [MaxLength(16)]
        public string Id { get; set; }

        [MaxLength(36)]
        public string ProfileID { get; set; }

        [Required]
        [MaxLength(20)]
        public string OperatorID { get; set; }

        [Required]
        [MaxLength(100)]
        public string BtsCode { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }

        [Required]
        [MaxLength(3)]
        public string CityID { get; set; }

        public double? Longtitude { get; set; }

        public double? Latitude { get; set; }

        public int InCaseOfID { get; set; }

        [MaxLength(20)]
        public string LabID { get; set; }

        [MaxLength(30)]
        public string TestReportNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime TestReportDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime IssuedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ExpiredDate { get; set; }

        [MaxLength(30)]
        [Required]
        public string IssuedPlace { get; set; }

        [MaxLength(50)]
        [Required]
        public string Signer { get; set; }

        public int SubBtsQuantity { get; set; }

        [MaxLength(255)]
        [Required]
        public string SubBtsCodes { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsOperatorIDs { get; set; }

        [MaxLength(255)]
        [Required]
        public string SubBtsEquipments { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsAntenNums { get; set; }

        [MaxLength(50)]
        public string SharedAntens { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsConfigurations { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsPowerSums { get; set; }

        [MaxLength(256)]
        [Required]
        public string SubBtsBands { get; set; }

        [MaxLength(256)]
        [Required]
        public string SubBtsBandsOld { get; set; }


        [MaxLength(150)]
        [Required]
        public string SubBtsAntenHeights { get; set; }

        public bool IsPoleOnGround { get; set; }

        public bool IsSafeLimit { get; set; }        
        
        public double? SafeLimitHeight { get; set; }

        public bool IsHouseIn100m { get; set; }

        public double? MaxHeightIn100m { get; set; }

        public double? MaxPowerSum { get; set; }

        public bool IsMeasuringExposure { get; set; }       

        public double? MinAntenHeight { get; set; }

        public double? OffsetHeight { get; set; }

        [MaxLength(50)]
        [Required]
        public string VerifyUnit { get; set; }

        [MaxLength(50)]
        [Required]
        public string SignerRole { get; set; }

        [MaxLength(50)]
        public string SignerSubRole { get; set; }

        [MaxLength(50)]
        public string Verifier1 { get; set; }

        [MaxLength(50)]
        public string Verifier2 { get; set; }

        //[MaxLength(150)]
        //public string SubBtsTechnologies { get; set; }

        public bool IsCanceled { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CanceledDate { get; set; }

        [MaxLength(150)]
        public string CanceledReason { get; set; }


        [ForeignKey("ProfileID")]
        public virtual Profile Profile { get; set; }

        [ForeignKey("OperatorID")]
        public virtual Operator Operator { get; set; }

        [ForeignKey("CityID")]
        public virtual City City { get; set; }

        [ForeignKey("InCaseOfID")]
        public virtual InCaseOf InCaseOf { get; set; }

        [ForeignKey("LabID")]
        public virtual Lab Lab { get; set; }

        public virtual IEnumerable<SubBtsInCert> SubBtsInCerts { get; set; }
        public Certificate()
        {            
        }
    }
}