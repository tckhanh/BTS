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
    public interface IConfigService
    {
        SystemConfig Add(SystemConfig newConfig);

        void Update(SystemConfig newConfig);

        SystemConfig Delete(string Id);

        IEnumerable<SystemConfig> getAll();

        IEnumerable<SystemConfig> getAll(string keyword);

        SystemConfig getByID(string Id);

        SystemConfig getByID(int Id);

        bool IsUsed(string Id);

        void Save();
    }

    public class ConfigService : IConfigService
    {
        private IConfigRepository _configRepository;
        private IUnitOfWork _unitOfWork;

        public ConfigService(IConfigRepository configRepository, IUnitOfWork unitOfWork)
        {
            _configRepository = configRepository;
            _unitOfWork = unitOfWork;
        }

        public SystemConfig Add(SystemConfig newConfig)
        {
            return _configRepository.Add(newConfig);
        }

        public SystemConfig Delete(string Id)
        {
            return _configRepository.Delete(Id);
        }

        public IEnumerable<SystemConfig> getAll()
        {
            return _configRepository.GetAll();
        }

        public IEnumerable<SystemConfig> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _configRepository.GetMulti(x => x.Id.ToString().Contains(keyword) || x.Code.Contains(keyword));
            else
                return _configRepository.GetAll();
        }

        public SystemConfig getByID(string Id)
        {
            return _configRepository.GetSingleById(Id);
        }

        public SystemConfig getByID(int Id)
        {
            return _configRepository.GetSingleById(Id);
        }

        public bool IsUsed(string Id)
        {
            return _configRepository.IsUsed(Id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(SystemConfig newConfig)
        {
            _configRepository.Update(newConfig);
        }
    }
}