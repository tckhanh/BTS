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

        InCaseOf Delete(int Id);

        IEnumerable<InCaseOf> getAll();

        IEnumerable<InCaseOf> getAll(string keyword);

        InCaseOf getByID(string Id);

        InCaseOf getByID(int Id);

        bool IsUsed(int Id);

        void Save();
    }

    public class InCaseOfService : IInCaseOfService
    {
        private IInCaseOfRepository _inCaseOfRepository;
        private IUnitOfWork _unitOfWork;

        public InCaseOfService(IInCaseOfRepository inCaseOfRepository, IUnitOfWork unitOfWork)
        {
            _inCaseOfRepository = inCaseOfRepository;
            _unitOfWork = unitOfWork;
        }

        public InCaseOf Add(InCaseOf newInCaseOf)
        {
            return _inCaseOfRepository.Add(newInCaseOf);
        }

        public InCaseOf Delete(int Id)
        {
            return _inCaseOfRepository.Delete(Id);
        }

        public IEnumerable<InCaseOf> getAll()
        {
            return _inCaseOfRepository.GetAll();
        }

        public IEnumerable<InCaseOf> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _inCaseOfRepository.GetMulti(x => x.Id.ToString().Contains(keyword) || x.Name.Contains(keyword));
            else
                return _inCaseOfRepository.GetAll();
        }

        public InCaseOf getByID(string Id)
        {
            return _inCaseOfRepository.GetSingleById(Id);
        }

        public InCaseOf getByID(int Id)
        {
            return _inCaseOfRepository.GetSingleById(Id);
        }

        public bool IsUsed(int Id)
        {
            return _inCaseOfRepository.IsUsed(Id);
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