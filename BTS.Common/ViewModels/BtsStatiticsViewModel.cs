using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Common.ViewModels
{
    public class BtsStatiticsViewModel
    {
        public int ID { get; set; }
        public string BTSCode { get; set; }
        public string OperatorID { get; set; }
        public string CityID { get; set; }
        public string Manufactory { get; set; }
        public string Equipment { get; set; }
        public string Band { get; set; }
        public int Btss { get; set; }
    }
}