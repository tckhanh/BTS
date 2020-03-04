using BTS.Common;
using BTS.Data;
using BTS.Model.Models;
using BTS.Web.Models;
using SKGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.Web.Infrastructure.Core
{
    public static class checkLicence
    {
        static SerialKeyConfiguration skc = new SerialKeyConfiguration();
        static Generate generate = new Generate(skc);
        static Validate validate = new Validate(skc);
        public static string machineCode = validate.MachineCode.ToString();

        public static IEnumerable<Licence> getLicenses()
        {
            BTSDbContext DbContext = new BTSDbContext();
            return DbContext.Licences.Where(x => x.enable == true);
        }

        public static bool isValid()
        {
            IEnumerable<Licence> Licences = getLicenses();
            foreach (var Licence in Licences)
            {
                LicenceViewModel lastLicenceVM = GetLicenceInfo(Licence);
                if (lastLicenceVM.isValid)
                    return !lastLicenceVM.isExpired;                
            }
            return false;
        }

        public static LicenceViewModel GetLicenceInfo(Licence licence)
        {
            validate.secretPhase = CommonConstants.SECRET_PHASE;
            validate.Key = licence.key;

            LicenceViewModel LicenceInfo = new LicenceViewModel();
            LicenceInfo.Id = licence.Id;
            LicenceInfo.enable = licence.enable;
            LicenceInfo.machineCode = machineCode;
            LicenceInfo.key = licence.key;
            LicenceInfo.isValid = validate.IsValid;
            if (LicenceInfo.isValid)
            {
                LicenceInfo.CreationDate = validate.CreationDate;
                LicenceInfo.ExpireDate = validate.ExpireDate;
                if (LicenceInfo.ExpireDate < DateTime.Now)
                    LicenceInfo.isExpired = true;
                else
                    LicenceInfo.isExpired = false;
                LicenceInfo.TimeSet = validate.SetTime;
                LicenceInfo.DaysLeft = validate.DaysLeft;
            }
            return LicenceInfo;
        }
    }
}