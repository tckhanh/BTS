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

        Bts Delete(string Id);

        IEnumerable<Bts> getAll(out int totalRow);

        IEnumerable<Bts> getAll(out int totalRow, DateTime startDate, DateTime endDate);
        IEnumerable<Bts> getAll(string keyword);

        IEnumerable<Operator> getAllOperator();

        IEnumerable<Profile> getAllProfile();

        IEnumerable<Profile> getAllProfileInProcess();

        IEnumerable<City> getAllCity();

        IEnumerable<InCaseOf> getAllInCaseOf();

        Bts getByID(string Id);

        bool IsUsed(string Id);

        void SaveChanges();
    }

    public class BtsService : IBtsService
    {
        private IBtsRepository _btsRepository;
        private IProfileService _profileService;
        private IUnitOfWork _unitOfWork;

        public BtsService(IBtsRepository btsRepository, IProfileService profileService, IUnitOfWork unitOfWork)
        {
            _btsRepository = btsRepository;
            _profileService = profileService;
            _unitOfWork = unitOfWork;
        }

        public Bts Add(Bts newBts)
        {
            return _btsRepository.Add(newBts);
        }

        public Bts Delete(string Id)
        {
            return _btsRepository.Delete(Id);
        }

        public IEnumerable<Bts> getAll(out int totalRow)
        {
            IEnumerable<Bts> result;
            result = _btsRepository.GetAll(includes: new string[] { "Profile" });
            totalRow = result.Count();
            return result;            
        }

        public IEnumerable<Bts> getAll(out int totalRow, DateTime startDate, DateTime endDate)
        {
            IEnumerable<Bts> result;
            result = _btsRepository.getAll(startDate, endDate);
            totalRow = result.Count();
            return result;
        }

        public IEnumerable<Bts> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _btsRepository.GetMulti(x => x.BtsCode.Contains(keyword) || x.Address.Contains(keyword));
            else
                return _btsRepository.GetAll();
        }

        public Bts getByID(string Id)
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