using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class SubBTSViewModel
    {
        public int ID { get; set; }

        public string OperatorID { get; set; }

        public int? BTSCertificateID { get; set; }

        public string BTSCode { get; set; }

        public string Equipment { get; set; }

        public int? AntenNum { get; set; }

        public string Configuration { get; set; }

        public string PowerSum { get; set; }

        public string Band { get; set; }

        public string HeightAnten { get; set; }

        public bool? Status { get; set; }

        public virtual BTSCertificateViewModel BTSCertificate { get; set; }

        public virtual OperatorViewModel Operator { get; set; }
    }
}