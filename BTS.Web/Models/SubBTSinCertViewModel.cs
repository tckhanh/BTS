using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class SubBtsInCertViewModel
    {
        public int ID { get; set; }

        [MaxLength(16)]
        public string CertificateID { get; set; }

        [MaxLength(50)]
        public string BtsCode { get; set; }

        [MaxLength(10)]
        public string OperatorID { get; set; }

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
    }
}