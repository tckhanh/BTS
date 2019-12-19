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
    public interface ISubBTSinCertRepository : IRepository<SubBtsInCert>
    {
        IEnumerable<StatBtsVM> GetStatBtsByOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<StatBtsVM> GetStatBtsByCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<StatBtsByBandVM> GetStatBtsByBand(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<StatBtsByOperatorBandVM> GetStatBtsByOperatorBand(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<StatBtsByBandCityVM> GetStatBtsByBandCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<StatBtsByOperatorCityVM> GetStatBtsByOperatorCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<StatBtsByOperatorAreaVM> GetStatBtsByOperatorArea(string operatorID = CommonConstants.SelectAll);
                
        IEnumerable<StatBtsByManufactoryVM> GetStatBtsByManufactory(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<StatBtsByOperatorManufactoryVM> GetStatBtsByOperatorManufactory(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<StatBtsByEquipmentVM> GetStatBtsByEquipemnt(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);
    }

    public class SubBtsInCertRepository : RepositoryBase<SubBtsInCert>, ISubBTSinCertRepository
    {
        public SubBtsInCertRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<StatBtsVM> GetStatBtsByOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query1 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         where ((operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.OperatorID, subBts.BtsCode } into ItemGroup
                         select new
                         {
                             Id = ItemGroup.Max(x => x.Id)
                         };

            var query2 = from subBts in DbContext.SubBtsInCerts
                         join q1 in query1 on subBts.Id equals q1.Id
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         group subBts by new { subBts.OperatorID } into ItemGroup
                         select new StatBtsVM()
                         {
                             OperatorID = ItemGroup.Key.OperatorID.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsVM> GetStatBtsByCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query1 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         where ((operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.OperatorID, subBts.BtsCode } into ItemGroup
                         select new
                         {
                             Id = ItemGroup.Max(x => x.Id)
                         };

            var query2 = from subBts in DbContext.SubBtsInCerts
                         join q1 in query1 on subBts.Id equals q1.Id
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
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
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.Band } into ItemGroup
                         select new StatBtsByBandVM()
                         {
                             Band = ItemGroup.Key.Band.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsByOperatorBandVM> GetStatBtsByOperatorBand(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.OperatorID, subBts.Band } into ItemGroup
                         select new StatBtsByOperatorBandVM()
                         {
                             OperatorID = ItemGroup.Key.OperatorID,
                             Band = ItemGroup.Key.Band,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsByBandCityVM> GetStatBtsByBandCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { cert.CityID, subBts.Band } into ItemGroup
                         select new StatBtsByBandCityVM()
                         {
                             CityID = ItemGroup.Key.CityID,
                             Band = ItemGroup.Key.Band,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsByOperatorCityVM> GetStatBtsByOperatorCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { cert.CityID, subBts.OperatorID } into ItemGroup
                         select new StatBtsByOperatorCityVM()
                         {
                             CityID = ItemGroup.Key.CityID,
                             OperatorID = ItemGroup.Key.OperatorID,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsByOperatorAreaVM> GetStatBtsByOperatorArea(string operatorID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         join city in DbContext.Cities
                         on cert.CityID equals city.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID))
                         group subBts by new { city.Area, subBts.OperatorID } into ItemGroup
                         select new StatBtsByOperatorAreaVM()
                         {
                             Area = ItemGroup.Key.Area,
                             OperatorID = ItemGroup.Key.OperatorID,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }
        public IEnumerable<StatBtsByManufactoryVM> GetStatBtsByManufactory(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
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
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.OperatorID, subBts.Manufactory } into ItemGroup
                         select new StatBtsByOperatorManufactoryVM()
                         {
                             OperatorID = ItemGroup.Key.OperatorID,
                             Manufactory = ItemGroup.Key.Manufactory,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<StatBtsByEquipmentVM> GetStatBtsByEquipemnt(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
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