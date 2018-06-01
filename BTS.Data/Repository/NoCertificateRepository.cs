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
    public interface INoCertificateRepository : IRepository<NoCertificate>
    {
    }

    public class NoCertificateRepository : RepositoryBase<NoCertificate>, INoCertificateRepository
    {
        public NoCertificateRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}