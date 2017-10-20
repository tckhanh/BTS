using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Common.ViewModels
{
    [Serializable]
    public class StatisticCertificateByYear
    {
        public string Year { get; set; }
        
        public int Certificates { get; set; }
    }
}
