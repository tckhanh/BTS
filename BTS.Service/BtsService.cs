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
    public interface IBtsService
    {
        Bts Add(Bts newBts);

        void Update(Bts newBts);

        Bts Delete(int Id);

        IEnumerable<Bts> getAll();

        IEnumerable<Bts> getAll(string keyword);

        IEnumerable<Operator> getAllOperator();

        IEnumerable<Profile> getAllProfile();

        IEnumerable<Profile> getAllProfileInProcess();

        IEnumerable<City> getAllCity();

        IEnumerable<InCaseOf> getAllInCaseOf();

        Bts getByID(int Id);

        bool IsUsed(string Id);

        void SaveChanges();
    }

    public class BtsService : IBtsService
    {
        private IBtsRepository _btsRepository;
        private IUnitOfWork _unitOfWork;

        public BtsService(IBtsRepository btsRepository, IUnitOfWork unitOfWork)
        {
            this._btsRepository = btsRepository;
            this._unitOfWork = unitOfWork;
        }

        public Bts Add(Bts newBts)
        {
            return _btsRepository.Add(newBts);
        }

        public Bts Delete(int Id)
        {
            return _btsRepository.Delete(Id);
        }

        public IEnumerable<Bts> getAll()
        {
            return _btsRepository.GetAll(includes: new string[] { "Profile" });
        }

        public IEnumerable<Bts> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _btsRepository.GetMulti(x => x.BtsCode.Contains(keyword) || x.Address.Contains(keyword));
            else
                return _btsRepository.GetAll();
        }

        public Bts getByID(int Id)
        {
            return _btsRepository.GetSingleById(Id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Bts newBts)
        {
            _btsRepository.Update(newBts);
        }

        public bool IsUsed(string Id)
        {
            return _btsRepository.IsUsed(Id);
        }

        public IEnumerable<Operator> getAllOperator()
        {
            return _btsRepository.getAllOperator();
        }

        public IEnumerable<Profile> getAllProfile()
        {
            return _btsRepository.getAllProfile();
        }

        public IEnumerable<Profile> getAllProfileInProcess()
        {
            return _btsRepository.getAllProfileInProcess();
        }

        public IEnumerable<City> getAllCity()
        {
            return _btsRepository.getAllCity();
        }

        public IEnumerable<InCaseOf> getAllInCaseOf()
        {
            return _btsRepository.getAllInCaseOf();
        }
    }
}