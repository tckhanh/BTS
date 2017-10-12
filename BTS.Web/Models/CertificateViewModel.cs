using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class CertificateViewModel
    {
        public double? Longtitude { get; set; }

        public double? Latitude { get; set; }

        public string Address { get; set; }

        public string CityID { get; set; }

        public int? SubBTSNum { get; set; }

        public string InCaseOf { get; set; }

        public string ReportNum { get; set; }

        public DateTime? ReportDate { get; set; }

        public string CertificateNum { get; set; }

        public double? SafeLimit { get; set; }

        public string IssuedPlace { get; set; }

        public DateTime? IssuedDate { get; set; }

        public DateTime? ExpiredDate { get; set; }

        public string Signer { get; set; }

        public string OperatorID { get; set; }        
    }
}