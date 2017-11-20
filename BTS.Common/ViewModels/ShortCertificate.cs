using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Common.ViewModels
{
    public class ShortCertificate
    {
        public string ID { get; set; }
        public int Year { get; set; }
        public string OperatorID { get; set; }
        public string CityID { get; set; }
        public int InCaseOfID { get; set; }
        public string LabID { get; set; }
    }
}