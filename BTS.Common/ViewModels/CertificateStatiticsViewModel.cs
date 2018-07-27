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

    public class StatBtsInProcessVm
    {
        public string OperatorID { get; set; }
        public int Btss { get; set; }
    }

    public class StatBtsInReceiptVm
    {
        public string OperatorID { get; set; }
        public int NoAnnounceFeeBtss { get; set; }
        public int NoReceiptFeeBtss { get; set; }
    }

    public class StatBtsVm
    {
        public string OperatorID { get; set; }
        public int ReceiptFeeBtss { get; set; }
        public int NoReceiptFeeBtss { get; set; }
        public int NoAnnounceFeeBtss { get; set; }
    }

    public class IssuedCertStatByOperatorYearVM
    {
        public string OperatorID { get; set; }
        public string Year { get; set; }
        public int IssuedCertificates { get; set; }
    }

    public class IssuedCertStatByOperatorCityVM
    {
        public string OperatorID { get; set; }
        public string CityID { get; set; }
        public int IssuedCertificates { get; set; }
    }

    public class ExpiredCertStatByOperatorYearVM
    {
        public string OperatorID { get; set; }
        public string Year { get; set; }
        public int ExpiredInYearCertificates { get; set; }
    }
}