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
    public interface ICertificateRepository : IRepository<Certificate>
    {
        IEnumerable<Certificate> GetMultiPagingByBtsCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10, bool onlyOwner = false);

        IEnumerable<Certificate> GetMultiByBtsCode(string btsCode, bool onlyOwner = false);

        IEnumerable<CertificateStatisticViewModel> GetStatistic(string fromDate, string toDate);

        IEnumerable<StatisticCertificate> GetStatisticCertificateByYear();

        IEnumerable<StatisticCertificate> GetStatisticCertificateByYearOperator();

        IEnumerable<StatisticCertificate> GetStatisticCertificateByOperatorCity();

        IEnumerable<ShortCertificate> GetShortCertificate();

        IEnumerable<ShortCertificate> GetShortCertificate(int year);
    }

    public class CertificateRepository : RepositoryBase<Certificate>, ICertificateRepository
    {
        public CertificateRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Certificate> GetMultiByBtsCode(string btsCode, bool onlyOwner = true)
        {
            if (onlyOwner)
            {
                var query = from certificate in DbContext.Certificates
                            join subBts in DbContext.SubBtsInCerts
                            on certificate.ID equals subBts.CertificateID
                            where subBts.BtsCode == btsCode && subBts.OperatorID == certificate.OperatorID
                            orderby certificate.IssuedDate, certificate.ID descending
                            select certificate;
                return query;
            }
            else
            {
                var query = from certificate in DbContext.Certificates
                            join subBts in DbContext.SubBtsInCerts
                            on certificate.ID equals subBts.CertificateID
                            where subBts.BtsCode == btsCode
                            orderby certificate.IssuedDate, certificate.ID descending
                            select certificate;
                return query;
            }
        }

        public IEnumerable<Certificate> GetMultiPagingByBtsCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10, bool onlyOwner = true)
        {
            if (onlyOwner)
            {
                var query = from certificate in DbContext.Certificates
                            join subBts in DbContext.SubBtsInCerts
                            on certificate.ID equals subBts.CertificateID
                            where subBts.BtsCode == btsCode && subBts.OperatorID == certificate.OperatorID
                            orderby certificate.IssuedDate, certificate.ID descending
                            select certificate;
                totalRow = query.Count();
                return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                var query = from certificate in DbContext.Certificates
                            join subBts in DbContext.SubBtsInCerts
                            on certificate.ID equals subBts.CertificateID
                            where subBts.BtsCode == btsCode
                            orderby certificate.IssuedDate, certificate.ID descending
                            select certificate;
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

        public IEnumerable<StatisticCertificate> GetStatisticCertificateByYear()
        {
            var query = from certificate in DbContext.Certificates
                        where !string.IsNullOrEmpty(certificate.OperatorID) && certificate.IssuedDate != null
                        orderby certificate.IssuedDate ascending
                        group certificate by new { certificate.IssuedDate.Value.Year } into OperatorGroup
                        select new StatisticCertificate()
                        {
                            Year = OperatorGroup.Key.Year.ToString(),
                            Certificates = OperatorGroup.Count()
                        };

            return query;
        }

        public IEnumerable<StatisticCertificate> GetStatisticCertificateByOperatorCity()
        {
            var query = from certificate in DbContext.Certificates
                        where !string.IsNullOrEmpty(certificate.OperatorID)
                        group certificate by new { certificate.OperatorID, certificate.CityID } into OperatorGroup
                        select new StatisticCertificate()
                        {
                            OperatorID = OperatorGroup.Key.OperatorID,
                            CityID = OperatorGroup.Key.CityID,
                            Certificates = OperatorGroup.Count()
                        };

            return query;
        }

        IEnumerable<CertificateStatisticViewModel> ICertificateRepository.GetStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<CertificateStatisticViewModel>("GetCertificateStatistic @fromDate,@toDate", parameters);
        }

        public IEnumerable<StatisticCertificate> GetStatisticCertificateByYearOperator()
        {
            var query = from certificate in DbContext.Certificates
                        where !string.IsNullOrEmpty(certificate.ID) && !string.IsNullOrEmpty(certificate.OperatorID) && certificate.IssuedDate != null
                        orderby certificate.IssuedDate ascending
                        group certificate by new { certificate.IssuedDate.Value.Year, certificate.OperatorID } into OperatorGroup
                        select new StatisticCertificate()
                        {
                            Year = OperatorGroup.Key.Year.ToString(),

                            Certificates = OperatorGroup.Count()
                        };

            return query;
        }

        public IEnumerable<ShortCertificate> GetShortCertificate(int year)
        {
            var query = from certificate in DbContext.Certificates
                        where certificate.IssuedDate.Value.Year == year
                        select new ShortCertificate()
                        {
                            ID = certificate.ID,
                            Year = year,
                            OperatorID = certificate.OperatorID,
                            CityID = certificate.CityID,
                            InCaseOfID = certificate.InCaseOfID,
                            LabID = certificate.LabID
                        };
            return query;
        }

        public IEnumerable<ShortCertificate> GetShortCertificate()
        {
            var query = from certificate in DbContext.Certificates
                        where certificate.ExpiredDate <= DateTime.Now
                        select new ShortCertificate()
                        {
                            ID = certificate.ID,
                            Year = certificate.IssuedDate.Value.Year,
                            OperatorID = certificate.OperatorID,
                            CityID = certificate.CityID,
                            InCaseOfID = certificate.InCaseOfID,
                            LabID = certificate.LabID
                        };
            return query;
        }
    }
}