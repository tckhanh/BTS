using BTS.Common.ViewModels;
using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.Repository
{
    public interface IBTSCertificateRepository : IRepository<BTSCertificate>
    {
        IEnumerable<BTSCertificate> GetMultiPagingByBtsCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10, bool onlyOwner = false);
        IEnumerable<BTSCertificate> GetMultiByBtsCode(string btsCode, bool onlyOwner = false);
        IEnumerable<CertificateStatisticViewModel> GetStatistic(string fromDate, string toDate);

        IEnumerable<StatisticCertificateByYear> GetStatisticCertificateByYear();

        IEnumerable<StatisticCertificateByYear> GetStatisticCertificateByYearOperator();

        IEnumerable<StatisticCertificateByOperatorCity> GetStatisticCertificateByOperatorCity();

    }

    public class BTSCertificateRepository : RepositoryBase<BTSCertificate>, IBTSCertificateRepository
    {
        public BTSCertificateRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<BTSCertificate> GetMultiByBtsCode(string btsCode, bool onlyOwner = true)
        {
            if (onlyOwner)
            {
                var query = from bts in DbContext.BTSCertificates
                            join subBts in DbContext.SubBTSs
                            on bts.ID equals subBts.BTSCertificateID
                            where subBts.BTSCode == btsCode && subBts.OperatorID == bts.OperatorID
                            orderby bts.IssuedDate, bts.ID descending
                            select bts;
                return query;
            }
            else
            {
                var query = from bts in DbContext.BTSCertificates
                            join subBts in DbContext.SubBTSs
                            on bts.ID equals subBts.BTSCertificateID
                            where subBts.BTSCode == btsCode
                            orderby bts.IssuedDate, bts.ID descending
                            select bts;
                return query;
            }
        }

        public IEnumerable<BTSCertificate> GetMultiPagingByBtsCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10, bool onlyOwner = true)
        {
            if (onlyOwner)
            {
                var query = from bts in DbContext.BTSCertificates
                            join subBts in DbContext.SubBTSs
                            on bts.ID equals subBts.BTSCertificateID
                            where subBts.BTSCode == btsCode && subBts.OperatorID == bts.OperatorID
                            orderby bts.IssuedDate, bts.ID descending
                            select bts;
                totalRow = query.Count();
                return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                var query = from bts in DbContext.BTSCertificates
                            join subBts in DbContext.SubBTSs
                            on bts.ID equals subBts.BTSCertificateID
                            where subBts.BTSCode == btsCode
                            orderby bts.IssuedDate, bts.ID descending
                            select bts;
                totalRow = query.Count();
                return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }

        public IEnumerable<CertificateStatisticViewModel> GetStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<CertificateStatisticViewModel>("GetStatistic @fromDate,@toDate", parameters);

        }

        public IEnumerable<StatisticCertificateByYear> GetStatisticCertificateByYear()
        {
            var query = from bts in DbContext.BTSCertificates
                        where !string.IsNullOrEmpty(bts.CertificateNum) && !string.IsNullOrEmpty(bts.OperatorID) && bts.IssuedDate != null
                        orderby bts.IssuedDate ascending
                        group bts by new { bts.IssuedDate.Value.Year} into OperatorGroup                        
                        select new StatisticCertificateByYear() {
                            Year = OperatorGroup.Key.Year.ToString(),                            
                            Certificates = OperatorGroup.Count()
                            };

            return query;
        }

        public IEnumerable<StatisticCertificateByOperatorCity> GetStatisticCertificateByOperatorCity()
        {
            var query = from bts in DbContext.BTSCertificates
                        where !string.IsNullOrEmpty(bts.CertificateNum) && !string.IsNullOrEmpty(bts.OperatorID)
                        group bts by new { bts.OperatorID, bts.CityID } into OperatorGroup
                        select new StatisticCertificateByOperatorCity()
                        {
                            OperatorID = OperatorGroup.Key.OperatorID,
                            CityID = OperatorGroup.Key.CityID,
                            Certificates = OperatorGroup.Count()
                        };

            return query;
        }

        IEnumerable<CertificateStatisticViewModel> IBTSCertificateRepository.GetStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<CertificateStatisticViewModel>("GetCertificateStatistic @fromDate,@toDate", parameters);
        }

        public IEnumerable<StatisticCertificateByYear> GetStatisticCertificateByYearOperator()
        {
            var query = from bts in DbContext.BTSCertificates
                        where !string.IsNullOrEmpty(bts.CertificateNum) && !string.IsNullOrEmpty(bts.OperatorID) && bts.IssuedDate != null
                        orderby bts.IssuedDate ascending
                        group bts by new { bts.IssuedDate.Value.Year, bts.OperatorID } into OperatorGroup
                        select new StatisticCertificateByYear()
                        {
                            Year = OperatorGroup.Key.Year.ToString(),
                            
                            Certificates = OperatorGroup.Count()
                        };

            return query;
        }
    }
}
