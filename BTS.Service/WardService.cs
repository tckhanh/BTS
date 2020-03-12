using BTS.Data.Infrastructure;
using BTS.Data.Repository;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Service
{
    public interface IWardService
    {
        Ward Add(Ward newWard);

        void Update(Ward newWard);

        Ward Delete(string Id);

        IEnumerable<Ward> getAll();

        IEnumerable<Ward> getAll(string keyword);

        Ward getByID(string Id);

        bool IsUsed(string Id);

        void Save();
    }

    public class WardService : IWardService
    {
        private IWardRepository _districtRepository;
        private IUnitOfWork _unitOfWork;

        public WardService(IWardRepository districtRepository, IUnitOfWork unitOfWork)
        {
            this._districtRepository = districtRepository;
            this._unitOfWork = unitOfWork;
        }

        public Ward Add(Ward newWard)
        {
            return _districtRepository.Add(newWard);
        }

        public Ward Delete(string Id)
        {
            return _districtRepository.Delete(Id);
        }

        public IEnumerable<Ward> getAll()
        {
            return _districtRepository.GetAll();
        }

        public IEnumerable<Ward> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _districtRepository.GetMulti(x => x.Id.Contains(keyword) || x.Name.Contains(keyword));
            else
                return _districtRepository.GetAll();
        }

        public Ward getByID(string Id)
        {
            return _districtRepository.GetSingleById(Id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Ward newWard)
        {
            _districtRepository.Update(newWard);
        }

        public bool IsUsed(string Id)
        {
            return _districtRepository.IsUsed(Id);
        }
    }
}