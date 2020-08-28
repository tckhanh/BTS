using BTS.Data.Infrastructure;
using BTS.Data.Repository;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Service
{
    public interface ISubBtsInCertService
    {
        SubBtsInCert Add(SubBtsInCert newSubBtsInCert);

        void Update(SubBtsInCert newSubBtsInCert);

        SubBtsInCert Delete(string Id);

        IEnumerable<SubBtsInCert> getAll(string[] includes = null);

        IEnumerable<SubBtsInCert> getAll(string keyword);

        SubBtsInCert getByID(string Id);

        bool IsUsed(string Id);

        void Save();
    }

    public class SubBtsInCertService : ISubBtsInCertService
    {
        private ISubBtsInCertRepository _SubBtsInCertRepository;
        private IUnitOfWork _unitOfWork;

        public SubBtsInCertService(ISubBtsInCertRepository SubBtsInCertRepository, IUnitOfWork unitOfWork)
        {
            this._SubBtsInCertRepository = SubBtsInCertRepository;
            this._unitOfWork = unitOfWork;
        }

        public SubBtsInCert Add(SubBtsInCert newSubBtsInCert)
        {
            return _SubBtsInCertRepository.Add(newSubBtsInCert);
        }

        public SubBtsInCert Delete(string Id)
        {
            return _SubBtsInCertRepository.Delete(Id);
        }

        public IEnumerable<SubBtsInCert> getAll(string[] includes = null)
        {
            return _SubBtsInCertRepository.GetAll(includes);
        }

        public IEnumerable<SubBtsInCert> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _SubBtsInCertRepository.GetMulti(x => x.CertificateID.Contains(keyword) || x.BtsCode.Contains(keyword));
            else
                return _SubBtsInCertRepository.GetAll();
        }

        public SubBtsInCert getByID(string Id)
        {
            return _SubBtsInCertRepository.GetSingleById(Id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(SubBtsInCert newSubBtsInCert)
        {
            _SubBtsInCertRepository.Update(newSubBtsInCert);
        }

        public bool IsUsed(string Id)
        {
            // Chua cai dat
            return true;
        }
    }
}