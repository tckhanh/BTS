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
        IEnumerable<Operator> GetOperator();

        IEnumerable<City> GetCity();

        IEnumerable<IssuedCertStatByOperatorYearVM> GetIssuedCertStatByOperatorYear();

        IEnumerable<IssuedCertStatByOperatorCityVM> GetIssuedCertStatByOperatorCity();

        IEnumerable<ExpiredCertStatByOperatorYearVM> GetExpiredCertStatByOperatorYear();

        IEnumerable<CertStatByOperatorVM> GetCertStatByOperator();

        IEnumerable<StatBtsVm> GetStatAllBtsInProcess();
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

        public IEnumerable<ExpiredCertStatByOperatorYearVM> GetExpiredCertStatByOperatorYear()
        {
            return _certificateRepository.GetExpiredCertStatByOperatorYear();
        }

        public IEnumerable<IssuedCertStatByOperatorYearVM> GetIssuedCertStatByOperatorYear()
        {
            return _certificateRepository.GetIssuedCertStatByOperatorYear();
        }

        public IEnumerable<Operator> GetOperator()
        {
            return _operatorRepository.GetAll();
        }

        public IEnumerable<City> GetCity()
        {
            return _cityRepository.GetAll();
        }

        public IEnumerable<CertStatByOperatorVM> GetCertStatByOperator()
        {
            return _certificateRepository.GetCertStatByOperator();
        }

        public IEnumerable<IssuedCertStatByOperatorCityVM> GetIssuedCertStatByOperatorCity()
        {
            return _certificateRepository.GetIssuedCertStatByOperatorCity(true);
        }

        IEnumerable<StatBtsVm> IStatisticService.GetStatAllBtsInProcess()
        {
            return _certificateRepository.GetStatAllBtsInProcess();
        }
    }
}