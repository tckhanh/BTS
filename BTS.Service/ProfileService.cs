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
    public interface IProfileService
    {
        Profile Add(Profile newProfile);

        void Update(Profile newProfile);

        Profile Delete(string ID);

        IEnumerable<Profile> getAll();

        IEnumerable<Profile> getAll(string keyword);

        Profile getByID(string ID);

        void Save();
    }

    public class ProfileService : IProfileService
    {
        private IProfileRepository _profileRepository;
        private IUnitOfWork _unitOfWork;

        public ProfileService(IProfileRepository profileRepository, IUnitOfWork unitOfWork)
        {
            this._profileRepository = profileRepository;
            this._unitOfWork = unitOfWork;
        }

        public Profile Add(Profile newProfile)
        {
            return _profileRepository.Add(newProfile);
        }

        public Profile Delete(string ID)
        {
            return _profileRepository.Delete(ID);
        }

        public IEnumerable<Profile> getAll()
        {
            return _profileRepository.GetAll();
        }

        public IEnumerable<Profile> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _profileRepository.GetMulti(x => x.ProfileNum.Contains(keyword) || x.ApplicantID.Contains(keyword));
            else
                return _profileRepository.GetAll();
        }

        public Profile getByID(string ID)
        {
            return _profileRepository.GetSingleById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Profile newProfile)
        {
            _profileRepository.Update(newProfile);
        }
    }
}