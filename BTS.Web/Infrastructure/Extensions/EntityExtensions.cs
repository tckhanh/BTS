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
        public static void UpdateBTSCertificate(this BTSCertificate btsCertificate, BTSCertificateViewModel btsCertificateVm)
        {
            btsCertificate.ID = btsCertificateVm.ID;
            btsCertificate.ProfileID = btsCertificateVm.ProfileID;
            btsCertificate.Longtitude = btsCertificateVm.Longtitude;
            btsCertificate.Latitude = btsCertificateVm.Latitude;
            btsCertificate.Address = btsCertificateVm.Address;
            btsCertificate.CityID = btsCertificateVm.CityID;

            btsCertificate.DistrictID = btsCertificateVm.DistrictID;
            btsCertificate.SubBTSNum = btsCertificateVm.SubBTSNum;
            btsCertificate.InCaseOf = btsCertificateVm.InCaseOf;
            btsCertificate.ReportNum = btsCertificateVm.ReportNum;
            btsCertificate.ReportDate = btsCertificateVm.ReportDate;
            btsCertificate.CertificateNum = btsCertificateVm.CertificateNum;

            btsCertificate.SafeLimit = btsCertificateVm.SafeLimit;
            btsCertificate.IssuedPlace = btsCertificateVm.IssuedPlace;
            btsCertificate.IssuedDate = btsCertificateVm.IssuedDate;
            btsCertificate.ExpiredDate = btsCertificateVm.ExpiredDate;
            btsCertificate.Signer = btsCertificateVm.Signer;
            btsCertificate.OperatorID = btsCertificateVm.OperatorID;
        }

        public static void UpdateOperator(this Operator myoperator, OperatorViewModel operatorVm)
        {
            myoperator.ID = operatorVm.ID;
            myoperator.Name = operatorVm.Name;
            myoperator.Address = operatorVm.Address;
            myoperator.Telephone = operatorVm.Telephone;
            myoperator.Fax = operatorVm.Fax;
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
    }
}