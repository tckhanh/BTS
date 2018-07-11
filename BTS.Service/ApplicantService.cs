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
    public interface IApplicantService
    {
        Applicant Add(Applicant newApplicant);

        void Update(Applicant newApplicant);

        Applicant Delete(string ID);

        IEnumerable<Applicant> getAll();

        IEnumerable<Applicant> getAll(string keyword);

        IEnumerable<Operator> GetAllOperator();

        Applicant getByID(string ID);

        bool IsUsed(string ID);

        void SaveChanges();
    }

    public class ApplicantService : IApplicantService
    {
        private IApplicantRepository _applicantRepository;
        private IUnitOfWork _unitOfWork;

        public ApplicantService(IApplicantRepository applicantRepository, IUnitOfWork unitOfWork)
        {
            this._applicantRepository = applicantRepository;
            this._unitOfWork = unitOfWork;
        }

        public Applicant Add(Applicant newApplicant)
        {
            return _applicantRepository.Add(newApplicant);
        }

        public Applicant Delete(string ID)
        {
            return _applicantRepository.Delete(ID);
        }

        public IEnumerable<Applicant> getAll()
        {
            return _applicantRepository.GetAll();
        }

        public IEnumerable<Applicant> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _applicantRepository.GetMulti(x => x.ID.Contains(keyword) || x.Name.Contains(keyword));
            else
                return _applicantRepository.GetAll();
        }

        public Applicant getByID(string ID)
        {
            return _applicantRepository.GetSingleById(ID);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Applicant newApplicant)
        {
            _applicantRepository.Update(newApplicant);
        }

        public bool IsUsed(string ID)
        {
            return _applicantRepository.IsUsed(ID);
        }

        public IEnumerable<Operator> GetAllOperator()
        {
            return _applicantRepository.GetAllOperator();
        }
    }
}