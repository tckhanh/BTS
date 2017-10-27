using BTS.Common;
using BTS.Data.Infrastructure;
using BTS.Data.Repository;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Service
{
    public interface IImportService
    {
        bool AddInCaseOf(InCaseOf item);
        void Update(InCaseOf newInCaseOf);
        void Save();
    }

    public class ImportService : IImportService
    {
        IInCaseOfRepository _inCaseOfRepository;
        IUnitOfWork _unitOfWork;


        public ImportService(IInCaseOfRepository inCaseOfRepository, IUnitOfWork unitOfWork)
        {
            this._inCaseOfRepository = inCaseOfRepository;
            this._unitOfWork = unitOfWork;
        }

        public InCaseOf Add(InCaseOf newInCaseOf)
        {
            return _inCaseOfRepository.Add(newInCaseOf);
        }

        public bool AddInCaseOf(InCaseOf item)
        {
            try
            {
                if (_inCaseOfRepository.GetSingleById(item.ID) == null)
                {
                    _inCaseOfRepository.Add(item);
                    _unitOfWork.Commit();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(InCaseOf newInCaseOf)
        {
            _inCaseOfRepository.Update(newInCaseOf);
        }
    }
}
