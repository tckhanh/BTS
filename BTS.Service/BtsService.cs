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

        Bts Delete(int ID);

        IEnumerable<Bts> getAll();

        IEnumerable<Bts> getAll(string keyword);

        IEnumerable<Operator> getAllOperator();

        IEnumerable<Profile> getAllProfile();

        IEnumerable<Profile> getAllProfileInProcess();

        IEnumerable<City> getAllCity();

        IEnumerable<InCaseOf> getAllInCaseOf();

        Bts getByID(int ID);

        bool IsUsed(string ID);

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

        public Bts Delete(int ID)
        {
            return _btsRepository.Delete(ID);
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

        public Bts getByID(int ID)
        {
            return _btsRepository.GetSingleById(ID);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Bts newBts)
        {
            _btsRepository.Update(newBts);
        }

        public bool IsUsed(string ID)
        {
            return _btsRepository.IsUsed(ID);
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