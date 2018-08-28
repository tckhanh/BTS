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
        IEnumerable<BtsStatVM> GetBtsStatByOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<BtsStatVM> GetBtsStatByCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<BtsStatByBandVM> GetBtsStatByBand(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<BtsStatByOperatorBandVM> GetBtsStatByOperatorBand(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<BtsStatByBandCityVM> GetBtsStatByBandCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<BtsStatByOperatorCityVM> GetBtsStatByOperatorCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<BtsStatByManufactoryVM> GetBtsStatByManufactory(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<BtsStatByOperatorManufactoryVM> GetBtsStatByOperatorManufactory(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<BtsStatByEquipmentVM> GetBtsStatByEquipemnt(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);
    }

    public class SubBtsInCertRepository : RepositoryBase<SubBtsInCert>, ISubBTSinCertRepository
    {
        public SubBtsInCertRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<BtsStatVM> GetBtsStatByOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
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
                         select new BtsStatVM()
                         {
                             OperatorID = ItemGroup.Key.OperatorID.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<BtsStatVM> GetBtsStatByCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
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
                         select new BtsStatVM()
                         {
                             CityID = ItemGroup.Key.Id.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<BtsStatByBandVM> GetBtsStatByBand(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.Band } into ItemGroup
                         select new BtsStatByBandVM()
                         {
                             Band = ItemGroup.Key.Band.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<BtsStatByOperatorBandVM> GetBtsStatByOperatorBand(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.OperatorID, subBts.Band } into ItemGroup
                         select new BtsStatByOperatorBandVM()
                         {
                             OperatorID = ItemGroup.Key.OperatorID,
                             Band = ItemGroup.Key.Band,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<BtsStatByBandCityVM> GetBtsStatByBandCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { cert.CityID, subBts.Band } into ItemGroup
                         select new BtsStatByBandCityVM()
                         {
                             CityID = ItemGroup.Key.CityID,
                             Band = ItemGroup.Key.Band,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<BtsStatByOperatorCityVM> GetBtsStatByOperatorCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { cert.CityID, subBts.OperatorID } into ItemGroup
                         select new BtsStatByOperatorCityVM()
                         {
                             CityID = ItemGroup.Key.CityID,
                             OperatorID = ItemGroup.Key.OperatorID,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<BtsStatByManufactoryVM> GetBtsStatByManufactory(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.Manufactory } into ItemGroup
                         select new BtsStatByManufactoryVM()
                         {
                             Manufactory = ItemGroup.Key.Manufactory,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<BtsStatByOperatorManufactoryVM> GetBtsStatByOperatorManufactory(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.OperatorID, subBts.Manufactory } into ItemGroup
                         select new BtsStatByOperatorManufactoryVM()
                         {
                             OperatorID = ItemGroup.Key.OperatorID,
                             Manufactory = ItemGroup.Key.Manufactory,
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<BtsStatByEquipmentVM> GetBtsStatByEquipemnt(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query2 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.Id
                         where (cert.ExpiredDate >= DateTime.Now && (operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.Equipment } into ItemGroup
                         select new BtsStatByEquipmentVM()
                         {
                             Equipment = ItemGroup.Key.Equipment.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }
    }
}