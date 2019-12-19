using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Common.ViewModels
{
    public class StatBtsVM
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

    public class StatBtsByBandVM
    {
        public string Band { get; set; }
        public int Btss { get; set; }
    }

    public class StatBtsByOperatorBandVM
    {
        public string OperatorID { get; set; }
        public string Band { get; set; }
        public int Btss { get; set; }
    }

    public class StatBtsByOperatorCityVM
    {
        public string OperatorID { get; set; }
        public string CityID { get; set; }
        public int Btss { get; set; }
    }

    public class StatBtsByOperatorAreaVM
    {
        public string OperatorID { get; set; }
        public string Area { get; set; }
        public int Btss { get; set; }
    }

    public class StatBtsByBandCityVM
    {
        public string Band { get; set; }
        public string CityID { get; set; }
        public int Btss { get; set; }
    }

    public class StatBtsByManufactoryVM
    {
        public string Manufactory { get; set; }
        public int Btss { get; set; }
    }

    public class StatBtsByOperatorManufactoryVM
    {
        public string OperatorID { get; set; }
        public string Manufactory { get; set; }
        public int Btss { get; set; }
    }

    public class StatBtsByEquipmentVM
    {
        public string Equipment { get; set; }
        public int Btss { get; set; }
    }
}