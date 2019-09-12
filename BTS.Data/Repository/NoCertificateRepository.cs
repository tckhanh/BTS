using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTS.Data.Repository
{
    public interface INoCertificateRepository : IRepository<NoCertificate>
    {
        IEnumerable<ReportTT18NoCert> GetReportTT18NoCertByDate(DateTime fromDate, DateTime toDate);
    }

    public class NoCertificateRepository : RepositoryBase<NoCertificate>, INoCertificateRepository
    {
        public NoCertificateRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<ReportTT18NoCert> GetReportTT18NoCertByDate(DateTime fromDate, DateTime toDate)
        {
            IQueryable<ReportTT18NoCert> query1 = from noCertificate in DbContext.NoCertificates
                                                where ((noCertificate.TestReportDate >= fromDate) && (noCertificate.TestReportDate >= fromDate))
                                                select new ReportTT18NoCert()
                                                {
                                                    OperatorID = noCertificate.OperatorID,
                                                    BtsCode = noCertificate.BtsCode,
                                                    Address = noCertificate.Address,
                                                    CityID = noCertificate.CityID,
                                                    Longtitude = noCertificate.Longtitude,
                                                    Latitude = noCertificate.Latitude,
                                                    SubBtsCode = noCertificate.BtsCode,
                                                    SubOperatorID = noCertificate.OperatorID,
                                                    //SubBtsQuantity = noCertificate.SubBtsQuantity,
                                                    //Manufactory = noCertificate.Manufactory,
                                                    //Equipment = noCertificate.Equipment,
                                                    //AntenNum = noCertificate.AntenNum,
                                                    //Configuration = noCertificate.Configuration,
                                                    //PowerSum = noCertificate.PowerSum,
                                                    //Band = noCertificate.Band,
                                                    //AntenHeight = noCertificate.AntenHeight,
                                                    Id = noCertificate.Id,
                                                    ProfileID = noCertificate.ProfileID,
                                                    LabID = noCertificate.LabID,
                                                    TestReportNo = noCertificate.TestReportNo,
                                                    TestReportDate = noCertificate.TestReportDate,
                                                    ReasonNoCertificate = noCertificate.ReasonNoCertificate
                                                };
            return query1;
        }
    }
}