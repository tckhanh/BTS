﻿using BTS.Common.ViewModels;
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

        IEnumerable<CertificateStatisticViewModel> IBTSCertificateRepository.GetStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<CertificateStatisticViewModel>("GetCertificateStatistic @fromDate,@toDate", parameters);
        }
    }
}
