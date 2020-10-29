using BTS.Data.Logs;
using BTS.Data.Logs;
using BTS.Data.Repositories;
using BTS.Model.Models;
using System.Collections.Generic;

namespace BTS.Service
{
    public interface IErrorService
    {
        Error Add(Error newError);

        void Update(Error newError);

        Error Delete(string Id);

        IEnumerable<Error> getAll();

        IEnumerable<Error> getAll(string keyword);

        Error getByID(string Id);

        Error getByID(int Id);

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

        public Error Add(Error newError)
        {
            return _errorRepository.Add(newError);
        }

        public Error Delete(string Id)
        {
            return _errorRepository.Delete(Id);
        }

        public IEnumerable<Error> getAll()
        {
            return _errorRepository.GetAll();
        }

        public IEnumerable<Error> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _errorRepository.GetMulti(x => x.Id.ToString().Contains(keyword) || x.Message.Contains(keyword));
            else
                return _errorRepository.GetAll();
        }

        public Error getByID(string Id)
        {
            return _errorRepository.GetSingleById(Id);
        }

        public Error getByID(int Id)
        {
            return _errorRepository.GetSingleById(Id);
        }

        public void Update(Error newError)
        {
            _errorRepository.Update(newError);
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