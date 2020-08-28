using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTS.Common;
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

        IEnumerable<StatIssuedCerByOperatorYearVM> GetIssuedStatCerByOperatorYear();
        IEnumerable<StatIssuedCerByAreaYearVM> GetIssuedStatCerByAreaYear();

        IEnumerable<StatIssuedCerByOperatorAreaVM> GetIssuedStatCerByOperatorArea();        
        
        IEnumerable<StatIssuedCerByOperatorAreaVM> GetStatExpiredInYearCerByOperatorArea();

        IEnumerable<StatIssuedCerByOperatorCityVM> GetIssuedStatCerByOperatorCity(string Area = CommonConstants.SelectAll);
        IEnumerable<StatIssuedCerByOperatorCityVM> GetStatInYearCerByOperatorCity(string Area = CommonConstants.SelectAll);

        IEnumerable<StatExpiredCerByOperatorYearVM> GetStatExpiredCerByOperatorYear();

        IEnumerable<StatNearExpiredInYearCerByOperatorCityVM> GetStatNearExpiredInYearCerByOperatorCity(string Area = CommonConstants.SelectAll);
        IEnumerable<StatExpiredCerByOperatorCityVM> GetStatExpiredInYearCerByOperatorCity(string Area = CommonConstants.SelectAll);

        IEnumerable<StatExpiredCerByAreaYearVM> GetStatExpiredCerByAreaYear();

        IEnumerable<StatCoupleCerByOperatorVM> GetStatCoupleCerByOperator();
        IEnumerable<StatCoupleCerByAreaVM> GetStatCoupleCerByArea();
        IEnumerable<StatCerByOperatorVM> GetStatCerByOperator();
        IEnumerable<StatCerByAreaVM> GetStatCerByArea();
        IEnumerable<StatBtsByBandVM> GetStatBtsByBand();

        IEnumerable<StatBtsByOperatorBandVM> GetStatBtsByOperatorBand();
        IEnumerable<StatBtsByAreaBandVM> GetStatBtsByAreaBand();

        IEnumerable<StatBtsByBandCityVM> GetStatBtsByBandCity(string Area = CommonConstants.SelectAll);

        IEnumerable<StatBtsByOperatorCityVM> GetStatBtsByOperatorCity(string Area = CommonConstants.SelectAll);

        IEnumerable<StatBtsByOperatorAreaVM> GetStatBtsByOperatorArea();

        IEnumerable<StatBtsByEquipmentVM> GetStatBtsByEquipment();

        IEnumerable<StatBtsByManufactoryVM> GetStatBtsByManufactory();

        IEnumerable<StatBtsByOperatorManufactoryVM> GetStatBtsByOperatorManufactory();
        IEnumerable<StatBtsByAreaManufactoryVM> GetStatBtsByAreaManufactory();

        IEnumerable<StatBtsVm> GetStatAllBtsInProcess();
    }

    public class StatisticService : IStatisticService
    {
        private ICertificateRepository _certificateRepository;
        private ISubBtsInCertRepository _subBTSinCertRepository;
        private IOperatorRepository _operatorRepository;
        private ICityRepository _cityRepository;

        public StatisticService(ICertificateRepository certificateRepository, IOperatorRepository operatorRepository, ICityRepository cityRepository, ISubBtsInCertRepository subBTSinCertRepository)
        {
            _certificateRepository = certificateRepository;
            _subBTSinCertRepository = subBTSinCertRepository;
            _operatorRepository = operatorRepository;
            _cityRepository = cityRepository;
        }

        public IEnumerable<StatExpiredCerByOperatorYearVM> GetStatExpiredCerByOperatorYear()
        {
            return _certificateRepository.GetStatExpiredCerByOperatorYear();
        }
        public IEnumerable<StatExpiredCerByAreaYearVM> GetStatExpiredCerByAreaYear()
        {
            return _certificateRepository.GetStatExpiredCerByAreaYear();
        }


        public IEnumerable<StatIssuedCerByOperatorYearVM> GetIssuedStatCerByOperatorYear()
        {
            return _certificateRepository.GetIssuedStatCerByOperatorYear();
        }

        public IEnumerable<StatIssuedCerByAreaYearVM> GetIssuedStatCerByAreaYear()
        {
            return _certificateRepository.GetIssuedStatCerByAreaYear();
        }

        public IEnumerable<StatIssuedCerByOperatorAreaVM> GetIssuedStatCerByOperatorArea()
        {
            return _certificateRepository.GetIssuedStatCerByOperatorArea();
        }

        public IEnumerable<StatIssuedCerByOperatorAreaVM> GetStatExpiredInYearCerByOperatorArea()
        {
            return _certificateRepository.GetStatExpiredInYearCerByOperatorArea();
        }

        public IEnumerable<Operator> GetOperator()
        {
            return _operatorRepository.GetAll();
        }

        public IEnumerable<City> GetCity()
        {
            return _cityRepository.GetAll();
        }

        public IEnumerable<StatCoupleCerByOperatorVM> GetStatCoupleCerByOperator()
        {
            return _certificateRepository.GetStatCoupleCerByOperator();
        }

        public IEnumerable<StatCoupleCerByAreaVM> GetStatCoupleCerByArea()
        {
            return _certificateRepository.GetStatCoupleCerByArea();
        }

        public IEnumerable<StatCerByOperatorVM> GetStatCerByOperator()
        {
            return _certificateRepository.GetStatCerByOperator();
        }

        public IEnumerable<StatCerByAreaVM> GetStatCerByArea()
        {
            return _certificateRepository.GetStatCerByArea();
        }

        public IEnumerable<StatBtsByBandVM> GetStatBtsByBand()
        {
            return _subBTSinCertRepository.GetStatBtsByBand();
        }

        public IEnumerable<StatBtsByOperatorBandVM> GetStatBtsByOperatorBand()
        {
            return _subBTSinCertRepository.GetStatBtsByOperatorBand();
        }

        public IEnumerable<StatBtsByAreaBandVM> GetStatBtsByAreaBand()
        {
            return _subBTSinCertRepository.GetStatBtsByAreaBand();
        }

        public IEnumerable<StatBtsByBandCityVM> GetStatBtsByBandCity(string Area = CommonConstants.SelectAll)
        {
            return _subBTSinCertRepository.GetStatBtsByBandCity(Area);
        }

        public IEnumerable<StatBtsByOperatorCityVM> GetStatBtsByOperatorCity(string Area = CommonConstants.SelectAll)
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

        public IEnumerable<StatBtsByAreaManufactoryVM> GetStatBtsByAreaManufactory()
        {
            return _subBTSinCertRepository.GetStatBtsByAreaManufactory();
        }

        public IEnumerable<StatBtsByEquipmentVM> GetStatBtsByEquipment()
        {
            return _subBTSinCertRepository.GetStatBtsByEquipemnt();
        }

        public IEnumerable<StatIssuedCerByOperatorCityVM> GetIssuedStatCerByOperatorCity(string Area = CommonConstants.SelectAll)
        {
            return _certificateRepository.GetIssuedStatCerByOperatorCity(Area, true);
        }

        public IEnumerable<StatIssuedCerByOperatorCityVM> GetStatInYearCerByOperatorCity(string Area = CommonConstants.SelectAll)
        {
            return _certificateRepository.GetStatInYearCerByOperatorCity(Area);
        }

        IEnumerable<StatBtsVm> IStatisticService.GetStatAllBtsInProcess()
        {
            return _certificateRepository.GetStatAllBtsInProcess();
        }

        public IEnumerable<StatNearExpiredInYearCerByOperatorCityVM> GetStatNearExpiredInYearCerByOperatorCity(string Area = "ALL")
        {
            return _certificateRepository.GetStatNearExpiredInYearCerByOperatorCity(Area);
        }
        public IEnumerable<StatExpiredCerByOperatorCityVM> GetStatExpiredInYearCerByOperatorCity(string Area = "ALL")
        {
            return _certificateRepository.GetStatExpiredInYearCerByOperatorCity(Area);
        }

        
    }
}