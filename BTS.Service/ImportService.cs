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

        Bts findBts(int profileID, string btsCode);

        Certificate findCertificate(string ID);

        bool Add(InCaseOf item);

        bool Add(Lab item);

        bool Add(City item);

        bool Add(Operator item);

        bool Add(Applicant item);

        bool Add(Profile item);

        bool Add(Bts item);

        bool Add(Certificate item);

        bool Add(SubBtsInCert item);

        void Update(InCaseOf newInCaseOf);

        void Update(Profile newProfile);

        void Update(Bts newBts);

        void Update(Certificate newCertificate);

        void RemoveSubBtsInCert(string CertificateID);

        void Save();
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
        private ISubBTSinCertRepository _subBTSinCertRepository;
        private IUnitOfWork _unitOfWork;

        public ImportService(IInCaseOfRepository inCaseOfRepository, ILabRepository labRepository, ICityRepository cityRepository, IOperatorRepository operatorRepository, IApplicantRepository applicantRepository, IProfileRepository profileRepository, IBtsRepository btsRepository, ICertificateRepository certificateRepository, ISubBTSinCertRepository subBTSinCertRepository, IUnitOfWork unitOfWork)
        {
            _inCaseOfRepository = inCaseOfRepository;
            _labRepository = labRepository;
            _cityRepository = cityRepository;
            _operatorRepository = operatorRepository;
            _applicantRepository = applicantRepository;
            _profileRepository = profileRepository;
            _btsRepository = btsRepository;
            _certificateRepository = certificateRepository;
            _subBTSinCertRepository = subBTSinCertRepository;

            _unitOfWork = unitOfWork;
        }

        public bool Add(InCaseOf item)
        {
            if (_inCaseOfRepository.GetSingleById(item.ID) == null)
            {
                _inCaseOfRepository.Add(item);
            }
            return true;
        }

        public bool Add(Lab item)
        {
            if (_labRepository.GetSingleById(item.ID) == null)
            {
                _labRepository.Add(item);
            }
            return true;
        }

        public bool Add(City item)
        {
            if (_cityRepository.GetSingleById(item.ID) == null)
            {
                _cityRepository.Add(item);
            }
            return true;
        }

        public bool Add(Operator item)
        {
            if (_operatorRepository.GetSingleById(item.ID) == null)
            {
                _operatorRepository.Add(item);
            }
            return true;
        }

        public bool Add(Applicant item)
        {
            if (_applicantRepository.GetSingleById(item.ID) == null)
            {
                _applicantRepository.Add(item);
            }
            return true;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(InCaseOf newInCaseOf)
        {
            _inCaseOfRepository.Update(newInCaseOf);
        }

        public bool Add(Profile item)
        {
            if (_profileRepository.GetSingleById(item.ID) == null)
            {
                _profileRepository.Add(item);
            }
            return true;
        }

        public bool Add(Bts item)
        {
            if (_btsRepository.GetSingleById(item.ID) == null)
            {
                _btsRepository.Add(item);
            }
            return true;
        }

        public bool Add(Certificate item)
        {
            if (_certificateRepository.GetSingleById(item.ID) == null)
            {
                _certificateRepository.Add(item);
            }
            return true;
        }

        public bool Add(SubBtsInCert item)
        {
            if (_subBTSinCertRepository.GetSingleById(item.ID) == null)
            {
                _subBTSinCertRepository.Add(item);
            }
            return true;
        }

        public Profile findProfile(string applicantID, string profileNum, DateTime profileDate)
        {
            return _profileRepository.findProfile(applicantID, profileNum, profileDate);
        }

        public void Update(Profile newProfile)
        {
            _profileRepository.Update(newProfile);
        }

        public void Update(Bts newBts)
        {
            _btsRepository.Update(newBts);
        }

        public Bts findBts(int profileID, string btsCode)
        {
            return _btsRepository.GetSingleByCondition(x => x.ProfileID == profileID && x.BtsCode == btsCode);
        }

        public Certificate findCertificate(string ID)
        {
            return _certificateRepository.GetSingleByCondition(x => x.ID == ID);
        }

        public void Update(Certificate newCertificate)
        {
            _certificateRepository.Update(newCertificate);
        }

        public void RemoveSubBtsInCert(string CertificateID)
        {
            _subBTSinCertRepository.DeleteMulti(x => x.CertificateID == CertificateID);
        }
    }
}