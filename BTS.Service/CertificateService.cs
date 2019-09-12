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

        SubBtsInCert Add(SubBtsInCert subBtsInCert);

        void Update(Certificate btsCertificate);

        void Delete(string Id);

        void DeleteSubBTSinCert(string Id);

        IEnumerable<Certificate> getAll(out int totalRow, bool onlyValidCertificate, DateTime startDate, DateTime endDate);
        IEnumerable<ReportTT18Cert> getReportTT18Cert(out int totalRows, DateTime startDate, DateTime endDate);
        IEnumerable<Certificate> getAll(out int totalRow, bool onlyValidCertificate);
        Certificate findCertificate(string BtsCode, string ProfileID);

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

        IEnumerable<Certificate> getCertificateProfile(string profileID);

        Certificate getByID(string Id);

        List<SubBtsInCert> getDetailByID(string Id);

        IEnumerable<string> getIssueYears();

        void SaveChanges();
    }

    public class CertificateService : ICertificateService
    {
        private ICertificateRepository _CertificateRepository;
        private INoCertificateRepository _NoCertificateRepository;
        private ISubBTSinCertRepository _SubBTSinCertRepository;
        private IUnitOfWork _unitOfWork;

        public CertificateService(ICertificateRepository certificateRepository, INoCertificateRepository noCertificateRepository, ISubBTSinCertRepository subBTSinCertRepository, IUnitOfWork unitOfWork)
        {
            _CertificateRepository = certificateRepository;
            _NoCertificateRepository = noCertificateRepository;
            _SubBTSinCertRepository = subBTSinCertRepository;
            _unitOfWork = unitOfWork;
        }

        public Certificate Add(Certificate btsCertificate)
        {
            return _CertificateRepository.Add(btsCertificate);
        }

        public SubBtsInCert Add(SubBtsInCert subBtsInCert)
        {
            return _SubBTSinCertRepository.Add(subBtsInCert);
        }

        public void Delete(string Id)
        {
            _SubBTSinCertRepository.DeleteMulti(x => x.CertificateID == Id);
            _CertificateRepository.Delete(Id);
        }

        public void DeleteSubBTSinCert(string Id)
        {
            _SubBTSinCertRepository.DeleteMulti(x => x.CertificateID == Id);
        }

        public IEnumerable<ReportTT18Cert> getReportTT18Cert(out int totalRows, DateTime startDate, DateTime endDate)
        {
            IEnumerable<ReportTT18Cert> result;
            result = _CertificateRepository.GetReportTT18CertByDate(startDate, endDate);        
            totalRows = result.Count();
            return result;
        }

        public IEnumerable<Certificate> getAll(out int totalRows, bool onlyValidCertificate, DateTime startDate, DateTime endDate)
        {
            IEnumerable<Certificate> result;
            if (onlyValidCertificate)
            {
                result = _CertificateRepository.GetMulti(x => x.ExpiredDate >= DateTime.Today && x.IssuedDate >= startDate && x.IssuedDate <= endDate);
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
                result = _CertificateRepository.GetMulti(x => x.ExpiredDate >= DateTime.Today);
            }
            else
            {
                result = _CertificateRepository.GetMulti(x => true);
            }

            totalRows = result.Count();
            return result;
        }

        public Certificate findCertificate(string BtsCode, string ProfileID)
        {
            return _CertificateRepository.GetSingleByCondition(x => x.BtsCode == BtsCode && x.ProfileID == ProfileID);
        }

        public IEnumerable<Certificate> getByBTSCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _CertificateRepository.GetMultiPagingByBtsCode(btsCode, out totalRow, pageIndex, pageSize, false);
        }

        public IEnumerable<Certificate> getByCity(string cityID, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _CertificateRepository.GetMultiPaging(x => x.CityID == cityID, out totalRow, pageIndex, pageSize);
        }

        public Certificate getByID(string Id)
        {
            return _CertificateRepository.GetSingleById(Id);
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

        public void SaveChanges()
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

        public IEnumerable<Certificate> getCertificateProfile(string profileID)
        {
            return _CertificateRepository.GetMulti(x => x.ProfileID == profileID);
        }

        public List<SubBtsInCert> getDetailByID(string CertId)
        {
            return _SubBTSinCertRepository.GetMulti(x => x.CertificateID == CertId).OrderBy(x => x.Id).ToList();
        }
    }
}