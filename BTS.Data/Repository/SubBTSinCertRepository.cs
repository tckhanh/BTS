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
        IEnumerable<BtsStatiticsViewModel> GetBtsStatisticByOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<BtsStatiticsViewModel> GetBtsStatisticByCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<BtsStatiticsViewModel> GetBtsStatisticByBand(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<BtsStatiticsViewModel> GetBtsStatisticByManufactory(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);

        IEnumerable<BtsStatiticsViewModel> GetBtsStatisticByEquipemnt(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll);
    }

    public class SubBtsInCertRepository : RepositoryBase<SubBtsInCert>, ISubBTSinCertRepository
    {
        public SubBtsInCertRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<BtsStatiticsViewModel> GetBtsStatisticByOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query1 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.ID
                         where ((operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.OperatorID, subBts.BtsCode } into ItemGroup
                         select new
                         {
                             ID = ItemGroup.Max(x => x.ID)
                         };

            var query2 = from subBts in DbContext.SubBtsInCerts
                         join q1 in query1 on subBts.ID equals q1.ID
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.ID
                         group subBts by new { subBts.OperatorID } into ItemGroup
                         select new BtsStatiticsViewModel()
                         {
                             OperatorID = ItemGroup.Key.OperatorID.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<BtsStatiticsViewModel> GetBtsStatisticByCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query1 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.ID
                         where ((operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.OperatorID, subBts.BtsCode } into ItemGroup
                         select new
                         {
                             ID = ItemGroup.Max(x => x.ID)
                         };

            var query2 = from subBts in DbContext.SubBtsInCerts
                         join q1 in query1 on subBts.ID equals q1.ID
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.ID
                         group subBts by new { cert.ID } into ItemGroup
                         select new BtsStatiticsViewModel()
                         {
                             CityID = ItemGroup.Key.ID.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<BtsStatiticsViewModel> GetBtsStatisticByBand(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query1 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.ID
                         where ((operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.OperatorID, subBts.BtsCode } into ItemGroup
                         select new
                         {
                             ID = ItemGroup.Max(x => x.ID)
                         };

            var query2 = from subBts in DbContext.SubBtsInCerts
                         join q1 in query1 on subBts.ID equals q1.ID
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.ID
                         group subBts by new { subBts.Band } into ItemGroup
                         select new BtsStatiticsViewModel()
                         {
                             Band = ItemGroup.Key.Band.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<BtsStatiticsViewModel> GetBtsStatisticByManufactory(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query1 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.ID
                         where ((operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.OperatorID, subBts.BtsCode } into ItemGroup
                         select new
                         {
                             ID = ItemGroup.Max(x => x.ID)
                         };

            var query2 = from subBts in DbContext.SubBtsInCerts
                         join q1 in query1 on subBts.ID equals q1.ID
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.ID
                         group subBts by new { subBts.Manufactory } into ItemGroup
                         select new BtsStatiticsViewModel()
                         {
                             Manufactory = ItemGroup.Key.Manufactory.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }

        public IEnumerable<BtsStatiticsViewModel> GetBtsStatisticByEquipemnt(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll)
        {
            var query1 = from subBts in DbContext.SubBtsInCerts
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.ID
                         where ((operatorID == CommonConstants.SelectAll || subBts.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || cert.CityID == cityID))
                         group subBts by new { subBts.OperatorID, subBts.BtsCode } into ItemGroup
                         select new
                         {
                             ID = ItemGroup.Max(x => x.ID)
                         };

            var query2 = from subBts in DbContext.SubBtsInCerts
                         join q1 in query1 on subBts.ID equals q1.ID
                         join cert in DbContext.Certificates
                         on subBts.CertificateID equals cert.ID
                         group subBts by new { subBts.Equipment } into ItemGroup
                         select new BtsStatiticsViewModel()
                         {
                             Equipment = ItemGroup.Key.Equipment.ToString(),
                             Btss = ItemGroup.Count()
                         };
            return query2;
        }
    }
}