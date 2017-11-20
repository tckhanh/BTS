using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTS.Common.ViewModels;
using BTS.Data.Repositories;
using BTS.Data.Repository;

namespace BTS.Service
{
    public interface IStatisticService
    {
        IEnumerable<CertificateStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate);

        IEnumerable<StatisticCertificate> GetStatisticCertificateByYear();

        IEnumerable<StatisticCertificate> GetStatisticCertificateByOperatorCity();
    }

    public class StatisticService : IStatisticService
    {
        private ICertificateRepository _btsCertificateRepository;

        public StatisticService(ICertificateRepository btsCertificateRepository)
        {
            _btsCertificateRepository = btsCertificateRepository;
        }

        public IEnumerable<CertificateStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate)
        {
            return _btsCertificateRepository.GetStatistic(fromDate, toDate);
        }

        public IEnumerable<StatisticCertificate> GetStatisticCertificateByYear()
        {
            return _btsCertificateRepository.GetStatisticCertificateByYear();
        }

        public IEnumerable<StatisticCertificate> GetStatisticCertificateByOperatorCity()
        {
            return _btsCertificateRepository.GetStatisticCertificateByOperatorCity();
        }
    }
}