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
        NoCertificate Add(NoCertificate btsNoCertificate);

        void Update(NoCertificate btsNoCertificate);

        void Delete(int Id);

        IEnumerable<NoCertificate> getAll(out int totalRow, bool onlyValidNoCertificate, DateTime startDate, DateTime endDate);

        IEnumerable<NoCertificate> getAll(out int totalRow, bool onlyValidNoCertificate);

        IEnumerable<NoCertificate> getNoCertificateProfile(string profileID);

        IEnumerable<NoCertificate> getNoCertificateByCity(string cityID);

        void Save();
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

        public void Delete(int Id)
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

        public IEnumerable<NoCertificate> getAll(out int totalRows, bool onlyValidNoCertificate)
        {
            IEnumerable<NoCertificate> result;
            if (onlyValidNoCertificate)
            {
                result = _NoCertificateRepository.GetMulti(x => x.TestReportDate >= DateTime.Today);
            }
            else
            {
                result = _NoCertificateRepository.GetMulti(x => true);
            }

            totalRows = result.Count();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(NoCertificate btsNoCertificate)
        {
            _NoCertificateRepository.Update(btsNoCertificate);
        }

        public IEnumerable<NoCertificate> getNoCertificateProfile(string profileID)
        {
            return _NoCertificateRepository.GetMulti(x => x.ProfileID == profileID);
        }

        public IEnumerable<NoCertificate> getNoCertificateByCity(string cityID)
        {
            return _NoCertificateRepository.GetMulti(x => x.CityID == cityID);
        }
    }
}