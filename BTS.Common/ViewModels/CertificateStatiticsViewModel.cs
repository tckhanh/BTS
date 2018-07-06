using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Common.ViewModels
{
    public class CertStatVM
    {
        public string Year { get; set; }
        public string OperatorID { get; set; }
        public string CityID { get; set; }
        public int InCaseOfID { get; set; }
        public string LabID { get; set; }
        public int ValidCertificates { get; set; }
        public int ExpiredInYearCertificates { get; set; }
    }

    public class CertStatByOperatorVM
    {
        public string OperatorID { get; set; }
        public int ValidCertificates { get; set; }
        public int ExpiredInYearCertificates { get; set; }
    }

    public class IssuedCertStatByOperatorYearVM
    {
        public string OperatorID { get; set; }
        public string Year { get; set; }
        public int IssuedCertificates { get; set; }
    }

    public class ExpiredCertStatByOperatorYearVM
    {
        public string OperatorID { get; set; }
        public string Year { get; set; }
        public int ExpiredInYearCertificates { get; set; }
    }
}