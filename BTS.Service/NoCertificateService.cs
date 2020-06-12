using BTS.Data.Infrastructure;
using BTS.Data.Repository;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Service
{
    public interface INoCertificateService
    {
        NoCertificate getByID(string Id);

        NoCertificate Add(NoCertificate btsNoCertificate);

        void Update(NoCertificate btsNoCertificate);

        void Delete(string Id);

        IEnumerable<NoCertificate> getAll(out int totalRow, bool onlyValidNoCertificate, DateTime startDate, DateTime endDate);

        IEnumerable<NoCertificate> getAll(out int totalRow);

        IEnumerable<NoCertificate> getNoCertificateByProfile(string profileID);

        IEnumerable<NoCertificate> getNoCertificateByCity(string cityID);

        IEnumerable<NoCertificate> getNoCertificateByOperator(string OperatorID);

        IEnumerable<NoCertificate> getNoCertificateByBtsCodeOrAddress(string BtsCodeOrAddress);

        IEnumerable<ReportTT18NoCert> getReportTT18NoCert(out int totalRows, DateTime startDate, DateTime endDate);

        void SaveChanges();
    }

    public class NoCertificateService : INoCertificateService
    {
        private INoCertificateRepository _NoCertificateRepository;
        private IUnitOfWork _unitOfWork;

        public NoCertificateService(INoCertificateRepository noCertificateRepository, IUnitOfWork unitOfWork)
        {
            this._NoCertificateRepository = noCertificateRepository;
            this._unitOfWork = unitOfWork;
        }

        public NoCertificate Add(NoCertificate btsNoCertificate)
        {
            return _NoCertificateRepository.Add(btsNoCertificate);
        }

        public void Delete(string Id)
        {
            _NoCertificateRepository.Delete(Id);
        }

        public IEnumerable<NoCertificate> getAll(out int totalRows, bool onlyValidNoCertificate, DateTime startDate, DateTime endDate)
        {
            IEnumerable<NoCertificate> result;
            if (onlyValidNoCertificate)
            {
                result = _NoCertificateRepository.GetMulti(x => x.TestReportDate >= DateTime.Today && x.TestReportDate >= startDate && x.TestReportDate <= endDate);
            }
            else
            {
                result = _NoCertificateRepository.GetMulti(x => x.TestReportDate >= startDate && x.TestReportDate <= endDate);
            }

            totalRows = result.Count();
            return result;
        }

        public IEnumerable<NoCertificate> getAll(out int totalRow)
        {
            IEnumerable<NoCertificate> result = _NoCertificateRepository.GetMulti(x => true);
            totalRow = result.Count();
            return result;
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(NoCertificate btsNoCertificate)
        {
            _NoCertificateRepository.Update(btsNoCertificate);
        }

        public IEnumerable<NoCertificate> getNoCertificateByProfile(string profileID)
        {
            return _NoCertificateRepository.GetMulti(x => x.ProfileID == profileID);
        }

        public IEnumerable<NoCertificate> getNoCertificateByCity(string cityID)
        {
            return _NoCertificateRepository.GetMulti(x => x.CityID == cityID);
        }

        public IEnumerable<NoCertificate> getNoCertificateByOperator(string OperatorID)
        {
            return _NoCertificateRepository.GetMulti(x => x.OperatorID == OperatorID);
        }

        public IEnumerable<NoCertificate> getNoCertificateByBtsCodeOrAddress(string BtsCodeOrAddress)
        {
            return _NoCertificateRepository.GetMulti(x => x.BtsCode.ToLower().Contains(BtsCodeOrAddress) || x.Address.ToLower().Contains(BtsCodeOrAddress));
        }
        

        public IEnumerable<ReportTT18NoCert> getReportTT18NoCert(out int totalRows, DateTime startDate, DateTime endDate)
        {
            IEnumerable<ReportTT18NoCert> result;
            result = _NoCertificateRepository.GetReportTT18NoCertByDate(startDate, endDate);
            totalRows = result.Count();
            return result;
        }

        public NoCertificate getByID(string Id)
        {
            return _NoCertificateRepository.GetSingleById(Id);
        }
    }
}