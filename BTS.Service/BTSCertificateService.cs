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
    public interface IBTSCertificateService
    {
        BTSCertificate Add(BTSCertificate btsCertificate);
        void Update(BTSCertificate btsCertificate);
        void Delete(int ID);        
        IEnumerable<BTSCertificate> getAll(out int totalRow, int pageIndex = 1, int pageSize = 10);
        IEnumerable<BTSCertificate> getByBTSCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10);
        IEnumerable<BTSCertificate> getByCity(string cityID, out int totalRow, int pageIndex = 1, int pageSize = 10);
        IEnumerable<BTSCertificate> getByOperator(string cityID, out int totalRow, int pageIndex = 1, int pageSize = 10);
        IEnumerable<BTSCertificate> getCertificateByYear(int year, int page, out int totalRow, int pageIndex = 1, int pageSize = 10);
        IEnumerable<BTSCertificate> getCertificateByBTSCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10);
        IEnumerable<BTSCertificate> getCertificateByCity(string cityID, out int totalRow, int pageIndex = 1, int pageSize = 10);               
        IEnumerable<BTSCertificate> getCertificateByOperator(string operatorID, out int totalRow, int pageIndex = 1, int pageSize = 10);
        BTSCertificate getByID(int ID);
        void Save();
    }
    public class BTSCertificateService : IBTSCertificateService
    {
        IBTSCertificateRepository _BTSCertificateRepository;
        IUnitOfWork _unitOfWork;

        public BTSCertificateService(IBTSCertificateRepository btsCertificateRepository, IUnitOfWork unitOfWork)
        {
            this._BTSCertificateRepository = btsCertificateRepository;
            this._unitOfWork = unitOfWork;
        }

        public BTSCertificate Add(BTSCertificate btsCertificate)
        {
            return _BTSCertificateRepository.Add(btsCertificate);
        }

        public void Delete(int ID)
        {
            _BTSCertificateRepository.Delete(ID);
        }

        public IEnumerable<BTSCertificate> getAll(out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            var result = _BTSCertificateRepository.GetMultiPaging(x => true, out totalRow, pageIndex, pageSize, null);
            return result;
        }

        public IEnumerable<BTSCertificate> getByBTSCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _BTSCertificateRepository.GetMultiPagingByBtsCode(btsCode, out totalRow, pageIndex, pageSize, false);
        }

        public IEnumerable<BTSCertificate> getByCity(string cityID, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _BTSCertificateRepository.GetMultiPaging(x => x.CityID == cityID, out totalRow, pageIndex, pageSize);
        }

        public BTSCertificate getByID(int ID)
        {
            return _BTSCertificateRepository.GetSingleById(ID);
        }

        public IEnumerable<BTSCertificate> getByOperator(string operatorID, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _BTSCertificateRepository.GetMultiPaging(x => x.OperatorID == operatorID, out totalRow, pageIndex, pageSize);
        }

        public IEnumerable<BTSCertificate> getCertificateByYear(int year, int page, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _BTSCertificateRepository.GetMultiPaging(x => x.IssuedDate != null && Convert.ToDateTime(x.IssuedDate).Year == year, out totalRow, pageIndex, pageSize);
        }

        public IEnumerable<BTSCertificate> getCertificateByBTSCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            var query = _BTSCertificateRepository.GetMultiByBtsCode(btsCode, true).Where(x=>x.CertificateNum!=null);
            totalRow = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<BTSCertificate> getCertificateByCity(string cityID, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _BTSCertificateRepository.GetMultiPaging(x => x.CityID == cityID && x.CertificateNum != null, out totalRow, pageIndex, pageSize);
        }

        public IEnumerable<BTSCertificate> getCertificateByOperator(string operatorID, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _BTSCertificateRepository.GetMultiPaging(x => x.OperatorID == operatorID && x.CertificateNum != null, out totalRow, pageIndex, pageSize);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(BTSCertificate btsCertificate)
        {
            _BTSCertificateRepository.Update(btsCertificate);
        }
        
    }
}
