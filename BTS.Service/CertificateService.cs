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
    public interface ICertificateService
    {
        Certificate Add(Certificate btsCertificate);

        void Update(Certificate btsCertificate);

        void Delete(int ID);

        IEnumerable<Certificate> getAll(out int totalRow, bool onlyValidCertificate, DateTime startDate, DateTime endDate);

        IEnumerable<Certificate> getAll(out int totalRow, bool onlyValidCertificate);

        IEnumerable<Certificate> getByBTSCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Certificate> getByCity(string cityID, out int totalRow, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Certificate> getByOperator(string cityID, out int totalRow, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Certificate> getCertificateByYear(int year, int page, out int totalRow, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Certificate> getCertificateByYear(int year);

        IEnumerable<Certificate> getCertificateByBTSCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Certificate> getCertificateByCity(string cityID, out int totalRow, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Certificate> getCertificateByCity(string cityID);

        IEnumerable<Certificate> getCertificateByOperator(string operatorID, out int totalRow, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Certificate> getCertificateByOperator(string operatorID);

        IEnumerable<Certificate> getCertificateProfile(int profileID);

        Certificate getByID(string ID);

        IEnumerable<string> getIssueYears();

        void Save();
    }

    public class CertificateService : ICertificateService
    {
        private ICertificateRepository _CertificateRepository;
        private IUnitOfWork _unitOfWork;

        public CertificateService(ICertificateRepository certificateRepository, IUnitOfWork unitOfWork)
        {
            this._CertificateRepository = certificateRepository;
            this._unitOfWork = unitOfWork;
        }

        public Certificate Add(Certificate btsCertificate)
        {
            return _CertificateRepository.Add(btsCertificate);
        }

        public void Delete(int ID)
        {
            _CertificateRepository.Delete(ID);
        }

        public IEnumerable<Certificate> getAll(out int totalRows, bool onlyValidCertificate, DateTime startDate, DateTime endDate)
        {
            IEnumerable<Certificate> result;
            if (onlyValidCertificate)
            {
                result = _CertificateRepository.GetMulti(x => x.ExpiredDate > DateTime.Today && x.IssuedDate >= startDate && x.IssuedDate <= endDate);
            }
            else
            {
                result = _CertificateRepository.GetMulti(x => x.IssuedDate >= startDate && x.IssuedDate <= endDate);
            }

            totalRows = result.Count();
            return result;
        }

        public IEnumerable<Certificate> getAll(out int totalRows, bool onlyValidCertificate)
        {
            IEnumerable<Certificate> result;
            if (onlyValidCertificate)
            {
                result = _CertificateRepository.GetMulti(x => x.ExpiredDate > DateTime.Today);
            }
            else
            {
                result = _CertificateRepository.GetMulti(x => true);
            }

            totalRows = result.Count();
            return result;
        }

        public IEnumerable<Certificate> getByBTSCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _CertificateRepository.GetMultiPagingByBtsCode(btsCode, out totalRow, pageIndex, pageSize, false);
        }

        public IEnumerable<Certificate> getByCity(string cityID, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _CertificateRepository.GetMultiPaging(x => x.CityID == cityID, out totalRow, pageIndex, pageSize);
        }

        public Certificate getByID(string ID)
        {
            return _CertificateRepository.GetSingleById(ID);
        }

        public IEnumerable<Certificate> getByOperator(string operatorID, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _CertificateRepository.GetMultiPaging(x => x.OperatorID == operatorID, out totalRow, pageIndex, pageSize);
        }

        public IEnumerable<Certificate> getCertificateByYear(int year, int page, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _CertificateRepository.GetMultiPaging(x => x.IssuedDate != null && Convert.ToDateTime(x.IssuedDate).Year == year, out totalRow, pageIndex, pageSize);
        }

        public IEnumerable<Certificate> getCertificateByBTSCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            var query = _CertificateRepository.GetMultiByBtsCode(btsCode, true);
            totalRow = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Certificate> getCertificateByCity(string cityID, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _CertificateRepository.GetMultiPaging(x => x.CityID == cityID, out totalRow, pageIndex, pageSize);
        }

        public IEnumerable<Certificate> getCertificateByOperator(string operatorID, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _CertificateRepository.GetMultiPaging(x => x.OperatorID == operatorID, out totalRow, pageIndex, pageSize);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Certificate btsCertificate)
        {
            _CertificateRepository.Update(btsCertificate);
        }

        public IEnumerable<Certificate> getCertificateByYear(int year)
        {
            return _CertificateRepository.GetMulti(x => x.IssuedDate != null && Convert.ToDateTime(x.IssuedDate).Year == year);
        }

        public IEnumerable<Certificate> getCertificateByCity(string cityID)
        {
            return _CertificateRepository.GetMulti(x => x.CityID == cityID);
        }

        public IEnumerable<Certificate> getCertificateByOperator(string operatorID)
        {
            return _CertificateRepository.GetMulti(x => x.OperatorID == operatorID);
        }

        public IEnumerable<string> getIssueYears()
        {
            return _CertificateRepository.GetIssueYears();
        }

        public IEnumerable<Certificate> getCertificateProfile(int profileID)
        {
            return _CertificateRepository.GetMulti(x => x.ProfileID == profileID);
        }
    }
}