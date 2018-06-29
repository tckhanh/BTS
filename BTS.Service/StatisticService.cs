using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTS.Common.ViewModels;
using BTS.Data.Repositories;
using BTS.Data.Repository;
using BTS.Model.Models;

namespace BTS.Service
{
    public interface IStatisticService
    {
        IEnumerable<CertificateStatiticsViewModel> GetRevenueStatistic(string fromDate, string toDate);

        IEnumerable<Operator> GetOperator();

        IEnumerable<City> GetCity();

        IEnumerable<CertificateStatiticsViewModel> GetCertificateStatisticByYear();

        IEnumerable<CertificateStatiticsViewModel> GetCertificateStatisticByOperator();

        IEnumerable<CertificateStatiticsViewModel> GetCertificateStatisticByOperatorCity();
    }

    public class StatisticService : IStatisticService
    {
        private ICertificateRepository _certificateRepository;
        private ISubBTSinCertRepository _subBTSinCertRepository;
        private IOperatorRepository _operatorRepository;
        private ICityRepository _cityRepository;

        public StatisticService(ICertificateRepository certificateRepository, IOperatorRepository operatorRepository, ICityRepository cityRepository, ISubBTSinCertRepository subBTSinCertRepository)
        {
            _certificateRepository = certificateRepository;
            _subBTSinCertRepository = subBTSinCertRepository;
            _operatorRepository = operatorRepository;
            _cityRepository = cityRepository;
        }

        public IEnumerable<CertificateStatiticsViewModel> GetRevenueStatistic(string fromDate, string toDate)
        {
            return _certificateRepository.GetStatistic(fromDate, toDate);
        }

        public IEnumerable<CertificateStatiticsViewModel> GetCertificateStatisticByYear()
        {
            return _certificateRepository.GetCertificateStatisticByYear();
        }

        public IEnumerable<CertificateStatiticsViewModel> GetCertificateStatisticByOperatorCity()
        {
            return _certificateRepository.GetCertificateStatisticByOperatorCity();
        }

        public IEnumerable<Operator> GetOperator()
        {
            return _operatorRepository.GetAll();
        }

        public IEnumerable<City> GetCity()
        {
            return _cityRepository.GetAll();
        }

        public IEnumerable<CertificateStatiticsViewModel> GetCertificateStatisticByOperator()
        {
            return _certificateRepository.GetCertificateStatisticByOperator();
        }
    }
}