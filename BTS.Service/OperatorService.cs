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
        void Delete(string ID);
        IEnumerable<Operator> getAll(out int totalRow);
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

        public void Delete(string ID)
        {
            _operatorRepository.Delete(ID);
        }

        public IEnumerable<Operator> getAll(out int totalRow)
        {
            var result = _operatorRepository.GetAll();
            totalRow = result.Count();
            return result;
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
