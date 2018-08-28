using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Common.ViewModels
{
    public class BtsStatVM
    {
        public int Id { get; set; }
        public string BTSCode { get; set; }
        public string OperatorID { get; set; }
        public string CityID { get; set; }
        public string Manufactory { get; set; }
        public string Equipment { get; set; }
        public string Band { get; set; }
        public int Btss { get; set; }
    }

    public class BtsStatByBandVM
    {
        public string Band { get; set; }
        public int Btss { get; set; }
    }

    public class BtsStatByOperatorBandVM
    {
        public string OperatorID { get; set; }
        public string Band { get; set; }
        public int Btss { get; set; }
    }

    public class BtsStatByOperatorCityVM
    {
        public string OperatorID { get; set; }
        public string CityID { get; set; }
        public int Btss { get; set; }
    }

    public class BtsStatByBandCityVM
    {
        public string Band { get; set; }
        public string CityID { get; set; }
        public int Btss { get; set; }
    }

    public class BtsStatByManufactoryVM
    {
        public string Manufactory { get; set; }
        public int Btss { get; set; }
    }

    public class BtsStatByOperatorManufactoryVM
    {
        public string OperatorID { get; set; }
        public string Manufactory { get; set; }
        public int Btss { get; set; }
    }

    public class BtsStatByEquipmentVM
    {
        public string Equipment { get; set; }
        public int Btss { get; set; }
    }
}