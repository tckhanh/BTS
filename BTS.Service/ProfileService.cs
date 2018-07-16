﻿using BTS.Data.Infrastructure;
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

        Profile Delete(string Id);

        IEnumerable<Profile> getAll();

        IEnumerable<Profile> getAll(string keyword);

        IEnumerable<Applicant> getAllApplicant();

        Profile getByID(int Id);

        bool IsUsed(int Id);

        void Save();
    }

    public class ProfileService : IProfileService
    {
        private IProfileRepository _profileRepository;
        private IApplicantRepository _applicantRepository;
        private IUnitOfWork _unitOfWork;

        public ProfileService(IProfileRepository profileRepository, IApplicantRepository applicantRepository, IUnitOfWork unitOfWork)
        {
            _profileRepository = profileRepository;
            _applicantRepository = applicantRepository;
            _unitOfWork = unitOfWork;
        }

        public Profile Add(Profile newProfile)
        {
            return _profileRepository.Add(newProfile);
        }

        public Profile Delete(string Id)
        {
            return _profileRepository.Delete(Id);
        }

        public IEnumerable<Profile> getAll()
        {
            return _profileRepository.GetAll(includes: new string[] { "Applicant" });
        }

        public IEnumerable<Profile> getAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _profileRepository.GetMulti(x => x.ProfileNum.Contains(keyword) || x.ApplicantID.Contains(keyword));
            else
                return _profileRepository.GetAll();
        }

        public Profile getByID(int Id)
        {
            return _profileRepository.GetSingleById(Id);
        }

        public bool IsUsed(int Id)
        {
            return _profileRepository.IsUsed(Id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Profile newProfile)
        {
            _profileRepository.Update(newProfile);
        }

        public IEnumerable<Applicant> getAllApplicant()
        {
            return _applicantRepository.GetAll();
        }
    }
}