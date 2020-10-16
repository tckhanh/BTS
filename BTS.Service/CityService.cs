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
    public interface ICityService
    {
        City Add(City newCity);

        void Update(City newCity);

        City Delete(string Id);

        IEnumerable<City> getAll(string[] includes = null);

        IEnumerable<City> getAll(string keyword);

        City getByID(string Id);

        bool IsUsed(string Id);

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

        public City Delete(string Id)
        {
            return _cityRepository.Delete(Id);
        }

        public IEnumerable<City> getAll(string[] includes = null)
        {
            return _cityRepository.GetAll(includes).OrderBy(x => x.Name);
        }

        public IEnumerable<City> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _cityRepository.GetMulti(x => x.Id.Contains(keyword) || x.Name.Contains(keyword)).OrderBy(x => x.Name);
            else
                return _cityRepository.GetAll().OrderBy(x => x.Name);
        }

        public City getByID(string Id)
        {
            return _cityRepository.GetSingleById(Id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(City newCity)
        {
            _cityRepository.Update(newCity);
        }

        public bool IsUsed(string Id)
        {
            return _cityRepository.IsUsed(Id);
        }
    }
}