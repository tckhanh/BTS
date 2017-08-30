using BTS.Model.Models;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.Web.Infastructure.Extensions
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
    }
}