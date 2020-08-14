using BTS.Data.Logs;
using BTS.Data.Logs;
using BTS.Data.Repositories;
using BTS.Model.Models;

namespace BTS.Service
{
    public interface IErrorService
    {
        Error Create(Error error);

        void Save();
    }

    public class ErrorService : IErrorService
    {
        private IErrorRepository _errorRepository;
        private ILogUnitOfWork _unitOfWork;

        public ErrorService(IErrorRepository errorRepository, ILogUnitOfWork unitOfWork)
        {
            this._errorRepository = errorRepository;
            this._unitOfWork = unitOfWork;
        }

        public Error Create(Error error)
        {
            return _errorRepository.Add(error);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}