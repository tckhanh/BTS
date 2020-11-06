using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTS.Data.Repository
{
    public interface INoRequiredBtsRepository : IRepository<NoRequiredBts>
    {
        IEnumerable<ReportTT18NoCert> GetReportTT18NoCertByDate(DateTime fromDate, DateTime toDate);
    }

    public class NoRequiredBtsRepository : RepositoryBase<NoRequiredBts>, INoRequiredBtsRepository
    {
        public NoRequiredBtsRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<ReportTT18NoCert> GetReportTT18NoCertByDate(DateTime fromDate, DateTime toDate)
        {
            IQueryable<ReportTT18NoCert> query1 = from noRequiredBts in DbContext.NoRequiredBtss
                                                where ((noRequiredBts.AnnouncedDate >= fromDate) && (noRequiredBts.AnnouncedDate >= fromDate))
                                                select new ReportTT18NoCert()
                                                {
                                                    OperatorID = noRequiredBts.OperatorID,
                                                    BtsCode = noRequiredBts.BtsCode,
                                                    Address = noRequiredBts.Address,
                                                    CityID = noRequiredBts.CityID,
                                                    Longtitude = noRequiredBts.Longtitude,
                                                    Latitude = noRequiredBts.Latitude,
                                                    SubBtsCode = noRequiredBts.BtsCode,
                                                    SubOperatorID = noRequiredBts.OperatorID,
                                                    //SubBtsQuantity = noRequiredBts.SubBtsQuantity,
                                                    //Manufactory = noRequiredBts.Manufactory,
                                                    //Equipment = noRequiredBts.Equipment,
                                                    //AntenNum = noRequiredBts.AntenNum,
                                                    //Configuration = noRequiredBts.Configuration,
                                                    //PowerSum = noRequiredBts.PowerSum,
                                                    //Band = noRequiredBts.Band,
                                                    //AntenHeight = noRequiredBts.AntenHeight,
                                                    Id = noRequiredBts.Id,
                                                    ProfileID = noRequiredBts.AnnouncedDoc,
                                                    TestReportDate = noRequiredBts.AnnouncedDate,
                                                };
            return query1;
        }
    }
}