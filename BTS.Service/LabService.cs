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
    public interface ILabService
    {
        Lab Add(Lab newLab);

        void Update(Lab newLab);

        Lab Delete(string Id);

        IEnumerable<Lab> getAll();

        IEnumerable<Lab> getAll(string keyword);

        Lab getByID(string Id);

        Lab getByID(int Id);

        bool IsUsed(string Id);

        void Save();
    }

    public class LabService : ILabService
    {
        private ILabRepository _labRepository;
        private IUnitOfWork _unitOfWork;

        public LabService(ILabRepository labRepository, IUnitOfWork unitOfWork)
        {
            _labRepository = labRepository;
            _unitOfWork = unitOfWork;
        }

        public Lab Add(Lab newLab)
        {
            return _labRepository.Add(newLab);
        }

        public Lab Delete(string Id)
        {
            return _labRepository.Delete(Id);
        }

        public IEnumerable<Lab> getAll()
        {
            return _labRepository.GetAll();
        }

        public IEnumerable<Lab> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _labRepository.GetMulti(x => x.Id.ToString().Contains(keyword) || x.Name.Contains(keyword));
            else
                return _labRepository.GetAll();
        }

        public Lab getByID(string Id)
        {
            return _labRepository.GetSingleById(Id);
        }

        public Lab getByID(int Id)
        {
            return _labRepository.GetSingleById(Id);
        }

        public bool IsUsed(string Id)
        {
            return _labRepository.IsUsed(Id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Lab newLab)
        {
            _labRepository.Update(newLab);
        }
    }
}