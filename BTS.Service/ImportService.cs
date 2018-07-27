using BTS.Common;
using BTS.Data.Infrastructure;
using BTS.Data.Repository;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Service
{
    public interface IImportService
    {
        Profile findProfile(string applicantID, string profileNum, DateTime profileDate);

        Profile getProfile(int Id);

        Bts findBts(int profileID, string btsCode);

        SubBtsInCert findSubBts(string certificateID, string btsCode, string operatorID);

        Certificate findCertificate(string Id);

        NoCertificate findNoCertificate(string BtsCode, int ProfileID);

        bool Add(InCaseOf item);

        bool Add(Lab item);

        bool Add(City item);

        bool Add(Operator item);

        bool Add(Applicant item);

        bool Add(Profile item);

        bool Add(Bts item);

        bool Add(Certificate item);

        bool Add(NoCertificate item);

        bool Add(SubBtsInCert item);

        void Update(InCaseOf newInCaseOf);

        void Update(Profile newProfile);

        void Update(Bts newBts);

        void Update(Certificate newCertificate);

        void Delete(Bts bts);

        void RemoveSubBtsInCert(string CertificateID);

        void Save();

        Applicant getApplicant(int proFileID);

        IEnumerable<string> getLastOwnCertificateIDs(string btsCode, string operatorID);

        IEnumerable<string> getLastNoOwnCertificateIDs(string btsCode, string operatorID);

        IEnumerable<Certificate> getLastOwnCertificates(string btsCode, string operatorID);

        IEnumerable<Certificate> getLastNoOwnCertificates(string btsCode, string operatorID);

        Certificate getLastOwnCertificate(string btsCode, string operatorID);

        Certificate getLastNoOwnCertificate(string btsCode, string operatorID);

        IEnumerable<Profile> findProfilesBtsInProcess(string btsCode, string operatorID);

        IEnumerable<Profile> findProfilestNoCertificate(string btsCode, string operatorID);

        IEnumerable<NoCertificate> findBtsNoCertificate(string btsCode, string operatorID);
    }

    public class ImportService : IImportService
    {
        private IInCaseOfRepository _inCaseOfRepository;
        private ILabRepository _labRepository;
        private ICityRepository _cityRepository;
        private IOperatorRepository _operatorRepository;
        private IApplicantRepository _applicantRepository;
        private IProfileRepository _profileRepository;
        private IBtsRepository _btsRepository;
        private ICertificateRepository _certificateRepository;
        private INoCertificateRepository _noCertificateRepository;
        private ISubBTSinCertRepository _subBTSinCertRepository;
        private IUnitOfWork _unitOfWork;

        public ImportService(IInCaseOfRepository inCaseOfRepository, ILabRepository labRepository, ICityRepository cityRepository, IOperatorRepository operatorRepository, IApplicantRepository applicantRepository, IProfileRepository profileRepository, IBtsRepository btsRepository, ICertificateRepository certificateRepository, INoCertificateRepository noCertificateRepository, ISubBTSinCertRepository subBTSinCertRepository, IUnitOfWork unitOfWork)
        {
            _inCaseOfRepository = inCaseOfRepository;
            _labRepository = labRepository;
            _cityRepository = cityRepository;
            _operatorRepository = operatorRepository;
            _applicantRepository = applicantRepository;
            _profileRepository = profileRepository;
            _btsRepository = btsRepository;
            _certificateRepository = certificateRepository;
            _noCertificateRepository = noCertificateRepository;
            _subBTSinCertRepository = subBTSinCertRepository;

            _unitOfWork = unitOfWork;
        }

        public bool Add(InCaseOf item)
        {
            if (_inCaseOfRepository.GetSingleById(item.Id) == null)
            {
                item.CreatedDate = DateTime.Now;
                _inCaseOfRepository.Add(item);
            }
            return true;
        }

        public bool Add(Lab item)
        {
            if (_labRepository.GetSingleById(item.Id) == null)
            {
                item.CreatedDate = DateTime.Now;
                _labRepository.Add(item);
            }
            return true;
        }

        public bool Add(City item)
        {
            if (_cityRepository.GetSingleById(item.Id) == null)
            {
                item.CreatedDate = DateTime.Now;
                _cityRepository.Add(item);
            }
            return true;
        }

        public bool Add(Operator item)
        {
            if (_operatorRepository.GetSingleById(item.Id) == null)
            {
                item.CreatedDate = DateTime.Now;
                _operatorRepository.Add(item);
            }
            return true;
        }

        public bool Add(Applicant item)
        {
            if (_applicantRepository.GetSingleById(item.Id) == null)
            {
                item.CreatedDate = DateTime.Now;
                _applicantRepository.Add(item);
            }
            return true;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public bool Add(Profile item)
        {
            if (_profileRepository.GetSingleById(item.Id) == null)
            {
                _profileRepository.Add(item);
            }
            return true;
        }

        public bool Add(Bts item)
        {
            if (_btsRepository.GetSingleById(item.Id) == null)
            {
                _btsRepository.Add(item);
            }
            return true;
        }

        public bool Add(Certificate item)
        {
            if (_certificateRepository.GetSingleById(item.Id) == null)
            {
                _certificateRepository.Add(item);
            }
            return true;
        }

        public bool Add(NoCertificate item)
        {
            if (_noCertificateRepository.GetSingleById(item.Id) == null)
            {
                _noCertificateRepository.Add(item);
            }
            return true;
        }

        public bool Add(SubBtsInCert item)
        {
            if (_subBTSinCertRepository.GetSingleById(item.Id) == null)
            {
                _subBTSinCertRepository.Add(item);
            }
            return true;
        }

        public void Update(Profile item)
        {
            item.UpdatedDate = DateTime.Now;
            _profileRepository.Update(item);
        }

        public void Update(Bts item)
        {
            item.UpdatedDate = DateTime.Now;
            _btsRepository.Update(item);
        }

        public void Update(InCaseOf item)
        {
            item.UpdatedDate = DateTime.Now;
            _inCaseOfRepository.Update(item);
        }

        public void Update(Certificate item)
        {
            item.UpdatedDate = DateTime.Now;
            _certificateRepository.Update(item);
        }

        public Profile findProfile(string applicantID, string profileNum, DateTime profileDate)
        {
            return _profileRepository.findProfile(applicantID, profileNum, profileDate);
        }

        public Profile getProfile(int Id)
        {
            return _profileRepository.GetSingleById(Id);
        }

        public Bts findBts(int profileID, string btsCode)
        {
            return _btsRepository.GetSingleByCondition(x => x.ProfileID == profileID && x.BtsCode == btsCode);
        }

        public Certificate findCertificate(string Id)
        {
            return _certificateRepository.GetSingleByCondition(x => x.Id == Id);
        }

        public NoCertificate findNoCertificate(string BtsCode, int ProfileID)
        {
            return _noCertificateRepository.GetSingleByCondition(x => x.BtsCode == BtsCode && x.ProfileID == ProfileID);
        }

        public SubBtsInCert findSubBts(string certificateID, string btsCode, string operatorID)
        {
            return _subBTSinCertRepository.GetSingleByCondition(x => x.CertificateID == certificateID && x.BtsCode == btsCode && x.OperatorID == operatorID);
        }

        public void RemoveSubBtsInCert(string CertificateID)
        {
            _subBTSinCertRepository.DeleteMulti(x => x.CertificateID == CertificateID);
        }

        public Applicant getApplicant(int proFileID)
        {
            Profile profileItem = _profileRepository.GetSingleByCondition(x => x.Id == proFileID);
            return _applicantRepository.GetSingleByCondition(x => x.Id == profileItem.ApplicantID);
        }

        public IEnumerable<string> getLastOwnCertificateIDs(string btsCode, string operatorID)
        {
            return _certificateRepository.getLastOwnCertificates(btsCode, operatorID).Select(x => x.Id);
        }

        public IEnumerable<string> getLastNoOwnCertificateIDs(string btsCode, string operatorID)
        {
            return _certificateRepository.getLastNoOwnCertificates(btsCode, operatorID).Select(x => x.Id);
        }

        public IEnumerable<Certificate> getLastOwnCertificates(string btsCode, string operatorID)
        {
            return _certificateRepository.getLastOwnCertificates(btsCode, operatorID);
        }

        public IEnumerable<Certificate> getLastNoOwnCertificates(string btsCode, string operatorID)
        {
            return _certificateRepository.getLastNoOwnCertificates(btsCode, operatorID);
        }

        public Certificate getLastOwnCertificate(string btsCode, string operatorID)
        {
            return _certificateRepository.getLastOwnCertificates(btsCode, operatorID).FirstOrDefault();
        }

        public Certificate getLastNoOwnCertificate(string btsCode, string operatorID)
        {
            return _certificateRepository.getLastNoOwnCertificates(btsCode, operatorID).FirstOrDefault();
        }

        public void Delete(Bts bts)
        {
            _btsRepository.Delete(bts);
        }

        public IEnumerable<Profile> findProfilesBtsInProcess(string btsCode, string operatorID)
        {
            return _profileRepository.findProfilesBtsInProcess(btsCode, operatorID);
        }

        public IEnumerable<Profile> findProfilestNoCertificate(string btsCode, string operatorID)
        {
            return _profileRepository.findProfilesBTSNoCertificate(btsCode, operatorID);
        }

        public IEnumerable<NoCertificate> findBtsNoCertificate(string btsCode, string operatorID)
        {
            return _noCertificateRepository.GetMulti(x => x.BtsCode == btsCode && x.OperatorID == operatorID);
        }
    }
}