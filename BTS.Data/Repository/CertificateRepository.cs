﻿using BTS.Common;
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

        IEnumerable<CertStatVM> GetStatistic(string fromDate, string toDate);

        IEnumerable<IssuedCertStatByOperatorYearVM> GetIssuedCertStatByOperatorYear(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false);

        IEnumerable<IssuedCertStatByOperatorCityVM> GetIssuedCertStatByOperatorCity(bool onlyIsValid = false);

        IEnumerable<ExpiredCertStatByOperatorYearVM> GetExpiredCertStatByOperatorYear(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false);

        IEnumerable<CertStatByOperatorVM> GetCertStatByOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false);

        IEnumerable<StatBtsInProcessVm> GetStatBtsInProcess();

        IEnumerable<CertStatVM> GetCertStatByCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false);

        IEnumerable<CertStatVM> GetCerStatByLab(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false);

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
                            on certificate.Id equals subBts.CertificateID
                            where subBts.BtsCode.Trim().ToUpper() == btsCode && subBts.OperatorID == certificate.OperatorID
                            orderby certificate.IssuedDate, certificate.Id descending
                            select certificate;
                return query;
            }
            else
            {
                var query = from certificate in DbContext.Certificates
                            join subBts in DbContext.SubBtsInCerts
                            on certificate.Id equals subBts.CertificateID
                            where subBts.BtsCode.Trim().ToUpper() == btsCode
                            orderby certificate.IssuedDate, certificate.Id descending
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
                            on certificate.Id equals subBts.CertificateID
                            where subBts.BtsCode.Trim().ToUpper() == btsCode && subBts.OperatorID == certificate.OperatorID
                            orderby certificate.IssuedDate, certificate.Id descending
                            select certificate;
                totalRow = query.Count();
                return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                var query = from certificate in DbContext.Certificates
                            join subBts in DbContext.SubBtsInCerts
                            on certificate.Id equals subBts.CertificateID
                            where subBts.BtsCode.Trim().ToUpper() == btsCode
                            orderby certificate.IssuedDate, certificate.Id descending
                            select certificate;
                totalRow = query.Count();
                return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }

        public IEnumerable<CertStatVM> GetStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<CertStatVM>("GetStatistic @fromDate,@toDate", parameters);
        }

        public IEnumerable<IssuedCertStatByOperatorYearVM> GetIssuedCertStatByOperatorYear(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                var query = from certificate in DbContext.Certificates
                            where ((certificate.ExpiredDate >= DateTime.Now) && (operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by new { certificate.IssuedDate.Value.Year, certificate.OperatorID } into ItemGroup
                            select new IssuedCertStatByOperatorYearVM()
                            {
                                Year = ItemGroup.Key.Year.ToString(),
                                OperatorID = ItemGroup.Key.OperatorID,
                                IssuedCertificates = ItemGroup.Count()
                            };
                return query;
            }
            else
            {
                var query = from certificate in DbContext.Certificates
                            where ((operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by new { certificate.IssuedDate.Value.Year, certificate.OperatorID } into ItemGroup
                            select new IssuedCertStatByOperatorYearVM()
                            {
                                Year = ItemGroup.Key.Year.ToString(),
                                OperatorID = ItemGroup.Key.OperatorID,
                                IssuedCertificates = ItemGroup.Count()
                            };
                return query;
            }
        }

        public IEnumerable<ExpiredCertStatByOperatorYearVM> GetExpiredCertStatByOperatorYear(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                var query = from certificate in DbContext.Certificates
                            where ((certificate.ExpiredDate >= DateTime.Now) && (operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by new { certificate.ExpiredDate.Value.Year, certificate.OperatorID } into ItemGroup
                            select new ExpiredCertStatByOperatorYearVM()
                            {
                                Year = ItemGroup.Key.Year.ToString(),
                                OperatorID = ItemGroup.Key.OperatorID,
                                ExpiredInYearCertificates = ItemGroup.Count()
                            };
                return query;
            }
            else
            {
                var query = from certificate in DbContext.Certificates
                            where ((operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by new { certificate.ExpiredDate.Value.Year, certificate.OperatorID } into ItemGroup
                            select new ExpiredCertStatByOperatorYearVM()
                            {
                                Year = ItemGroup.Key.Year.ToString(),
                                OperatorID = ItemGroup.Key.OperatorID,
                                ExpiredInYearCertificates = ItemGroup.Count()
                            };
                return query;
            }
        }

        public IEnumerable<CertStatVM> GetCertStatByCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                var query = from certificate in DbContext.Certificates
                            where ((certificate.ExpiredDate >= DateTime.Now) && (operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by new { certificate.CityID } into ItemGroup
                            select new CertStatVM()
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
                            select new CertStatVM()
                            {
                                CityID = ItemGroup.Key.ToString(),
                                ValidCertificates = ItemGroup.Count()
                            };
                return query;
            }
        }

        public IEnumerable<CertStatByOperatorVM> GetCertStatByOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                var query = from certificate in DbContext.Certificates
                            where ((certificate.ExpiredDate >= DateTime.Now) && (operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by certificate.OperatorID into ItemGroup
                            select new CertStatByOperatorVM()
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
                            select new CertStatByOperatorVM()
                            {
                                OperatorID = ItemGroup.Key.ToString(),
                                ValidCertificates = ItemGroup.Count(),
                                ExpiredInYearCertificates = ItemGroup.Where(x => x.ExpiredDate.Value.Year == DateTime.Now.Year && x.ExpiredDate >= DateTime.Now).Count()
                            };
                return query;
            }
        }

        public IEnumerable<CertStatVM> GetCerStatByLab(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                var query = from certificate in DbContext.Certificates
                            where ((certificate.ExpiredDate >= DateTime.Now) && (operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                            group certificate by new { certificate.LabID } into ItemGroup
                            select new CertStatVM()
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
                            select new CertStatVM()
                            {
                                LabID = ItemGroup.Key.ToString(),
                                ValidCertificates = ItemGroup.Count()
                            };
                return query;
            }
        }

        IEnumerable<CertStatVM> ICertificateRepository.GetStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<CertStatVM>("GetCertificateStatistic @fromDate,@toDate", parameters);
        }

        public IEnumerable<ShortCertificate> GetShortCertificate(int year)
        {
            var query = from certificate in DbContext.Certificates
                        where certificate.IssuedDate.Value.Year == year
                        select new ShortCertificate()
                        {
                            Id = certificate.Id,
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
                            Id = certificate.Id,
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
                         on item1 equals certificate.Id
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
                         on item1 equals certificate.Id
                         where certificate.OperatorID != operatorID
                         orderby certificate.IssuedDate descending
                         select certificate;
            return query2;

            //IEnumerable<SubBtsInCert> query1 = from subBtsInCert in DbContext.SubBtsInCerts
            //                                   where subBtsInCert.BtsCode.Trim().ToUpper() == btsCode && subBtsInCert.OperatorID == operatorID
            //                                   select subBtsInCert;

            //var query2 = from certificate in DbContext.Certificates
            //             join item1 in query1
            //             on certificate.Id equals item1.CertificateID
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

        public IEnumerable<IssuedCertStatByOperatorCityVM> GetIssuedCertStatByOperatorCity(bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                var query = from certificate in DbContext.Certificates
                            where (certificate.ExpiredDate >= DateTime.Now)
                            group certificate by new { certificate.CityID, certificate.OperatorID } into ItemGroup
                            select new IssuedCertStatByOperatorCityVM()
                            {
                                CityID = ItemGroup.Key.CityID,
                                OperatorID = ItemGroup.Key.OperatorID,
                                IssuedCertificates = ItemGroup.Count()
                            };
                return query;
            }
            else
            {
                var query = from certificate in DbContext.Certificates
                            group certificate by new { certificate.CityID, certificate.OperatorID } into ItemGroup
                            select new IssuedCertStatByOperatorCityVM()
                            {
                                CityID = ItemGroup.Key.CityID,
                                OperatorID = ItemGroup.Key.OperatorID,
                                IssuedCertificates = ItemGroup.Count()
                            };
                return query;
            }
        }

        public IEnumerable<StatBtsInProcessVm> GetStatBtsInProcess()
        {
            var query = from bts in DbContext.Btss
                        group bts by new { bts.OperatorID } into ItemGroup
                        select new StatBtsInProcessVm()
                        {
                            OperatorID = ItemGroup.Key.OperatorID,
                            Btss = ItemGroup.Count()
                        };
            return query;
        }
    }
}