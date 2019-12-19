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

        IEnumerable<IssuedStatCerByOperatorYearVM> GetIssuedStatCerByOperatorYear();

        IEnumerable<IssuedStatCerByOperatorAreaVM> GetIssuedStatCerByOperatorArea();        
        
        IEnumerable<IssuedStatCerByOperatorAreaVM> GetIssuedStatExpiredInYearCerByOperatorArea();

        IEnumerable<IssuedStatCerByOperatorCityVM> GetIssuedStatCerByOperatorCity();

        IEnumerable<ExpiredStatCerByOperatorYearVM> GetExpiredStatCerByOperatorYear();

        IEnumerable<StatCerByOperatorVM> GetStatCerByOperator();

        IEnumerable<StatBtsByBandVM> GetStatBtsByBand();

        IEnumerable<StatBtsByOperatorBandVM> GetStatBtsByOperatorBand();

        IEnumerable<StatBtsByBandCityVM> GetStatBtsByBandCity();

        IEnumerable<StatBtsByOperatorCityVM> GetStatBtsByOperatorCity();

        IEnumerable<StatBtsByOperatorAreaVM> GetStatBtsByOperatorArea();

        IEnumerable<StatBtsByEquipmentVM> GetStatBtsByEquipment();

        IEnumerable<StatBtsByManufactoryVM> GetStatBtsByManufactory();

        IEnumerable<StatBtsByOperatorManufactoryVM> GetStatBtsByOperatorManufactory();

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

        public IEnumerable<ExpiredStatCerByOperatorYearVM> GetExpiredStatCerByOperatorYear()
        {
            return _certificateRepository.GetExpiredStatCerByOperatorYear();
        }

        public IEnumerable<IssuedStatCerByOperatorYearVM> GetIssuedStatCerByOperatorYear()
        {
            return _certificateRepository.GetIssuedStatCerByOperatorYear();
        }

        public IEnumerable<IssuedStatCerByOperatorAreaVM> GetIssuedStatCerByOperatorArea()
        {
            return _certificateRepository.GetIssuedStatCerByOperatorArea();
        }

        public IEnumerable<IssuedStatCerByOperatorAreaVM> GetIssuedStatExpiredInYearCerByOperatorArea()
        {
            return _certificateRepository.GetIssuedStatExpiredInYearCerByOperatorArea();
        }

        public IEnumerable<Operator> GetOperator()
        {
            return _operatorRepository.GetAll();
        }

        public IEnumerable<City> GetCity()
        {
            return _cityRepository.GetAll();
        }

        public IEnumerable<StatCerByOperatorVM> GetStatCerByOperator()
        {
            return _certificateRepository.GetStatCerByOperator();
        }

        public IEnumerable<StatBtsByBandVM> GetStatBtsByBand()
        {
            return _subBTSinCertRepository.GetStatBtsByBand();
        }

        public IEnumerable<StatBtsByOperatorBandVM> GetStatBtsByOperatorBand()
        {
            return _subBTSinCertRepository.GetStatBtsByOperatorBand();
        }

        public IEnumerable<StatBtsByBandCityVM> GetStatBtsByBandCity()
        {
            return _subBTSinCertRepository.GetStatBtsByBandCity();
        }

        public IEnumerable<StatBtsByOperatorCityVM> GetStatBtsByOperatorCity()
        {
            return _subBTSinCertRepository.GetStatBtsByOperatorCity();
        }

        public IEnumerable<StatBtsByOperatorAreaVM> GetStatBtsByOperatorArea()
        {
            return _subBTSinCertRepository.GetStatBtsByOperatorArea();
        }

        public IEnumerable<StatBtsByManufactoryVM> GetStatBtsByManufactory()
        {
            return _subBTSinCertRepository.GetStatBtsByManufactory();
        }

        public IEnumerable<StatBtsByOperatorManufactoryVM> GetStatBtsByOperatorManufactory()
        {
            return _subBTSinCertRepository.GetStatBtsByOperatorManufactory();
        }

        public IEnumerable<StatBtsByEquipmentVM> GetStatBtsByEquipment()
        {
            return _subBTSinCertRepository.GetStatBtsByEquipemnt();
        }

        public IEnumerable<IssuedStatCerByOperatorCityVM> GetIssuedStatCerByOperatorCity()
        {
            return _certificateRepository.GetIssuedStatCerByOperatorCity(true);
        }

        IEnumerable<StatBtsVm> IStatisticService.GetStatAllBtsInProcess()
        {
            return _certificateRepository.GetStatAllBtsInProcess();
        }
    }
}