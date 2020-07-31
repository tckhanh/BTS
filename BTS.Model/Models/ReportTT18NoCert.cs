using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Model.Models
{
    public class ReportTT18NoCert
    {
        // Certificate Field
        public string Id { get; set; }

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

        public int SubBtsQuantity { get; set; }

        // SubBtsInCert Field

        [MaxLength(50)]
        public string SubBtsCode { get; set; }

        [MaxLength(20)]
        public string SubOperatorID { get; set; }

        [MaxLength(50)]
        public string Manufactory { get; set; }

        [MaxLength(50)]
        public string Equipment { get; set; }

        public int? AntenNum { get; set; }

        [MaxLength(30)]
        public string Configuration { get; set; }

        [MaxLength(30)]
        public string PowerSum { get; set; }

        [MaxLength(30)]
        public string Band { get; set; }

        [MaxLength(30)]
        public string AntenHeight { get; set; }

        [MaxLength(20)]
        public string LabID { get; set; }

        [MaxLength(30)]
        public string TestReportNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime TestReportDate { get; set; }

        [MaxLength(255)]
        [Required]
        public string ReasonNoCertificate { get; set; }

    }
}
