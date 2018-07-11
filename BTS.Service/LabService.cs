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

        Lab Delete(string ID);

        IEnumerable<Lab> getAll();

        IEnumerable<Lab> getAll(string keyword);

        Lab getByID(string ID);

        Lab getByID(int ID);

        bool IsUsed(string ID);

        void Save();
    }

    public class LabService: ILabService
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

        public Lab Delete(string ID)
        {
            return _labRepository.Delete(ID);
        }

        public IEnumerable<Lab> getAll()
        {
            return _labRepository.GetAll();
        }

        public IEnumerable<Lab> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _labRepository.GetMulti(x => x.ID.ToString().Contains(keyword) || x.Name.Contains(keyword));
            else
                return _labRepository.GetAll();
        }

        public Lab getByID(string ID)
        {
            return _labRepository.GetSingleById(ID);
        }

        public Lab getByID(int ID)
        {
            return _labRepository.GetSingleById(ID);
        }

        public bool IsUsed(string ID)
        {
            return _labRepository.IsUsed(ID);
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