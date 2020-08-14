using BTS.Data.Logs;
using BTS.Data.Repositories;
using BTS.Data.Repository;
using BTS.Model.Models;

namespace BTS.Service
{
    public interface IAuditService
    {
        Audit Create(Audit audit);

        void Save();
    }

    public class AuditService : IAuditService
    {
        private IAuditRepository _auditRepository;
        private ILogUnitOfWork _unitOfWork;

        public AuditService(IAuditRepository auditRepository, ILogUnitOfWork unitOfWork)
        {
            this._auditRepository = auditRepository;
            this._unitOfWork = unitOfWork;
        }

        public Audit Create(Audit audit)
        {
            return _auditRepository.Add(audit);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}