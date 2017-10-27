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
        public string IssuedPlace { get; set; }

        [MaxLength(30)]
        public string Signer { get; set; }

        public int SubBtsQuantity { get; set; }

        public int MinAntenHeight { get; set; }

        public int MaxHeightIn100m { get; set; }

        public int OffsetHeight { get; set; }

        public double? SafeLimit { get; set; }

        public virtual IEnumerable<SubBtsInCertViewModel> SubBTSinCerts { get; set; }
    }
}