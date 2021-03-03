using BTS.Data.Logs;
using BTS.Data.Logs;
using BTS.Data.Repositories;
using BTS.Model.Models;
using System.Collections.Generic;

namespace BTS.Service
{
    public interface IAuditService
    {
        Audit Add(Audit newAudit);

        void Update(Audit newAudit);

        Audit Delete(int Id);

        IEnumerable<Audit> getAll();

        IEnumerable<Audit> getAll(string keyword);

        Audit getByID(string Id);

        Audit getByID(int Id);

        Audit Create(Audit error);

        void Save();
    }

    public class AuditService : IAuditService
    {
        private IAuditRepository _errorRepository;
        private ILogUnitOfWork _unitOfWork;

        public AuditService(IAuditRepository errorRepository, ILogUnitOfWork unitOfWork)
        {
            this._errorRepository = errorRepository;
            this._unitOfWork = unitOfWork;
        }

        public Audit Add(Audit newAudit)
        {
            return _errorRepository.Add(newAudit);
        }

        public Audit Delete(int Id)
        {
            return _errorRepository.Delete(Id);
        }

        public IEnumerable<Audit> getAll()
        {
            return _errorRepository.GetAll();
        }

        public IEnumerable<Audit> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _errorRepository.GetMulti(x => x.Id.ToString().Contains(keyword) || x.UserName.Contains(keyword));
            else
                return _errorRepository.GetAll();
        }

        public Audit getByID(string Id)
        {
            return _errorRepository.GetSingleById(Id);
        }

        public Audit getByID(int Id)
        {
            return _errorRepository.GetSingleById(Id);
        }

        public void Update(Audit newAudit)
        {
            _errorRepository.Update(newAudit);
        }

        public Audit Create(Audit error)
        {
            return _errorRepository.Add(error);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}