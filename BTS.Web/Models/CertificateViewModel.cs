using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class CertificateViewModel
    {
        public string ID { get; set; }

        public int? ProfileID { get; set; }

        [Required]
        [MaxLength(10)]
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

        public DateTime TestReportDate { get; set; }

        public DateTime? IssuedDate { get; set; }

        public DateTime? ExpiredDate { get; set; }

        [MaxLength(30)]
        [Required]
        public string IssuedPlace { get; set; }

        [MaxLength(30)]
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

        [MaxLength(150)]
        [Required]
        public string SubBtsConfigurations { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsPowerSums { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsBands { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsAntenHeights { get; set; }

        public double? MinAntenHeight { get; set; }

        public double? MaxHeightIn100m { get; set; }

        public double? OffsetHeight { get; set; }

        public double? SafeLimit { get; set; }

        public virtual IEnumerable<SubBtsInCertViewModel> SubBtsInCerts { get; set; }
    }
}