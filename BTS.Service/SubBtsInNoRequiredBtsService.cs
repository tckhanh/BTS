using BTS.Data.Infrastructure;
using BTS.Data.Repository;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Service
{
    public interface ISubBtsInNoRequiredBtsService
    {
        SubBtsInNoRequiredBts Add(SubBtsInNoRequiredBts newSubBtsInNoRequiredBts);

        void Update(SubBtsInNoRequiredBts newSubBtsInNoRequiredBts);

        SubBtsInNoRequiredBts Delete(string Id);

        IEnumerable<SubBtsInNoRequiredBts> getAll(string[] includes = null);

        IEnumerable<SubBtsInNoRequiredBts> getAll(string keyword);

        SubBtsInNoRequiredBts getByID(string Id);

        bool IsUsed(string Id);

        void Save();
    }

    public class SubBtsInNoRequiredBtsService : ISubBtsInNoRequiredBtsService
    {
        private ISubBtsInNoRequiredBtsRepository _SubBtsInNoRequiredBtsRepository;
        private IUnitOfWork _unitOfWork;

        public SubBtsInNoRequiredBtsService(ISubBtsInNoRequiredBtsRepository SubBtsInNoRequiredBtsRepository, IUnitOfWork unitOfWork)
        {
            this._SubBtsInNoRequiredBtsRepository = SubBtsInNoRequiredBtsRepository;
            this._unitOfWork = unitOfWork;
        }

        public SubBtsInNoRequiredBts Add(SubBtsInNoRequiredBts newSubBtsInNoRequiredBts)
        {
            return _SubBtsInNoRequiredBtsRepository.Add(newSubBtsInNoRequiredBts);
        }

        public SubBtsInNoRequiredBts Delete(string Id)
        {
            return _SubBtsInNoRequiredBtsRepository.Delete(Id);
        }

        public IEnumerable<SubBtsInNoRequiredBts> getAll(string[] includes = null)
        {
            return _SubBtsInNoRequiredBtsRepository.GetAll(includes);
        }

        public IEnumerable<SubBtsInNoRequiredBts> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _SubBtsInNoRequiredBtsRepository.GetMulti(x => x.Equipment.Contains(keyword) || x.BtsCode.Contains(keyword));
            else
                return _SubBtsInNoRequiredBtsRepository.GetAll();
        }

        public SubBtsInNoRequiredBts getByID(string Id)
        {
            return _SubBtsInNoRequiredBtsRepository.GetSingleById(Id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(SubBtsInNoRequiredBts newSubBtsInNoRequiredBts)
        {
            _SubBtsInNoRequiredBtsRepository.Update(newSubBtsInNoRequiredBts);
        }

        public bool IsUsed(string Id)
        {
            // Chua cai dat
            return true;
        }
    }
}