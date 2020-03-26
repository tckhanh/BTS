using BTS.Data.Infrastructure;
using BTS.Data.Repository;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Service
{
    public interface IDistrictService
    {
        District Add(District newDistrict);

        void Update(District newDistrict);

        District Delete(string Id);

        IEnumerable<District> getAll(string[] includes = null);

        IEnumerable<District> getAll(string keyword);

        District getByID(string Id);

        bool IsUsed(string Id);

        void Save();
    }

    public class DistrictService : IDistrictService
    {
        private IDistrictRepository _districtRepository;
        private IUnitOfWork _unitOfWork;

        public DistrictService(IDistrictRepository districtRepository, IUnitOfWork unitOfWork)
        {
            this._districtRepository = districtRepository;
            this._unitOfWork = unitOfWork;
        }

        public District Add(District newDistrict)
        {
            return _districtRepository.Add(newDistrict);
        }

        public District Delete(string Id)
        {
            return _districtRepository.Delete(Id);
        }

        public IEnumerable<District> getAll(string[] includes = null)
        {
            return _districtRepository.GetAll(includes);
        }

        public IEnumerable<District> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _districtRepository.GetMulti(x => x.Id.Contains(keyword) || x.Name.Contains(keyword));
            else
                return _districtRepository.GetAll();
        }

        public District getByID(string Id)
        {
            return _districtRepository.GetSingleById(Id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(District newDistrict)
        {
            _districtRepository.Update(newDistrict);
        }

        public bool IsUsed(string Id)
        {
            return _districtRepository.IsUsed(Id);
        }
    }
}