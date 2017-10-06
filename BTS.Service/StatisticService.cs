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

    }
    public class StatisticService : IStatisticService
    {
        IBTSCertificateRepository _btsCertificateRepository;
        //public StatisticService(IOrderRepository orderRepository)
        //{
        //    _orderRepository = orderRepository;
        //}
        public IEnumerable<CertificateStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate)
        {
            return _btsCertificateRepository.GetStatistic(fromDate, toDate);
        }
    }
}
