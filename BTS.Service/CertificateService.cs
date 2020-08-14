using BTS.Data.Infrastructure;
using BTS.Data.Repository;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTS.Service
{
    public interface ICertificateService
    {
        Certificate Add(Certificate btsCertificate);

        SubBtsInCert Add(SubBtsInCert subBtsInCert);

        void Update(Certificate btsCertificate);

        void Delete(string Id);

        void DeleteSubBTSinCert(string Id);

        IEnumerable<Certificate> getAll(out int totalRow, bool onlyValidCertificate, DateTime startDate, DateTime endDate, string[] includes = null);
        IEnumerable<Certificate> getAll(out int totalRow, bool onlyValidCertificate, string[] includes = null);
        IEnumerable<ReportTT18Cert> getReportTT18Cert(out int totalRows, DateTime startDate, DateTime endDate);
        Certificate findCertificate(string BtsCode, string ProfileID);

        IEnumerable<Certificate> getByBTSCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Certificate> getByCity(string cityID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Certificate> getByOperator(string cityID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Certificate> getCertificateByYear(int year, int page, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Certificate> getCertificateByYear(int year, string[] includes = null);

        IEnumerable<Certificate> getCertificateByBTSCode(string btsCode, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Certificate> getCertificateByBtsCodeOrAddress(string btsCodeOrAddress, string[] includes = null);

        IEnumerable<Certificate> getCertificateByCertificateNum(string CertificateNum, string[] includes = null);

        IEnumerable<Certificate> getCertificateByCity(string cityID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Certificate> getCertificateByCity(string cityID, string[] includes = null);

        IEnumerable<Certificate> getCertificateByOperator(string operatorID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Certificate> getCertificateByOperator(string operatorID, string[] includes = null);

        IEnumerable<Certificate> getCertificateByProfile(string profileID, string[] includes = null);

        Certificate getByID(string Id, string[] includes = null);

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

        public IEnumerable<Certificate> getAll(out int totalRows, bool onlyValidCertificate, DateTime startDate, DateTime endDate, string[] includes = null)
        {
            IEnumerable<Certificate> result;
            if (onlyValidCertificate)
            {
                result = _CertificateRepository.GetMulti(x => x.ExpiredDate >= DateTime.Today && x.IssuedDate >= startDate && x.IssuedDate <= endDate, includes);
            }
            else
            {
                result = _CertificateRepository.GetMulti(x => x.IssuedDate >= startDate && x.IssuedDate <= endDate, includes);
            }

            totalRows = result.Count();
            return result;
        }

        public IEnumerable<Certificate> getAll(out int totalRows, bool onlyValidCertificate, string[] includes = null)
        {
            IEnumerable<Certificate> result;
            if (onlyValidCertificate)
            {
                result = _CertificateRepository.GetMulti(x => x.ExpiredDate >= DateTime.Today, includes);
            }
            else
            {
                result = _CertificateRepository.GetMulti(x => true, includes);
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

        public IEnumerable<Certificate> getByCity(string cityID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10)
        {
            return _CertificateRepository.GetMultiPaging(x => x.CityID == cityID, out totalRow, includes, pageIndex, pageSize);
        }

        public Certificate getByID(string Id, string[] includes = null)
        {
            return _CertificateRepository.GetSingleByCondition(x => x.Id == Id, includes);
        }

        public IEnumerable<Certificate> getByOperator(string operatorID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10)
        {
            return _CertificateRepository.GetMultiPaging(x => x.OperatorID == operatorID, out totalRow, includes, pageIndex, pageSize);
        }

        public IEnumerable<Certificate> getCertificateByYear(int year, int page, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10)
        {
            return _CertificateRepository.GetMultiPaging(x => x.IssuedDate != null && Convert.ToDateTime(x.IssuedDate).Year == year, out totalRow,includes, pageIndex, pageSize);
        }

        public IEnumerable<Certificate> getCertificateByBTSCode(string btsCode, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10)
        {
            var query = _CertificateRepository.GetMultiByBtsCode(btsCode, true);
            totalRow = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Certificate> getCertificateByCity(string cityID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10)
        {
            return _CertificateRepository.GetMultiPaging(x => x.CityID == cityID, out totalRow, includes, pageIndex, pageSize);
        }

        public IEnumerable<Certificate> getCertificateByOperator(string operatorID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10)
        {
            return _CertificateRepository.GetMultiPaging(x => x.OperatorID == operatorID, out totalRow, includes, pageIndex, pageSize);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Certificate btsCertificate)
        {
            _CertificateRepository.Update(btsCertificate);
        }

        public IEnumerable<Certificate> getCertificateByYear(int year, string[] includes = null)
        {
            return _CertificateRepository.GetMulti(x => x.IssuedDate != null && Convert.ToDateTime(x.IssuedDate).Year == year, includes);
        }

        public IEnumerable<Certificate> getCertificateByCity(string cityID, string[] includes = null)
        {
            return _CertificateRepository.GetMulti(x => x.CityID == cityID, includes);
        }

        public IEnumerable<Certificate> getCertificateByCertificateNum(string CertificateNum, string[] includes = null)
        {
            return _CertificateRepository.GetMulti(x => x.Id.ToUpper().Contains(CertificateNum), includes);
        }


        public IEnumerable<Certificate> getCertificateByBtsCodeOrAddress(string btsCodeOrAddress, string[] includes = null)
        {
            return _CertificateRepository.GetMulti(x => x.BtsCode.ToLower().Contains(btsCodeOrAddress) || x.Address.ToLower().Contains(btsCodeOrAddress), includes);
        }

        public IEnumerable<Certificate> getCertificateByOperator(string operatorID, string[] includes = null)
        {
            return _CertificateRepository.GetMulti(x => x.OperatorID == operatorID, includes);
        }

        public IEnumerable<string> getIssueYears()
        {
            return _CertificateRepository.GetIssueYears();
        }

        public IEnumerable<Certificate> getCertificateByProfile(string profileID, string[] includes = null)
        {
            return _CertificateRepository.GetMulti(x => x.ProfileID == profileID, includes);
        }

        public List<SubBtsInCert> getDetailByID(string CertId)
        {
            return _SubBTSinCertRepository.GetMulti(x => x.CertificateID == CertId).OrderBy(x => x.Id).ToList();
        }
    }
}