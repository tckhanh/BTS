using BTS.Model.Models;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateCertificate(this Certificate certificate, CertificateViewModel btsCertificateVm)
        {
            certificate.ID = btsCertificateVm.ID;
            certificate.ProfileID = btsCertificateVm.ProfileID;
            certificate.Longtitude = btsCertificateVm.Longtitude;
            certificate.Latitude = btsCertificateVm.Latitude;
            certificate.Address = btsCertificateVm.Address;
            certificate.CityID = btsCertificateVm.CityID;

            certificate.SubBtsQuantity = btsCertificateVm.SubBtsQuantity;
            certificate.SubBtsAntenHeights = btsCertificateVm.SubBtsAntenHeights;
            certificate.SubBtsAntenNums = btsCertificateVm.SubBtsAntenNums;
            certificate.SubBtsBands = btsCertificateVm.SubBtsBands;
            certificate.SubBtsCodes = btsCertificateVm.SubBtsCodes;
            certificate.SubBtsConfigurations = btsCertificateVm.SubBtsConfigurations;
            certificate.SubBtsEquipments = btsCertificateVm.SubBtsEquipments;
            certificate.SubBtsOperatorIDs = btsCertificateVm.SubBtsOperatorIDs;
            certificate.SubBtsPowerSums = btsCertificateVm.SubBtsPowerSums;

            certificate.InCaseOfID = btsCertificateVm.InCaseOfID;
            certificate.TestReportNo = btsCertificateVm.TestReportNo;
            certificate.TestReportDate = btsCertificateVm.TestReportDate;

            certificate.SafeLimit = btsCertificateVm.SafeLimit;
            certificate.IssuedPlace = btsCertificateVm.IssuedPlace;
            certificate.IssuedDate = btsCertificateVm.IssuedDate;
            certificate.ExpiredDate = btsCertificateVm.ExpiredDate;
            certificate.Signer = btsCertificateVm.Signer;
            certificate.OperatorID = btsCertificateVm.OperatorID;
        }

        public static void UpdateInCaseOf(this InCaseOf myInCaseOf, InCaseOfViewModel inCaseOfVm)
        {
            myInCaseOf.ID = inCaseOfVm.ID;
            myInCaseOf.Code = inCaseOfVm.Code;
            myInCaseOf.Name = inCaseOfVm.Name;
            myInCaseOf.UpdatedBy = inCaseOfVm.UpdatedBy;
            myInCaseOf.UpdatedDate = inCaseOfVm.UpdatedDate;
            myInCaseOf.DeletedBy = inCaseOfVm.DeletedBy;
            myInCaseOf.DeletedDate = inCaseOfVm.DeletedDate;
        }

        public static void UpdateCity(this City myCity, CityViewModel cityVm)
        {
            myCity.ID = cityVm.ID;
            myCity.Name = cityVm.Name;
        }

        public static void UpdateOperator(this Operator myoperator, OperatorViewModel operatorVm)
        {
            myoperator.ID = operatorVm.ID;
            myoperator.Name = operatorVm.Name;
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
            appGroup.ID = appGroupViewModel.ID;
            appGroup.Name = appGroupViewModel.Name;
            appGroup.Description = appGroupViewModel.Description;
        }

        public static void UpdateApplicationRole(this ApplicationRole appRole, ApplicationRoleViewModel appRoleViewModel, string action = "add")
        {
            if (action == "update")
                appRole.Id = appRoleViewModel.Id;
            else
                appRole.Id = Guid.NewGuid().ToString();
            appRole.Name = appRoleViewModel.Name;
            appRole.Description = appRoleViewModel.Description;
        }

        public static void UpdateUser(this ApplicationUser appUser, ApplicationUserViewModel appUserViewModel, string action = "add")
        {
            appUser.Id = appUserViewModel.Id;
            appUser.FullName = appUserViewModel.FullName;
            appUser.BirthDay = appUserViewModel.BirthDay;
            appUser.Email = appUserViewModel.Email;
            appUser.UserName = appUserViewModel.UserName;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
        }

        public static void UpdateNewCertificate(this Bts Item, Certificate newItem)
        {
            Item.Address = newItem.Address;
            Item.CityID = newItem.CityID;
            Item.Longtitude = newItem.Longtitude;
            Item.Latitude = newItem.Latitude;
            Item.InCaseOfID = newItem.InCaseOfID;
            Item.IssuedCertificateID = newItem.ID;
            Item.UpdatedDate = DateTime.Now;
        }

        public static void UpdatePreviousCertificate(this Bts Item, Certificate newItem)
        {
            Item.Address = newItem.Address;
            Item.CityID = newItem.CityID;
            Item.Longtitude = newItem.Longtitude;
            Item.Latitude = newItem.Latitude;
            Item.InCaseOfID = newItem.InCaseOfID;
            Item.IssuedCertificateID = newItem.ID;
            Item.UpdatedDate = DateTime.Now;
        }
    }
}