using BTS.Data.Infrastructure;
using BTS.Data.Repository;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Service
{
    public interface ICityService
    {
        City Add(City newCity);

        void Update(City newCity);

        City Delete(string ID);

        IEnumerable<City> getAll();

        IEnumerable<City> getAll(string keyword);

        City getByID(string ID);

        void Save();
    }

    public class CityService : ICityService
    {
        private ICityRepository _cityRepository;
        private IUnitOfWork _unitOfWork;

        public CityService(ICityRepository cityRepository, IUnitOfWork unitOfWork)
        {
            this._cityRepository = cityRepository;
            this._unitOfWork = unitOfWork;
        }

        public City Add(City newCity)
        {
            return _cityRepository.Add(newCity);
        }

        public City Delete(string ID)
        {
            return _cityRepository.Delete(ID);
        }

        public IEnumerable<City> getAll()
        {
            return _cityRepository.GetAll();
        }

        public IEnumerable<City> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _cityRepository.GetMulti(x => x.ID.Contains(keyword) || x.Name.Contains(keyword));
            else
                return _cityRepository.GetAll();
        }

        public City getByID(string ID)
        {
            return _cityRepository.GetSingleById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(City newCity)
        {
            _cityRepository.Update(newCity);
        }
    }
}