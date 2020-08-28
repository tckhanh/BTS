using BTS.Common;
using BTS.Common.ViewModels;
using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.Repository
{
    public interface ISubBtsInCertRepository : IRepository<SubBtsInCert>
    {
        IEnumerable<StatBtsVM> GetStatBtsByOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<StatBtsVM> GetStatBtsByCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<StatBtsByBandVM> GetStatBtsByBand(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<StatBtsByOperatorBandVM> GetStatBtsByOperatorBand(string operatorId = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);
        IEnumerable<StatBtsByAreaBandVM> GetStatBtsByAreaBand(string area = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<StatBtsByBandCityVM> GetStatBtsByBandCity(string Area = CommonConstants.SelectAll);

        IEnumerable<StatBtsByOperatorCityVM> GetStatBtsByOperatorCity(string Area = CommonConstants.SelectAll);

        IEnumerable<StatBtsByOperatorAreaVM> GetStatBtsByOperatorArea(string operatorID = CommonConstants.SelectAll);
                
        IEnumerable<StatBtsByManufactoryVM> GetStatBtsByManufactory(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<StatBtsByOperatorManufactoryVM> GetStatBtsByOperatorManufactory(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);
        IEnumerable<StatBtsByAreaManufactoryVM> GetStatBtsByAreaManufactory(string area = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<StatBtsByEquipmentVM> GetStatBtsByEquipemnt(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);
    }

    public class SubBtsInCertRepository : RepositoryBase<SubBtsInCert>, ISubBtsInCertRepository
    {
        public SubBtsInCertRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<StatBtsVM> GetStatBtsByOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query1 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates on subBts.CertificateID equals cert.Id
                         join oper in DbContext.Operators on subBts.OperatorID equals oper.Id
                         where ((operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new {oper.RootId, subBts.BtsCode } into ItemGroup
                         select new
                         {
                             Id = ItemGroup.Max(x => x.Id)
                         };

            var query2 = from subBts in DbContext.SubBtsInCerts
                         join q1 in query1 on subBts.Id equals q1.Id
                         join cert in DbContext.Certificates on subBts.CertificateID equals cert.Id
                         join oper in DbContext.Operators on subBts.OperatorID equals oper.Id
                         group subBts by new { oper.RootId } into ItemGroup
                         select new StatBtsVM()
                         {
                             OperatorID = ItemGroup.Key.RootId.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsVM> GetStatBtsByCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query1 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates on subBts.CertificateID equals cert.Id
                         where ((operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.OperatorID, subBts.BtsCode } into ItemGroup
                         select new
                         {
                             Id = ItemGroup.Max(x => x.Id)
                         };

            var query2 = from subBts in DbContext.SubBtsInCerts
                         join q1 in query1 on subBts.Id equals q1.Id
                         join cert in DbContext.Certificates on subBts.CertificateID equals cert.Id
                         group subBts by new { cert.Id } into ItemGroup
                         select new StatBtsVM()
                         {
                             CityID = ItemGroup.Key.Id.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsByBandVM> GetStatBtsByBand(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.Band } into ItemGroup
                         select new StatBtsByBandVM()
                         {
                             Band = ItemGroup.Key.Band.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsByOperatorBandVM> GetStatBtsByOperatorBand(string operatorId = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates on subBts.CertificateID equals cert.Id
                         join oper in DbContext.Operators on subBts.OperatorID equals oper.Id
                         where (cert.ExpiredDate >= DateTime.Now && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { oper.RootId, subBts.Band } into ItemGroup
                         select new StatBtsByOperatorBandVM()
                         {
                             OperatorID = ItemGroup.Key.RootId,
                             Band = ItemGroup.Key.Band,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsByAreaBandVM> GetStatBtsByAreaBand(string area = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join certificateArea in (from certificate in DbContext.Certificates
                                                  join city in DbContext.Cities on certificate.CityID equals city.Id
                                                  where ((area == CommonConstants.SelectAll || city.Area == area))
                                                  select new { certificate.Id, certificate.ExpiredDate, certificate.CityID, city.Area })
                         on subBts.CertificateID equals certificateArea.Id
                         where (certificateArea.ExpiredDate >= DateTime.Now && (cityID == CommonConstants.SelectAll || certificateArea.CityID == cityID))
                         group subBts by new { certificateArea.Area, subBts.Band } into ItemGroup
                         select new StatBtsByAreaBandVM()
                         {
                             Area = ItemGroup.Key.Area,
                             Band = ItemGroup.Key.Band,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsByBandCityVM> GetStatBtsByBandCity(string Area = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates on subBts.CertificateID equals cert.Id
                         join city in DbContext.Cities on cert.CityID equals city.Id
                         where ((Area == CommonConstants.SelectAll || city.Area == Area) && cert.ExpiredDate >= DateTime.Now && (Area == CommonConstants.SelectAll || subBts.OperatorID == cert.OperatorID))
                         group subBts by new { cert.CityID, subBts.Band } into ItemGroup
                         select new StatBtsByBandCityVM()
                         {
                             CityID = ItemGroup.Key.CityID,
                             Band = ItemGroup.Key.Band,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsByOperatorCityVM> GetStatBtsByOperatorCity(string Area = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates on subBts.CertificateID equals cert.Id
                         join city in DbContext.Cities on cert.CityID equals city.Id
                         join oper in DbContext.Operators on subBts.OperatorID equals oper.Id
                         where (cert.ExpiredDate >= DateTime.Now && (Area == CommonConstants.SelectAll || city.Area == Area))
                         group subBts by new { cert.CityID, oper.RootId } into ItemGroup
                         select new StatBtsByOperatorCityVM()
                         {
                             CityID = ItemGroup.Key.CityID,
                             OperatorID = ItemGroup.Key.RootId,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsByOperatorAreaVM> GetStatBtsByOperatorArea(string operatorID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates on subBts.CertificateID equals cert.Id
                         join city in DbContext.Cities on cert.CityID equals city.Id
                         join oper in DbContext.Operators on subBts.OperatorID equals oper.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID))
                         group subBts by new { city.Area, oper.RootId } into ItemGroup
                         select new StatBtsByOperatorAreaVM()
                         {
                             Area = ItemGroup.Key.Area,
                             OperatorID = ItemGroup.Key.RootId,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }
        public IEnumerable<StatBtsByManufactoryVM> GetStatBtsByManufactory(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.Manufactory } into ItemGroup
                         select new StatBtsByManufactoryVM()
                         {
                             Manufactory = ItemGroup.Key.Manufactory,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsByOperatorManufactoryVM> GetStatBtsByOperatorManufactory(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates on subBts.CertificateID equals cert.Id
                         join oper in DbContext.Operators on subBts.OperatorID equals oper.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { oper.RootId , subBts.Manufactory } into ItemGroup
                         select new StatBtsByOperatorManufactoryVM()
                         {
                             OperatorID = ItemGroup.Key.RootId,
                             Manufactory = ItemGroup.Key.Manufactory,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsByAreaManufactoryVM> GetStatBtsByAreaManufactory(string area = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join certificateArea in (from certificate in DbContext.Certificates
                                                       join city in DbContext.Cities on certificate.CityID equals city.Id
                                                       where ((area == CommonConstants.SelectAll || city.Area == area))
                                                       select new { certificate.Id, certificate.ExpiredDate, certificate.CityID, city.Area })
                         on subBts.CertificateID equals certificateArea.Id
                         where (certificateArea.ExpiredDate >= DateTime.Now && (area == CommonConstants.SelectAll || certificateArea.Area == area) && (cityID == CommonConstants.SelectAll || certificateArea.CityID == cityID))
                         group subBts by new { certificateArea.Area, subBts.Manufactory } into ItemGroup
                         select new StatBtsByAreaManufactoryVM()
                         {
                             Area = ItemGroup.Key.Area,
                             Manufactory = ItemGroup.Key.Manufactory,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsByEquipmentVM> GetStatBtsByEquipemnt(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.Equipment } into ItemGroup
                         select new StatBtsByEquipmentVM()
                         {
                             Equipment = ItemGroup.Key.Equipment.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }
    }
}