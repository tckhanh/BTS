using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Common.ViewModels
{
    public class CertificateStatiticsViewModel
    {
        public string Year { get; set; }
        public string OperatorID { get; set; }
        public string CityID { get; set; }
        public int InCaseOfID { get; set; }
        public string LabID { get; set; }
        public int Certificates { get; set; }
    }
}