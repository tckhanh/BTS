using BTS.Common;
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

        IEnumerable<CertStatViewModel> GetStatistic(string fromDate, string toDate);

        IEnumerable<CertStatViewModel> GetCertificateStatisticByYearOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false);

        IEnumerable<CertStatByOperatorViewModel> GetCertificateStatisticByOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false);

        IEnumerable<CertStatViewModel> GetCertificateStatisticByCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false);

        IEnumerable<CertStatViewModel> GetCertificateStatisticByLab(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false);

        IEnumerable<CertStatViewModel> GetCertificateStatisticByOperatorCity();

        IEnumerable<ShortCertificate> GetShortCertificate();

        IEnumerable<ShortCertificate> GetShortCertificate(int year);

        IEnumerable<Certificate> getLastOwnCertificates(string btsCode, string operatorID);

        IEnumerable<Certificate> getLastNoOwnCertificates(string btsCode, string operatorID);

        IEnumerable<string> GetIssueYears();
    }

    public class CertificateRepository : RepositoryBase<Certificate>, ICertificateRepository
    {
        public CertificateRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Certificate> GetMultiByBtsCode(string btsCode, bool onlyOwner = true)
        {
            btsCode = btsCode.Trim().ToUpper();
            if (onlyOwner)
            {
                var query = from certificate in DbContext.Certificates
                            join subBts in DbContext.SubBtsInCerts
                            on certificate.ID equals subBts.CertificateID
                            where subBts.BtsCode.Trim().ToUpper() == btsCode && subBts.OperatorID == certificate.OperatorID
                            orderby certificate.IssuedDate, certificate.ID descending
                            select certificate;
                return query;
            }
            else
            {
                var query = from certificate in DbContext.Certificates
                            join subBts in DbContext.SubBtsInCerts
                            on certificate.ID equals subBts.CertificateID
                            where subBts.BtsCode.Trim().ToUpper() == btsCode
                            orderby certificate.IssuedDate, certificate.ID descending
                            select certificate;
                return query;
            }
        }

        public IEnumerable<Certificate> GetMultiPagingByBtsCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10, bool onlyOwner = true)
        {
            btsCode = btsCode.Trim().ToUpper();
            if (onlyOwner)
            {
                var query = from certificate in DbContext.Certificates
                            join subBts in DbContext.SubBtsInCerts
                            on certificate.ID equals subBts.CertificateID
                            where subBts.BtsCode.Trim().ToUpper() == btsCode && subBts.OperatorID == certificate.OperatorID
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
                            where subBts.BtsCode.Trim().ToUpper() == btsCode
                            orderby certificate.IssuedDate, certificate.ID descending
                            select certificate;
                totalRow = query.Count();
                return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }

        public IEnumerable<CertStatViewModel> GetStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<CertStatViewModel>("GetStatistic @fromDate,@toDate", parameters);
        }

        public IEnumerable<CertStatViewModel> GetCertificateStatisticByYearOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                var query = from certificate in DbContext.Certificates
                            where ((certificate.ExpiredDate >= DateTime.Now) && (operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by new { certificate.IssuedDate.Value.Year, certificate.OperatorID } into ItemGroup
                            select new CertStatViewModel()
                            {
                                Year = ItemGroup.Key.Year.ToString(),
                                OperatorID = ItemGroup.Key.OperatorID,
                                ValidCertificates = ItemGroup.Count(),
                                ExpiredInYearCertificates = ItemGroup.Where(x => x.ExpiredDate.Value.Year == DateTime.Now.Year && x.ExpiredDate >= DateTime.Now).Count()
                            };
                return query;
            }
            else
            {
                var query = from certificate in DbContext.Certificates
                            where ((operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by new { certificate.IssuedDate.Value.Year, certificate.OperatorID } into ItemGroup
                            select new CertStatViewModel()
                            {
                                Year = ItemGroup.Key.Year.ToString(),
                                OperatorID = ItemGroup.Key.OperatorID,
                                ValidCertificates = ItemGroup.Count(),
                                ExpiredInYearCertificates = ItemGroup.Where(x => x.ExpiredDate.Value.Year == DateTime.Now.Year && x.ExpiredDate >= DateTime.Now).Count()
                            };
                return query;
            }
        }

        public IEnumerable<CertStatViewModel> GetCertificateStatisticByCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                var query = from certificate in DbContext.Certificates
                            where ((certificate.ExpiredDate >= DateTime.Now) && (operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by new { certificate.CityID } into ItemGroup
                            select new CertStatViewModel()
                            {
                                CityID = ItemGroup.Key.ToString(),
                                ValidCertificates = ItemGroup.Count()
                            };
                return query;
            }
            else
            {
                var query = from certificate in DbContext.Certificates
                            where ((operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by new { certificate.CityID } into ItemGroup
                            select new CertStatViewModel()
                            {
                                CityID = ItemGroup.Key.ToString(),
                                ValidCertificates = ItemGroup.Count()
                            };
                return query;
            }
        }

        public IEnumerable<CertStatByOperatorViewModel> GetCertificateStatisticByOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                var query = from certificate in DbContext.Certificates
                            where ((certificate.ExpiredDate >= DateTime.Now) && (operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by certificate.OperatorID into ItemGroup
                            select new CertStatByOperatorViewModel()
                            {
                                OperatorID = ItemGroup.Key.ToString(),
                                ValidCertificates = ItemGroup.Count(),
                                ExpiredInYearCertificates = ItemGroup.Where(x => x.ExpiredDate.Value.Year == DateTime.Now.Year && x.ExpiredDate >= DateTime.Now).Count()
                            };
                return query;
            }
            else
            {
                var query = from certificate in DbContext.Certificates
                            where ((operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by certificate.OperatorID into ItemGroup
                            select new CertStatByOperatorViewModel()
                            {
                                OperatorID = ItemGroup.Key.ToString(),
                                ValidCertificates = ItemGroup.Count(),
                                ExpiredInYearCertificates = ItemGroup.Where(x => x.ExpiredDate.Value.Year == DateTime.Now.Year && x.ExpiredDate >= DateTime.Now).Count()
                            };
                return query;
            }
        }

        public IEnumerable<CertStatViewModel> GetCertificateStatisticByLab(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                var query = from certificate in DbContext.Certificates
                            where ((certificate.ExpiredDate >= DateTime.Now) && (operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by new { certificate.LabID } into ItemGroup
                            select new CertStatViewModel()
                            {
                                LabID = ItemGroup.Key.ToString(),
                                ValidCertificates = ItemGroup.Count()
                            };
                return query;
            }
            else
            {
                var query = from certificate in DbContext.Certificates
                            where ((operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by new { certificate.LabID } into ItemGroup
                            select new CertStatViewModel()
                            {
                                LabID = ItemGroup.Key.ToString(),
                                ValidCertificates = ItemGroup.Count()
                            };
                return query;
            }
        }

        public IEnumerable<CertStatViewModel> GetCertificateStatisticByOperatorCity()
        {
            var query = from certificate in DbContext.Certificates
                        where !string.IsNullOrEmpty(certificate.OperatorID)
                        group certificate by new { certificate.OperatorID, certificate.CityID } into OperatorGroup
                        select new CertStatViewModel()
                        {
                            OperatorID = OperatorGroup.Key.OperatorID,
                            CityID = OperatorGroup.Key.CityID,
                            ValidCertificates = OperatorGroup.Count()
                        };

            return query;
        }

        IEnumerable<CertStatViewModel> ICertificateRepository.GetStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<CertStatViewModel>("GetCertificateStatistic @fromDate,@toDate", parameters);
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

        public IEnumerable<Certificate> getLastOwnCertificates(string btsCode, string operatorID)
        {
            btsCode = btsCode.Trim().ToUpper();
            IEnumerable<string> query1 = (from subBtsInCert in DbContext.SubBtsInCerts
                                          where subBtsInCert.BtsCode.Trim().ToUpper() == btsCode && subBtsInCert.OperatorID == operatorID
                                          select subBtsInCert.CertificateID).Distinct();

            var query2 = from item1 in query1
                         join certificate in DbContext.Certificates
                         on item1 equals certificate.ID
                         where certificate.OperatorID == operatorID
                         orderby certificate.IssuedDate descending
                         select certificate;
            return query2;
        }

        public IEnumerable<Certificate> getLastNoOwnCertificates(string btsCode, string operatorID)
        {
            btsCode = btsCode.Trim().ToUpper();
            IEnumerable<string> query1 = (from subBtsInCert in DbContext.SubBtsInCerts
                                          where subBtsInCert.BtsCode.Trim().ToUpper() == btsCode && subBtsInCert.OperatorID == operatorID
                                          select subBtsInCert.CertificateID).Distinct();

            var query2 = from item1 in query1
                         join certificate in DbContext.Certificates
                         on item1 equals certificate.ID
                         where certificate.OperatorID != operatorID
                         orderby certificate.IssuedDate descending
                         select certificate;
            return query2;

            //IEnumerable<SubBtsInCert> query1 = from subBtsInCert in DbContext.SubBtsInCerts
            //                                   where subBtsInCert.BtsCode.Trim().ToUpper() == btsCode && subBtsInCert.OperatorID == operatorID
            //                                   select subBtsInCert;

            //var query2 = from certificate in DbContext.Certificates
            //             join item1 in query1
            //             on certificate.ID equals item1.CertificateID
            //             where certificate.OperatorID != operatorID
            //             orderby certificate.IssuedDate descending
            //             select certificate;
            //return query2;
        }

        public IEnumerable<string> GetIssueYears()
        {
            IEnumerable<string> query1 = from certificate in DbContext.Certificates
                                         group certificate by certificate.IssuedDate.Value.Year into OperatorGroup
                                         select OperatorGroup.Key.ToString();
            return query1;
        }
    }
}