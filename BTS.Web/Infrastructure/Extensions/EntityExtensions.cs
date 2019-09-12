using BTS.Data.ApplicationModels;
using BTS.Model.Models;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace BTS.Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static string GetImagePath(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("user");
            }
            var claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                return claimsIdentity.FindFirst("ImagePath").Value;
            }
            return null;
        }

        public static void UpdateBts(this Bts bts, BtsViewModel btsVm)
        {
            bts.ProfileID = btsVm.ProfileID;
            bts.OperatorID = btsVm.OperatorID;
            bts.BtsCode = btsVm.BtsCode;
            bts.Address = btsVm.Address;
            bts.CityID = btsVm.CityID;
            bts.Longtitude = btsVm.Longtitude;
            bts.Latitude = btsVm.Latitude;
            bts.InCaseOfID = btsVm.InCaseOfID;
            bts.LastOwnCertificateIDs = btsVm.LastOwnCertificateIDs;
            bts.LastNoOwnCertificateIDs = btsVm.LastNoOwnCertificateIDs;
            bts.ProfilesInProcess = btsVm.ProfilesInProcess;
            bts.ReasonsNoCertificate = btsVm.ReasonsNoCertificate;
        }

        public static void UpdateCertificate(this Certificate certificate, CertificateViewModel btsCertificateVm)
        {
            certificate.ProfileID = btsCertificateVm.ProfileID;
            certificate.BtsCode = btsCertificateVm.BtsCode;
            certificate.Longtitude = btsCertificateVm.Longtitude;
            certificate.Latitude = btsCertificateVm.Latitude;
            certificate.Address = btsCertificateVm.Address;
            certificate.CityID = btsCertificateVm.CityID;

            certificate.SubBtsQuantity = btsCertificateVm.SubBtsQuantity;
            certificate.SubBtsAntenHeights = btsCertificateVm.SubBtsAntenHeights;
            certificate.SubBtsAntenNums = btsCertificateVm.SubBtsAntenNums;
            certificate.SharedAntens = btsCertificateVm.SharedAntens;
            certificate.SubBtsBands = btsCertificateVm.SubBtsBands;
            certificate.SubBtsCodes = btsCertificateVm.SubBtsCodes;
            certificate.SubBtsConfigurations = btsCertificateVm.SubBtsConfigurations;
            certificate.SubBtsEquipments = btsCertificateVm.SubBtsEquipments;
            certificate.SubBtsOperatorIDs = btsCertificateVm.SubBtsOperatorIDs;
            certificate.SubBtsPowerSums = btsCertificateVm.SubBtsPowerSums;

            certificate.InCaseOfID = btsCertificateVm.InCaseOfID;
            certificate.TestReportNo = btsCertificateVm.TestReportNo;
            certificate.TestReportDate = btsCertificateVm.TestReportDate;

            certificate.IsPoleOnGround = btsCertificateVm.IsPoleOnGround;
            certificate.IsSafeLimit = btsCertificateVm.IsSafeLimit;
            certificate.SafeLimitHeight = btsCertificateVm.SafeLimitHeight;
            certificate.IsHouseIn100m = btsCertificateVm.IsHouseIn100m;
            certificate.MaxHeightIn100m = btsCertificateVm.MaxHeightIn100m;
            certificate.MaxPowerSum = btsCertificateVm.MaxPowerSum;
            certificate.IsMeasuringExposure = btsCertificateVm.IsMeasuringExposure;
            certificate.MinAntenHeight = btsCertificateVm.MinAntenHeight;
            certificate.OffsetHeight = btsCertificateVm.OffsetHeight;
            
            certificate.IssuedPlace = btsCertificateVm.IssuedPlace;
            certificate.IssuedDate = btsCertificateVm.IssuedDate;
            certificate.ExpiredDate = btsCertificateVm.ExpiredDate;
            certificate.Signer = btsCertificateVm.Signer;
            certificate.OperatorID = btsCertificateVm.OperatorID;
        }

        public static void UpdateNoCertificate(this NoCertificate noCertificate, NoCertificateViewModel btsCertificateVm)
        {
            noCertificate.ProfileID = btsCertificateVm.ProfileID;
            noCertificate.OperatorID = btsCertificateVm.OperatorID;
            noCertificate.BtsCode = btsCertificateVm.BtsCode;
            noCertificate.Address = btsCertificateVm.Address;
            noCertificate.CityID = btsCertificateVm.CityID;

            noCertificate.Longtitude = btsCertificateVm.Longtitude;
            noCertificate.Latitude = btsCertificateVm.Latitude;
            noCertificate.InCaseOfID = btsCertificateVm.InCaseOfID;
            noCertificate.LabID = btsCertificateVm.LabID;
            noCertificate.TestReportNo = btsCertificateVm.TestReportNo;
            noCertificate.TestReportDate = btsCertificateVm.TestReportDate;
            noCertificate.ReasonNoCertificate = btsCertificateVm.ReasonNoCertificate;
        }

        public static void UpdateInCaseOf(this InCaseOf myInCaseOf, InCaseOfViewModel inCaseOfVm)
        {
            myInCaseOf.Name = inCaseOfVm.Name;
        }

        public static void UpdateLab(this Lab myLab, LabViewModel labCaseOfVm)
        {
            myLab.Name = labCaseOfVm.Name;
            myLab.Address = labCaseOfVm.Address;
            myLab.Phone = labCaseOfVm.Phone;
            myLab.Fax = labCaseOfVm.Fax;
        }

        public static void UpdateCity(this City myCity, CityViewModel cityVm)
        {
            myCity.Name = cityVm.Name;
        }

        public static void UpdateOperator(this Operator myoperator, OperatorViewModel operatorVm)
        {
            myoperator.Name = operatorVm.Name;
        }

        public static void UpdateApplicant(this Applicant myApplicant, ApplicantViewModel applicantVm)
        {
            myApplicant.Name = applicantVm.Name;
            myApplicant.OperatorID = applicantVm.OperatorID;
            myApplicant.Address = applicantVm.Address;
            myApplicant.Phone = applicantVm.Phone;
            myApplicant.Fax = applicantVm.Fax;
            myApplicant.ContactName = applicantVm.ContactName;
        }

        public static void UpdateFeedback(this Feedback feedback, FeedbackViewModel feedbackVm)
        {
            feedback.Name = feedbackVm.Name;
            feedback.Email = feedbackVm.Email;
            feedback.Message = feedbackVm.Message;
            feedback.Status = feedbackVm.Status;
            feedback.CreatedDate = DateTime.Now;
        }

        public static void UpdateApplicationGroup(this ApplicationGroup appGroup, ApplicationGroupViewModel appGroupViewModel)
        {
            appGroup.Name = appGroupViewModel.Name;
            appGroup.Description = appGroupViewModel.Description;
        }

        public static void UpdateApplicationRole(this ApplicationRole appRole, ApplicationRoleViewModel appRoleViewModel, String action = "add")
        {
            if (action == "add")
                appRole.Id = Guid.NewGuid().ToString();
            appRole.Name = appRoleViewModel.Name;
            appRole.Description = appRoleViewModel.Description;
        }

        public static void UpdateUser(this ApplicationUser appUser, ApplicationUserViewModel appUserViewModel)
        {
            appUser.FullName = appUserViewModel.FullName;
            appUser.Email = appUserViewModel.Email;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
            appUser.Address = appUserViewModel.Address;
            appUser.BirthDay = appUserViewModel.BirthDay;
            appUser.FatherLand = appUserViewModel.FatherLand;
            appUser.Level = appUserViewModel.Level;
            appUser.EducationalField = appUserViewModel.EducationalField;
            appUser.EntryDate = appUserViewModel.EntryDate;
            appUser.EndDate = appUserViewModel.EndDate;
            appUser.Locked = appUserViewModel.Locked;
            appUser.JobPositions = appUserViewModel.JobPositions;
            appUser.ImagePath = appUserViewModel.ImagePath;
        }

        public static void UpdateGroup(this ApplicationGroup appGroup, ApplicationGroupViewModel appGroupViewModel)
        {
            appGroup.Name = appGroupViewModel.Name;
            appGroup.Description = appGroupViewModel.Description;
        }

        public static void UpdateNewCertificate(this Bts Item, Certificate newItem)
        {
            Item.Address = newItem.Address;
            Item.CityID = newItem.CityID;
            Item.Longtitude = newItem.Longtitude;
            Item.Latitude = newItem.Latitude;
            Item.InCaseOfID = newItem.InCaseOfID;
            Item.UpdatedDate = DateTime.Now;
        }

        public static void UpdatePreviousCertificate(this Bts Item, Certificate newItem)
        {
            Item.Address = newItem.Address;
            Item.CityID = newItem.CityID;
            Item.Longtitude = newItem.Longtitude;
            Item.Latitude = newItem.Latitude;
            Item.InCaseOfID = newItem.InCaseOfID;
            Item.UpdatedDate = DateTime.Now;
        }

        public static void UpdateProfile(this Profile Item, ProfileViewModel ItemVm)
        {
            Item.ApplicantID = ItemVm.ApplicantID;
            Item.ProfileNum = ItemVm.ProfileNum;
            Item.ProfileDate = ItemVm.ProfileDate;
            Item.BtsQuantity = ItemVm.BtsQuantity;
            Item.AcceptedBtsQuantity = ItemVm.AcceptedBtsQuantity;
            Item.ApplyDate = ItemVm.ApplyDate;
            Item.Fee = ItemVm.Fee;
            Item.FeeAnnounceNum = ItemVm.FeeAnnounceNum;
            Item.FeeAnnounceDate = ItemVm.FeeAnnounceDate;
            Item.FeeReceiptDate = ItemVm.FeeReceiptDate;
        }
    }
}