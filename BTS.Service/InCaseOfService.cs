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
    public interface IInCaseOfService
    {
        InCaseOf Add(InCaseOf newInCaseOf);

        void Update(InCaseOf newInCaseOf);

        InCaseOf Delete(string ID);

        InCaseOf Delete(int ID);

        IEnumerable<InCaseOf> getAll();

        IEnumerable<InCaseOf> getAll(string keyword);

        InCaseOf getByID(string ID);

        InCaseOf getByID(int ID);

        void Save();
    }

    public class InCaseOfService : IInCaseOfService
    {
        private IInCaseOfRepository _operatorRepository;
        private IUnitOfWork _unitOfWork;

        public InCaseOfService(IInCaseOfRepository operatorRepository, IUnitOfWork unitOfWork)
        {
            this._operatorRepository = operatorRepository;
            this._unitOfWork = unitOfWork;
        }

        public InCaseOf Add(InCaseOf newInCaseOf)
        {
            return _operatorRepository.Add(newInCaseOf);
        }

        public InCaseOf Delete(string ID)
        {
            return _operatorRepository.Delete(ID);
        }

        public InCaseOf Delete(int ID)
        {
            return _operatorRepository.Delete(ID);
        }

        public IEnumerable<InCaseOf> getAll()
        {
            return _operatorRepository.GetAll();
        }

        public IEnumerable<InCaseOf> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _operatorRepository.GetMulti(x => x.ID.ToString().Contains(keyword) || x.Name.Contains(keyword));
            else
                return _operatorRepository.GetAll();
        }

        public InCaseOf getByID(string ID)
        {
            return _operatorRepository.GetSingleById(ID);
        }

        public InCaseOf getByID(int ID)
        {
            return _operatorRepository.GetSingleById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(InCaseOf newInCaseOf)
        {
            _operatorRepository.Update(newInCaseOf);
        }
    }
}