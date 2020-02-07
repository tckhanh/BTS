using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Common.ViewModels
{
    public class StatCerVM
    {
        public string Year { get; set; }
        public string OperatorID { get; set; }
        public string CityID { get; set; }
        public int InCaseOfID { get; set; }
        public string LabID { get; set; }
        public int ValidCertificates { get; set; }
        public int ExpiredInYearCertificates { get; set; }
    }

    public class StatCoupleCerByOperatorVM
    {
        public string OperatorID { get; set; }
        public int ValidCertificates { get; set; }
        public int ExpiredInYearCertificates { get; set; }
    }

    public class StatCoupleCerByAreaVM
    {
        public string Area { get; set; }
        public int ValidCertificates { get; set; }
        public int ExpiredInYearCertificates { get; set; }
    }

    public class StatCoupleCerByOperatorAreaVM
    {
        public string OperatorID { get; set; }
        public int ValidCertificates { get; set; }
        public int ExpiredInYearCertificates { get; set; }
        public string Area { get; set; }
    }

    public class StatCerByOperatorAreaVM
    {
        public string OperatorID { get; set; }
        public int ValidCertificates { get; set; }
        public string Area { get; set; }
    }

    public class StatCerByOperatorVM
    {
        public string OperatorID { get; set; }
        public int ValidCertificates { get; set; }        
    }

    public class StatCerByAreaVM
    {
        public string Area { get; set; }
        public int ValidCertificates { get; set; }
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

    public class StatIssuedCerByOperatorYearVM
    {
        public string OperatorID { get; set; }
        public string Year { get; set; }
        public int IssuedCertificates { get; set; }
    }

    public class StatIssuedCerByAreaYearVM
    {
        public string Area { get; set; }
        public string Year { get; set; }
        public int IssuedCertificates { get; set; }
    }


    public class StatIssuedCerByOperatorAreaVM
    {
        public string OperatorID { get; set; }
        public string Area { get; set; }
        public int IssuedCertificates { get; set; }
    }


    public class StatIssuedCerByOperatorCityVM
    {
        public string OperatorID { get; set; }
        public string CityID { get; set; }
        public int IssuedCertificates { get; set; }
    }

    public class StatNearExpiredInYearCerByOperatorCityVM
    {
        public string OperatorID { get; set; }
        public string CityID { get; set; }
        public int NearExpiredInYearCertificates { get; set; }
    }

    public class StatExpiredCerByOperatorCityVM
    {
        public string OperatorID { get; set; }
        public string CityID { get; set; }
        public int ExpiredCertificates { get; set; }
    }

    public class StatExpiredCerByOperatorYearVM
    {
        public string OperatorID { get; set; }
        public string Year { get; set; }
        public int ExpiredCertificates { get; set; }
    }

    public class StatExpiredCerByAreaYearVM
    {
        public string Area { get; set; }
        public string Year { get; set; }
        public int ExpiredCertificates { get; set; }
    }
}