using BTS.Common;
using BTS.Common.ViewModels;
using BTS.Data.Infrastructure;
using BTS.Data.Infrastructure.Extensions;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace BTS.Data.Repository
{
    public interface ICertificateRepository : IRepository<Certificate>
    {
        IEnumerable<Certificate> GetMultiPagingByBtsCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10, bool onlyOwner = false);

        IEnumerable<Certificate> GetMultiByBtsCode(string btsCode, bool onlyOwner = false);

        IEnumerable<CertStatVM> GetStatistic(string fromDate, string toDate);

        IEnumerable<ReportTT18Cert> GetReportTT18CertByDate(DateTime fromDate, DateTime toDate);

        IEnumerable<IssuedCertStatByOperatorYearVM> GetIssuedCertStatByOperatorYear(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false);

        IEnumerable<IssuedCertStatByOperatorCityVM> GetIssuedCertStatByOperatorCity(bool onlyIsValid = false);

        IEnumerable<ExpiredCertStatByOperatorYearVM> GetExpiredCertStatByOperatorYear(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false);

        IEnumerable<CertStatByOperatorVM> GetCertStatByOperator(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false);

        IEnumerable<StatBtsVm> GetStatAllBtsInProcess();

        IEnumerable<CertStatVM> GetStatCertByCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false);

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
                IQueryable<Certificate> query = from certificate in DbContext.Certificates
                                                join subBts in DbContext.SubBtsInCerts
                                                on certificate.Id equals subBts.CertificateID
                                                where subBts.BtsCode.Trim().ToUpper() == btsCode && subBts.OperatorID == certificate.OperatorID
                                                orderby certificate.IssuedDate, certificate.Id descending
                                                select certificate;
                return query;
            }
            else
            {
                IQueryable<Certificate> query = from certificate in DbContext.Certificates
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
                IQueryable<Certificate> query = from certificate in DbContext.Certificates
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
                IQueryable<Certificate> query = from certificate in DbContext.Certificates
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
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<CertStatVM>("GetStatistic @fromDate,@toDate", parameters);
        }

        public IEnumerable<ReportTT18Cert> GetReportTT18CertByDate(DateTime fromDate, DateTime toDate)
        {
            IQueryable<Certificate> query1 = from certificate in DbContext.Certificates
                                             where ((certificate.IssuedDate >= fromDate) && (certificate.IssuedDate >= fromDate))
                                             select certificate;

            IQueryable<ReportTT18Cert> query2 = from certificate in query1
                                            join subBtsInCert in DbContext.SubBtsInCerts
                                            on certificate.Id equals subBtsInCert.CertificateID
                                            select new ReportTT18Cert()
                                            {
                                                CertificateId = certificate.Id,
                                                OperatorID = certificate.OperatorID,
                                                BtsCode = certificate.BtsCode,
                                                Address = certificate.Address,
                                                CityID = certificate.CityID,
                                                Longtitude = certificate.Longtitude,
                                                Latitude = certificate.Latitude,
                                                SubBtsQuantity = certificate.SubBtsQuantity,
                                                BtsSerialNo = subBtsInCert.BtsSerialNo,
                                                SubBtsCode = subBtsInCert.BtsCode,
                                                SubOperatorID = subBtsInCert.OperatorID,
                                                Manufactory = subBtsInCert.Manufactory,
                                                Equipment = subBtsInCert.Equipment,
                                                AntenNum = subBtsInCert.AntenNum,
                                                Configuration = subBtsInCert.Configuration,
                                                PowerSum = subBtsInCert.PowerSum,
                                                Band = subBtsInCert.Band,
                                                AntenHeight = subBtsInCert.AntenHeight
                                            };
            return query2;
        }

        public IEnumerable<IssuedCertStatByOperatorYearVM> GetIssuedCertStatByOperatorYear(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                IQueryable<IssuedCertStatByOperatorYearVM> query = from certificate in DbContext.Certificates
                                                                   where ((certificate.ExpiredDate >= DateTime.Now) && (operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                                                                   group certificate by new { Year = certificate.IssuedDate.Year.ToString(), OperatorID = certificate.OperatorID } into ItemGroup
                                                                   select new IssuedCertStatByOperatorYearVM()
                                                                   {
                                                                       Year = ItemGroup.Key.Year,
                                                                       OperatorID = ItemGroup.Key.OperatorID,
                                                                       IssuedCertificates = ItemGroup.Count(_ => true)
                                                                   };
                return query;
            }
            else
            {
                IQueryable<IssuedCertStatByOperatorYearVM> query = from certificate in DbContext.Certificates
                                                                   where ((operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                                                                   group certificate by new { Year = certificate.IssuedDate.Year.ToString(), OperatorID = certificate.OperatorID } into ItemGroup
                                                                   select new IssuedCertStatByOperatorYearVM()
                                                                   {
                                                                       Year = ItemGroup.Key.Year.ToString(),
                                                                       OperatorID = ItemGroup.Key.OperatorID,
                                                                       IssuedCertificates = ItemGroup.Count(_ => true)
                                                                   };
                return query;
            }
        }
        public IEnumerable<ExpiredCertStatByOperatorYearVM> GetExpiredCertStatByOperatorYear(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                IQueryable<ExpiredCertStatByOperatorYearVM> query = from certificate in DbContext.Certificates
                                                                    where ((certificate.ExpiredDate >= DateTime.Now) && (operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                                                                    group certificate by new { certificate.ExpiredDate.Year, certificate.OperatorID } into ItemGroup
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
                IQueryable<ExpiredCertStatByOperatorYearVM> query = from certificate in DbContext.Certificates
                                                                    where ((operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                                                                    group certificate by new { certificate.ExpiredDate.Year, certificate.OperatorID } into ItemGroup
                                                                    select new ExpiredCertStatByOperatorYearVM()
                                                                    {
                                                                        Year = ItemGroup.Key.Year.ToString(),
                                                                        OperatorID = ItemGroup.Key.OperatorID,
                                                                        ExpiredInYearCertificates = ItemGroup.Count()
                                                                    };
                return query;
            }
        }

        public IEnumerable<CertStatVM> GetStatCertByCity(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                IQueryable<CertStatVM> query = from certificate in DbContext.Certificates
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
                IQueryable<CertStatVM> query = from certificate in DbContext.Certificates
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
                IQueryable<CertStatByOperatorVM> query = from certificate in DbContext.Certificates
                                                         where ((certificate.ExpiredDate >= DateTime.Now) && (operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                                                         group certificate by certificate.OperatorID into ItemGroup
                                                         select new CertStatByOperatorVM()
                                                         {
                                                             OperatorID = ItemGroup.Key.ToString(),
                                                             ValidCertificates = ItemGroup.Count(),
                                                             ExpiredInYearCertificates = ItemGroup.Where(x => x.ExpiredDate.Year == DateTime.Now.Year && x.ExpiredDate >= DateTime.Now).Count()
                                                         };
                return query;
            }
            else
            {
                IQueryable<CertStatByOperatorVM> query = from certificate in DbContext.Certificates
                                                         where ((operatorID == CommonConstants.SelectAll || certificate.OperatorID == operatorID) && (cityID == CommonConstants.SelectAll || certificate.CityID == cityID))
                                                         group certificate by certificate.OperatorID into ItemGroup
                                                         select new CertStatByOperatorVM()
                                                         {
                                                             OperatorID = ItemGroup.Key.ToString(),
                                                             ValidCertificates = ItemGroup.Count(),
                                                             ExpiredInYearCertificates = ItemGroup.Where(x => x.ExpiredDate.Year == DateTime.Now.Year && x.ExpiredDate >= DateTime.Now).Count()
                                                         };
                return query;
            }
        }

        public IEnumerable<CertStatVM> GetCerStatByLab(string operatorID = CommonConstants.SelectAll, string cityID = CommonConstants.SelectAll, bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                IQueryable<CertStatVM> query = from certificate in DbContext.Certificates
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
                IQueryable<CertStatVM> query = from certificate in DbContext.Certificates
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
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<CertStatVM>("GetCertificateStatistic @fromDate,@toDate", parameters);
        }

        public IEnumerable<ShortCertificate> GetShortCertificate(int year)
        {
            IQueryable<ShortCertificate> query = from certificate in DbContext.Certificates
                                                 where certificate.IssuedDate.Year == year
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
            IQueryable<ShortCertificate> query = from certificate in DbContext.Certificates
                                                 where certificate.ExpiredDate <= DateTime.Now
                                                 select new ShortCertificate()
                                                 {
                                                     Id = certificate.Id,
                                                     Year = certificate.IssuedDate.Year,
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

            IEnumerable<Certificate> query2 = from item1 in query1
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

            IEnumerable<Certificate> query2 = from item1 in query1
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
                                         group certificate by certificate.IssuedDate.Year into OperatorGroup
                                         select OperatorGroup.Key.ToString();
            return query1;
        }

        public IEnumerable<IssuedCertStatByOperatorCityVM> GetIssuedCertStatByOperatorCity(bool onlyIsValid = false)
        {
            if (onlyIsValid)
            {
                IQueryable<IssuedCertStatByOperatorCityVM> query = from certificate in DbContext.Certificates
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
                IQueryable<IssuedCertStatByOperatorCityVM> query = from certificate in DbContext.Certificates
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

        //public IEnumerable<StatBtsInProcessVm> GetStatBtsInProcess()
        //{
        //    var query = from bts in DbContext.Btss
        //                group bts by new { bts.OperatorID } into ItemGroup
        //                select new StatBtsInProcessVm()
        //                {
        //                    OperatorID = ItemGroup.Key.OperatorID,
        //                    Btss = ItemGroup.Count()
        //                };
        //    return query;
        //}

        public IEnumerable<StatBtsVm> GetStatAllBtsInProcess()
        {
            IEnumerable<StatBtsInReceiptVm> query1 = from proApp in (from pro in DbContext.Profiles
                                                                     join app in DbContext.Applicants
                                                                     on pro.ApplicantID equals app.Id
                                                                     select new { pro, app })
                                                     group proApp by new { proApp.app.OperatorID } into ItemGroup
                                                     select new StatBtsInReceiptVm()
                                                     {
                                                         OperatorID = ItemGroup.Key.OperatorID,
                                                         NoAnnounceFeeBtss = ItemGroup.Where(a => a.pro.FeeAnnounceDate == null).Count() == 0 ? 0 : ItemGroup.Where(a => a.pro.FeeAnnounceDate == null).Sum(y => y.pro.BtsQuantity),
                                                         NoReceiptFeeBtss = ItemGroup.Where(x => !string.IsNullOrEmpty(x.pro.FeeAnnounceNum) && (x.pro.FeeReceiptDate == null)).Count() == 0 ? 0 : ItemGroup.Where(x => !string.IsNullOrEmpty(x.pro.FeeAnnounceNum) && (x.pro.FeeReceiptDate == null)).Sum(y => y.pro.BtsQuantity)
                                                     };

            IEnumerable<StatBtsInProcessVm> query2 = from btspro in (from bts in DbContext.Btss
                                                                     join pro in DbContext.Profiles
                                                                     on bts.ProfileID equals pro.Id
                                                                     where pro.FeeReceiptDate != null
                                                                     select bts)
                                                     group btspro by new { btspro.OperatorID } into ItemGroup
                                                     select new StatBtsInProcessVm()
                                                     {
                                                         OperatorID = ItemGroup.Key.OperatorID,
                                                         Btss = ItemGroup.Count()
                                                     };

            IEnumerable<StatBtsVm> query3 = query1.FullOuterJoin(
                query2, left => left.OperatorID, right => right.OperatorID, (left, right) => new StatBtsVm
                {
                    OperatorID = left == null ? right.OperatorID : left.OperatorID,
                    NoAnnounceFeeBtss = left == null ? 0 : left.NoAnnounceFeeBtss,
                    NoReceiptFeeBtss = left == null ? 0 : left.NoReceiptFeeBtss,
                    ReceiptFeeBtss = right == null ? 0 : right.Btss
                });

            return query3.Where(x => x.NoAnnounceFeeBtss > 0 || x.NoReceiptFeeBtss > 0 || x.ReceiptFeeBtss > 0);
        }
    }
}