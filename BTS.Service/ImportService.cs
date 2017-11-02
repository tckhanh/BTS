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
        bool Add(InCaseOf item);
        bool Add(Lab item);
        bool Add(City item);
        bool Add(Operator item);
        bool Add(Applicant item);
        void Update(InCaseOf newInCaseOf);
        void Save();
    }

    public class ImportService : IImportService
    {
        IInCaseOfRepository _inCaseOfRepository;
        ILabRepository _labRepository;
        ICityRepository _cityRepository;
        public IOperatorRepository _operatorRepository;
        IApplicantRepository _applicantRepository;
        IUnitOfWork _unitOfWork;


        public ImportService(IInCaseOfRepository inCaseOfRepository, ILabRepository labRepository, ICityRepository cityRepository, IOperatorRepository operatorRepository, IApplicantRepository applicantRepository, IUnitOfWork unitOfWork)
        {
            this._inCaseOfRepository = inCaseOfRepository;
            this._labRepository = labRepository;
            this._cityRepository = cityRepository;
            this._operatorRepository = operatorRepository;
            this._applicantRepository = applicantRepository;
            this._unitOfWork = unitOfWork;
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

    }
}
