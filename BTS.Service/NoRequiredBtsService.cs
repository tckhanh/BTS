using BTS.Data.Infrastructure;
using BTS.Data.Repository;
using BTS.Model.Models;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTS.Service
{
    public interface INoRequiredBtsService
    {
        NoRequiredBts Add(NoRequiredBts btsNoRequiredBts);

        SubBtsInNoRequiredBts Add(SubBtsInNoRequiredBts subBtsInCert);

        void Update(NoRequiredBts btsNoRequiredBts);

        void Delete(string Id);

        void DeleteSubBTSinNoRequiredBts(string NoRequiredBtsId);

        IEnumerable<NoRequiredBts> getAll(out int totalRow, bool onlyValidNoRequiredBts, DateTime startDate, DateTime endDate, string[] includes = null);
        IEnumerable<NoRequiredBts> getAll(out int totalRow, bool onlyValidNoRequiredBts, string[] includes = null);
        IEnumerable<ReportTT18NoCert> getReportTT18NoCert(out int totalRows, DateTime startDate, DateTime endDate);
        NoRequiredBts findNoRequiredBts(string BtsCode, string ProfileID);

        IEnumerable<NoRequiredBts> getByBTSCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10);

        IEnumerable<NoRequiredBts> getByCity(string cityID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10);

        IEnumerable<NoRequiredBts> getByOperator(string cityID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10);

        IEnumerable<NoRequiredBts> getNoRequiredBtsByYear(int year, int page, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10);

        IEnumerable<NoRequiredBts> getNoRequiredBtsByYear(int year, string[] includes = null);

        IEnumerable<NoRequiredBts> getNoRequiredBtsByBTSCode(string btsCode, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10);

        IEnumerable<NoRequiredBts> getNoRequiredBtsByBtsCodeOrAddress(string btsCodeOrAddress, string[] includes = null);

        IEnumerable<NoRequiredBts> getNoRequiredBtsExpired(string[] includes = null);

        IEnumerable<NoRequiredBts> getNoRequiredBtsByCity(string cityID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10);

        IEnumerable<NoRequiredBts> getNoRequiredBtsByCity(string cityID, string[] includes = null);

        IEnumerable<NoRequiredBts> getNoRequiredBtsByLocation(LatLng center, LatLngBounds boundBox, int KmRadius, string[] includes = null);

        IEnumerable<NoRequiredBts> getNoRequiredBtsByOperator(string operatorID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10);

        IEnumerable<NoRequiredBts> getNoRequiredBtsByOperator(string operatorID, string[] includes = null);

        IEnumerable<NoRequiredBts> getNoRequiredBtsByProfile(string profileID, string[] includes = null);

        NoRequiredBts getByID(string Id, string[] includes = null);

        List<SubBtsInNoRequiredBts> getDetailByID(string Id);

        List<SubBtsInNoRequiredBts> getDetailByBtsID(string noRequiredBtsID);

        void SaveChanges();
    }

    public class NoRequiredBtsService : INoRequiredBtsService
    {
        private INoRequiredBtsRepository _NoRequiredBtsRepository;
        private ISubBtsInNoRequiredBtsRepository _SubBTSinNoRequiredBtsRepository;
        private IUnitOfWork _unitOfWork;

        public NoRequiredBtsService(INoRequiredBtsRepository certificateRepository, ISubBtsInNoRequiredBtsRepository subBTSinNoRequiredBtsRepository, IUnitOfWork unitOfWork)
        {
            _NoRequiredBtsRepository = certificateRepository;
            _SubBTSinNoRequiredBtsRepository = subBTSinNoRequiredBtsRepository;
            _unitOfWork = unitOfWork;
        }

        public NoRequiredBts Add(NoRequiredBts btsNoRequiredBts)
        {
            return _NoRequiredBtsRepository.Add(btsNoRequiredBts);
        }

        public SubBtsInNoRequiredBts Add(SubBtsInNoRequiredBts subBtsInCert)
        {
            return _SubBTSinNoRequiredBtsRepository.Add(subBtsInCert);
        }

        public void Delete(string Id)
        {
            _SubBTSinNoRequiredBtsRepository.DeleteMulti(x => x.Id == Id);
            _NoRequiredBtsRepository.Delete(Id);
        }

        public void DeleteSubBTSinNoRequiredBts(string NoRequiredBtsId)
        {
            _SubBTSinNoRequiredBtsRepository.DeleteMulti(x => x.NoRequiredBtsID == NoRequiredBtsId);
        }

        public IEnumerable<ReportTT18NoCert> getReportTT18NoCert(out int totalRows, DateTime startDate, DateTime endDate)
        {
            IEnumerable<ReportTT18NoCert> result;
            result = _NoRequiredBtsRepository.GetReportTT18NoCertByDate(startDate, endDate);
            totalRows = result.Count();
            return result;
        }

        public IEnumerable<NoRequiredBts> getAll(out int totalRows, bool onlyValidNoRequiredBts, DateTime startDate, DateTime endDate, string[] includes = null)
        {
            IEnumerable<NoRequiredBts> result;
            if (onlyValidNoRequiredBts)
            {
                result = _NoRequiredBtsRepository.GetMulti(x => x.AnnouncedDate >= startDate && x.AnnouncedDate <= endDate, includes);
            }
            else
            {
                result = _NoRequiredBtsRepository.GetMulti(x => x.AnnouncedDate >= startDate && x.AnnouncedDate <= endDate, includes);
            }

            totalRows = result.Count();
            return result;
        }

        public IEnumerable<NoRequiredBts> getAll(out int totalRows, bool onlyValidNoRequiredBts, string[] includes = null)
        {
            IEnumerable<NoRequiredBts> result;
            if (onlyValidNoRequiredBts)
            {
                result = _NoRequiredBtsRepository.GetMulti(x => x.IsCanceled == false, includes);
            }
            else
            {
                result = _NoRequiredBtsRepository.GetMulti(x => true, includes);
            }

            totalRows = result.Count();
            return result;
        }

        public NoRequiredBts findNoRequiredBts(string BtsCode, string ProfileID)
        {
            return _NoRequiredBtsRepository.GetSingleByCondition(x => x.BtsCode == BtsCode && x.AnnouncedDoc == ProfileID);
        }

        public IEnumerable<NoRequiredBts> getByBTSCode(string btsCode, out int totalRow, int pageIndex = 1, int pageSize = 10)
        {
            return _SubBTSinNoRequiredBtsRepository.GetMultiPagingByBtsCode(btsCode, out totalRow, pageIndex, pageSize, false);
        }

        public IEnumerable<NoRequiredBts> getByCity(string cityID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10)
        {
            return _NoRequiredBtsRepository.GetMultiPaging(x => x.CityID == cityID, out totalRow, includes, pageIndex, pageSize);
        }

        public NoRequiredBts getByID(string Id, string[] includes = null)
        {
            return _NoRequiredBtsRepository.GetSingleByCondition(x => x.Id == Id, includes);
        }

        public IEnumerable<NoRequiredBts> getByOperator(string operatorID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10)
        {
            return _NoRequiredBtsRepository.GetMultiPaging(x => x.OperatorID == operatorID, out totalRow, includes, pageIndex, pageSize);
        }

        public IEnumerable<NoRequiredBts> getNoRequiredBtsByYear(int year, int page, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10)
        {
            return _NoRequiredBtsRepository.GetMultiPaging(x => x.AnnouncedDate != null && Convert.ToDateTime(x.AnnouncedDate).Year == year, out totalRow, includes, pageIndex, pageSize);
        }

        public IEnumerable<NoRequiredBts> getNoRequiredBtsByBTSCode(string btsCode, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10)
        {
            var query = _SubBTSinNoRequiredBtsRepository.GetMultiByBtsCode(btsCode, true);
            totalRow = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<NoRequiredBts> getNoRequiredBtsByCity(string cityID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10)
        {
            return _NoRequiredBtsRepository.GetMultiPaging(x => x.CityID == cityID, out totalRow, includes, pageIndex, pageSize);
        }

        public IEnumerable<NoRequiredBts> getNoRequiredBtsByOperator(string operatorID, out int totalRow, string[] includes = null, int pageIndex = 1, int pageSize = 10)
        {
            return _NoRequiredBtsRepository.GetMultiPaging(x => x.OperatorID == operatorID, out totalRow, includes, pageIndex, pageSize);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(NoRequiredBts btsNoRequiredBts)
        {
            _NoRequiredBtsRepository.Update(btsNoRequiredBts);
        }

        public IEnumerable<NoRequiredBts> getNoRequiredBtsByYear(int year, string[] includes = null)
        {
            return _NoRequiredBtsRepository.GetMulti(x => x.AnnouncedDate != null && Convert.ToDateTime(x.AnnouncedDate).Year == year, includes);
        }

        public IEnumerable<NoRequiredBts> getNoRequiredBtsByCity(string cityID, string[] includes = null)
        {
            return _NoRequiredBtsRepository.GetMulti(x => x.CityID == cityID, includes);
        }

        public IEnumerable<NoRequiredBts> getNoRequiredBtsByLocation(LatLng center, LatLngBounds boundBox, int KmRadius, string[] includes = null)
        {
            return _NoRequiredBtsRepository.GetMulti(x => x.Latitude < boundBox._northEast.lat && x.Latitude > boundBox._southWest.lat && x.Longtitude < boundBox._northEast.lng && x.Longtitude > boundBox._southWest.lng, includes);
        }

        public IEnumerable<NoRequiredBts> getNoRequiredBtsExpired(string[] includes = null)
        {
            return _NoRequiredBtsRepository.GetMulti(x => x.IsCanceled == true, includes);
        }


        public IEnumerable<NoRequiredBts> getNoRequiredBtsByBtsCodeOrAddress(string btsCodeOrAddress, string[] includes = null)
        {
            return _NoRequiredBtsRepository.GetMulti(x => x.BtsCode.ToLower().Contains(btsCodeOrAddress) || x.Address.ToLower().Contains(btsCodeOrAddress), includes);
        }

        public IEnumerable<NoRequiredBts> getNoRequiredBtsByOperator(string operatorID, string[] includes = null)
        {
            return _NoRequiredBtsRepository.GetMulti(x => x.OperatorID == operatorID, includes);
        }

        public IEnumerable<NoRequiredBts> getNoRequiredBtsByProfile(string profileID, string[] includes = null)
        {
            return _NoRequiredBtsRepository.GetMulti(x => x.AnnouncedDoc == profileID, includes);
        }

        public List<SubBtsInNoRequiredBts> getDetailByID(string CertId)
        {
            return _SubBTSinNoRequiredBtsRepository.GetMulti(x => x.Id == CertId).OrderBy(x => x.Id).ToList();
        }

        public List<SubBtsInNoRequiredBts> getDetailByBtsID(string noRequiredBtsID)
        {
            return _SubBTSinNoRequiredBtsRepository.GetMulti(x => x.NoRequiredBtsID == noRequiredBtsID).OrderBy(x => x.BtsSerialNo).ToList();
        }
    }
}