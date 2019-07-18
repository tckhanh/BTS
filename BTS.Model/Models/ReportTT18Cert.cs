using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Model.Models
{
    public class ReportTT18Cert
    {
        // Certificate Field

        [StringLength(16)]
        public string CertificateId { get; set; }

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

        public int SubBtsQuantity { get; set; }

        // SubBtsInCert Field

        [StringLength(50)]
        public string SubBtsCode { get; set; }

        [StringLength(10)]
        public string SubOperatorID { get; set; }

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
    }
}
