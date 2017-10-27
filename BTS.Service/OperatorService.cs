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

    public interface IOperatorService
    {

        Operator Add(Operator newOperator);
        void Update(Operator newOperator);
        Operator Delete(string ID);
        IEnumerable<Operator> getAll();
        IEnumerable<Operator> getAll(string keyword);
        Operator getByID(string ID);
        void Save();
    }
    public class OperatorService : IOperatorService
    {
        IOperatorRepository _operatorRepository;
        IUnitOfWork _unitOfWork;

        public OperatorService(IOperatorRepository operatorRepository, IUnitOfWork unitOfWork)
        {
            this._operatorRepository = operatorRepository;
            this._unitOfWork = unitOfWork;
        }

        public Operator Add(Operator newOperator)
        {
            return _operatorRepository.Add(newOperator);
        }

        public Operator Delete(string ID)
        {
            return _operatorRepository.Delete(ID);
        }

        public IEnumerable<Operator> getAll()
        {
            return _operatorRepository.GetAll();
        }

        public IEnumerable<Operator> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _operatorRepository.GetMulti(x => x.ID.Contains(keyword) || x.Name.Contains(keyword));
            else
                return _operatorRepository.GetAll();
        }

        public Operator getByID(string ID)
        {
            return _operatorRepository.GetSingleById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Operator newOperator)
        {
            _operatorRepository.Update(newOperator);
        }
    }
}
