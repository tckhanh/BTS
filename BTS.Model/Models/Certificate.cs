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
        [StringLength(16)]
        public string Id { get; set; }

        public string ProfileID { get; set; }

        [Required]
        [StringLength(10)]
        public string OperatorID { get; set; }

        [Required]
        [StringLength(100)]
        public string BtsCode { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(3)]
        public string CityID { get; set; }

        public double? Longtitude { get; set; }

        public double? Latitude { get; set; }

        public int InCaseOfID { get; set; }

        [StringLength(20)]
        public string LabID { get; set; }

        [StringLength(30)]
        public string TestReportNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime TestReportDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime IssuedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ExpiredDate { get; set; }

        [StringLength(30)]
        [Required]
        public string IssuedPlace { get; set; }

        [StringLength(30)]
        [Required]
        public string Signer { get; set; }

        public int SubBtsQuantity { get; set; }

        [StringLength(255)]
        [Required]
        public string SubBtsCodes { get; set; }

        [StringLength(150)]
        [Required]
        public string SubBtsOperatorIDs { get; set; }

        [StringLength(255)]
        [Required]
        public string SubBtsEquipments { get; set; }

        [StringLength(150)]
        [Required]
        public string SubBtsAntenNums { get; set; }

        [StringLength(50)]
        public string SharedAntens { get; set; }

        [StringLength(150)]
        [Required]
        public string SubBtsConfigurations { get; set; }

        [StringLength(150)]
        [Required]
        public string SubBtsPowerSums { get; set; }

        [StringLength(150)]
        [Required]
        public string SubBtsBands { get; set; }

        [StringLength(150)]
        [Required]
        public string SubBtsAntenHeights { get; set; }

        public bool IsPoleOnGround { get; set; }

        public bool IsSafeLimit { get; set; }        
        
        public double? SafeLimitHeight { get; set; }

        public bool IsHouseIn100m { get; set; }

        public double? MaxHeightIn100m { get; set; }

        public double? MaxPowerSum { get; set; }

        public bool IsMeasuringExposure = false;

        public bool GetIsMeasuringExposure()
        {
            return IsMeasuringExposure;
        }

        public void SetIsMeasuringExposure(bool value)
        {
            IsMeasuringExposure = value;
        }

        public double? MinAntenHeight { get; set; }

        public double? OffsetHeight { get; set; }

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
    }
}