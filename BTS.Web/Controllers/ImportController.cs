using BTS.Common;
using BTS.Common.ViewModels;
using BTS.ExcelLib;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    [AuthorizeRoles(CommonConstants.Data_CanImport_Role)]
    public class ImportController : BaseController
    {
        // GET: Import
        private IImportService _importService;

        private ExcelIO _excelIO;
        private NumberFormatInfo provider;

        public ImportController(IImportService importService, IErrorService errorService) : base(errorService)
        {
            this._importService = importService;
            _excelIO = new ExcelIO(errorService);
            provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ",";
            provider.NumberGroupSeparator = ".";
            provider.NumberGroupSizes = new int[] { 3 };
        }

        // Create a NumberFormatInfo object and set some of its properties.

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HttpPostedFileBase file, string ImportAction)
        {
            int result = 0;
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/ImportData/") + Request.Files["file"].FileName;
                    try
                    {
                        if (System.IO.File.Exists(fileLocation))
                            System.IO.File.Delete(fileLocation);

                        Request.Files["file"].SaveAs(fileLocation);

                        _excelIO.FormatColumnDecimalToText(fileLocation);

                        //string extendedProperties = "Excel 12.0;HDR=YES;IMEX=1";
                        //string connectionString1 = string.Format(CultureInfo.CurrentCulture, "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"{1}\"", fileLocation, extendedProperties);

                        string excelConnectionString = _excelIO.CreateConnectionString(fileLocation, fileExtension);
                        int ProfileID = 0;

                        ExecuteDatabase(ImportInCaseOf, excelConnectionString);
                        ExecuteDatabase(ImportLab, excelConnectionString);
                        ExecuteDatabase(ImportCity, excelConnectionString);
                        ExecuteDatabase(ImportOperator, excelConnectionString);
                        ExecuteDatabase(ImportApplicant, excelConnectionString);
                        ExecuteDatabase(ImportProfile, excelConnectionString, out ProfileID);
                        if (ImportAction == CommonConstants.ImportBTS)
                            ExecuteDatabase(ImportBts, excelConnectionString, ProfileID);
                        if (ImportAction == CommonConstants.ImportCER)
                        {
                            ExecuteDatabase(ImportCertificate, excelConnectionString, ProfileID);
                            ExecuteDatabase(ImportNoCertificate, excelConnectionString, ProfileID);
                        }

                        //_excelIO.AddNewColumns(file.FileName, CommonConstants.Sheet_InCaseOf, "NewCol1;NewCol2");
                    }
                    catch (Exception e)
                    {
                        // Base Controller đã ghi Log Error rồi
                        return Json(new { Status = CommonConstants.Status_Error, Message = e.Message }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { Status = CommonConstants.Status_Success, Message = "Import Certificate Finished !" }, JsonRequestBehavior.AllowGet);
        }

        private int ImportInCaseOf(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_InCaseOf);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_InCaseOf_ID].ToString()))
                {
                    var Item = new InCaseOf();
                    Item.ID = Convert.ToInt32(dt.Rows[i][CommonConstants.Sheet_InCaseOf_ID]);
                    Item.Name = dt.Rows[i][CommonConstants.Sheet_InCaseOf_Name].ToString();

                    _importService.Add(Item);
                    _importService.Save();
                }
            }

            return 1;
        }

        private int ImportLab(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Lab);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Lab_ID].ToString()))
                {
                    var Item = new Lab();
                    Item.ID = dt.Rows[i][CommonConstants.Sheet_Lab_ID].ToString();
                    Item.Name = dt.Rows[i][CommonConstants.Sheet_Lab_Name].ToString();
                    Item.Address = dt.Rows[i][CommonConstants.Sheet_Lab_Address].ToString();
                    Item.Phone = dt.Rows[i][CommonConstants.Sheet_Lab_Phone].ToString();
                    Item.Fax = dt.Rows[i][CommonConstants.Sheet_Lab_Fax].ToString();

                    _importService.Add(Item);
                    _importService.Save();
                }
            }

            return 1;
        }

        private int ImportCity(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_City);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_City_ID].ToString()))
                {
                    var Item = new City();
                    Item.ID = dt.Rows[i][CommonConstants.Sheet_City_ID].ToString();
                    Item.Name = dt.Rows[i][CommonConstants.Sheet_City_Name].ToString();

                    _importService.Add(Item);
                    _importService.Save();
                }
            }

            return 1;
        }

        private int ImportOperator(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Operator);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Operator_ID].ToString()))
                {
                    var Item = new Operator();
                    Item.ID = dt.Rows[i][CommonConstants.Sheet_Operator_ID].ToString();
                    Item.Name = dt.Rows[i][CommonConstants.Sheet_Operator_Name].ToString();

                    _importService.Add(Item);
                    _importService.Save();
                }
            }

            return 1;
        }

        private int ImportApplicant(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Applicant);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Applicant_ID].ToString()))
                {
                    var Item = new Applicant();
                    Item.ID = dt.Rows[i][CommonConstants.Sheet_Applicant_ID].ToString();
                    Item.Name = dt.Rows[i][CommonConstants.Sheet_Applicant_Name].ToString();
                    Item.Address = dt.Rows[i][CommonConstants.Sheet_Applicant_Address].ToString();
                    Item.Phone = dt.Rows[i][CommonConstants.Sheet_Applicant_Phone].ToString();
                    Item.Fax = dt.Rows[i][CommonConstants.Sheet_Applicant_Fax].ToString();
                    Item.ContactName = dt.Rows[i][CommonConstants.Sheet_Applicant_ContactName].ToString();
                    Item.OperatorID = dt.Rows[i][CommonConstants.Sheet_Applicant_OperatorID].ToString();

                    _importService.Add(Item);
                    _importService.Save();
                }
            }
            return 1;
        }

        private int ImportProfile(string excelConnectionString, int proFileID)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Profile);
            var Item = new Profile();
            proFileID = 0;
            if (!string.IsNullOrEmpty(dt.Rows[0][CommonConstants.Sheet_Profile_ApplicantID].ToString()))
            {
                Item.ApplicantID = dt.Rows[0][CommonConstants.Sheet_Profile_ApplicantID].ToString();
                Item.ProfileNum = dt.Rows[0][CommonConstants.Sheet_Profile_ProfileNum].ToString();
                Item.ProfileDate = DateTime.Parse(dt.Rows[0][CommonConstants.Sheet_Profile_ProfileDate].ToString());
                Item.BtsQuantity = int.Parse(dt.Rows[0][CommonConstants.Sheet_Profile_BtsQuantity].ToString());
                Item.ApplyDate = DateTime.Parse(dt.Rows[0][CommonConstants.Sheet_Profile_ApplyDate].ToString());
                if (dt.Rows[0][CommonConstants.Sheet_Profile_Fee].ToString().Length > 0)
                {
                    //int fee;
                    //int.TryParse(dt.Rows[0][CommonConstants.Sheet_Profile_Fee].ToString(), NumberStyles.Number, provider, out fee);
                    Item.Fee = int.Parse(dt.Rows[0][CommonConstants.Sheet_Profile_Fee].ToString(), NumberStyles.Number);
                }

                if (dt.Rows[0][CommonConstants.Sheet_Profile_FeeAnnounceNum].ToString().Length > 0)
                    Item.FeeAnnounceNum = dt.Rows[0][CommonConstants.Sheet_Profile_FeeAnnounceNum].ToString();
                if (dt.Rows[0][CommonConstants.Sheet_Profile_FeeAnnounceDate].ToString().Length > 0)
                    Item.FeeAnnounceDate = DateTime.Parse(dt.Rows[0][CommonConstants.Sheet_Profile_FeeAnnounceDate].ToString());
                if (dt.Rows[0][CommonConstants.Sheet_Profile_FeeReceiptDate].ToString().Length > 0)
                    Item.FeeReceiptDate = DateTime.Parse(dt.Rows[0][CommonConstants.Sheet_Profile_FeeReceiptDate].ToString());

                Profile dbProfile = _importService.findProfile(Item.ApplicantID, Item.ProfileNum, Item.ProfileDate);
                if (dbProfile != null)
                {
                    proFileID = dbProfile.ID;
                    //_importService.Update(Item);
                }
                else
                {
                    _importService.Add(Item);
                    _importService.Save();
                    proFileID = Item.ID;
                }
            }
            return proFileID;
        }

        private int ImportBts(string excelConnectionString, int proFileID)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Bts);
            string operatorID = _importService.getApplicant(proFileID).OperatorID;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString()))
                {
                    var Item = new Bts();
                    Item.OperatorID = operatorID;
                    Item.ProfileID = proFileID;
                    Item.BtsCode = dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString();
                    Item.Address = dt.Rows[i][CommonConstants.Sheet_Bts_Address].ToString();
                    Item.CityID = dt.Rows[i][CommonConstants.Sheet_Bts_CityID].ToString();
                    if (dt.Rows[i][CommonConstants.Sheet_Bts_Longtitude].ToString().Length > 0)
                        Item.Longtitude = double.Parse(dt.Rows[i][CommonConstants.Sheet_Bts_Longtitude].ToString());
                    if (dt.Rows[i][CommonConstants.Sheet_Bts_Latitude].ToString().Length > 0)
                        Item.Latitude = double.Parse(dt.Rows[i][CommonConstants.Sheet_Bts_Latitude].ToString());
                    if (dt.Rows[i][CommonConstants.Sheet_Bts_InCaseOfID].ToString().Length > 0)
                        Item.InCaseOfID = int.Parse(dt.Rows[i][CommonConstants.Sheet_Bts_InCaseOfID].ToString());

                    IEnumerable<string> certIDs = _importService.getLastOwnCertificateIDs(Item.BtsCode, operatorID);

                    if (certIDs != null)
                    {
                        Item.LastOwnCertificateIDs = string.Join(";", certIDs);
                    }

                    certIDs = _importService.getLastNoOwnCertificateIDs(Item.BtsCode, operatorID);
                    if (certIDs != null)
                    {
                        Item.LastNoOwnCertificateIDs = string.Join(";", certIDs);
                    }

                    Bts dbBts = _importService.findBts(Item.ProfileID, Item.BtsCode);
                    if (dbBts != null)
                    {
                        //_importService.Update(Item);
                    }
                    else
                    {
                        _importService.Add(Item);

                        _importService.Save();
                    }
                }
            }

            return 1;
        }

        private int ImportCertificate(string excelConnectionString, int proFileID)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Certificate);
            string operatorID = _importService.getApplicant(proFileID).OperatorID;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString()))
                {
                    var Item = new Certificate();
                    string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;

                    Item.ProfileID = proFileID;
                    Item.OperatorID = operatorID;
                    Item.ID = dt.Rows[i][CommonConstants.Sheet_Certificate_CertificateNum].ToString();
                    Item.BtsCode = dt.Rows[i][CommonConstants.Sheet_Certificate_BtsCode].ToString();
                    Item.Address = dt.Rows[i][CommonConstants.Sheet_Certificate_Address].ToString();
                    Item.CityID = dt.Rows[i][CommonConstants.Sheet_Certificate_CityID].ToString();
                    if (dt.Rows[i][CommonConstants.Sheet_Certificate_Longtitude].ToString().Length > 0)
                        Item.Longtitude = double.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_Longtitude].ToString());
                    if (dt.Rows[i][CommonConstants.Sheet_Certificate_Latitude].ToString().Length > 0)
                        Item.Latitude = double.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_Latitude].ToString());
                    if (dt.Rows[i][CommonConstants.Sheet_Certificate_InCaseOfID].ToString().Length > 0)
                        Item.InCaseOfID = int.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_InCaseOfID].ToString());

                    Item.LabID = dt.Rows[i][CommonConstants.Sheet_Certificate_LabID].ToString();

                    Item.TestReportNo = dt.Rows[i][CommonConstants.Sheet_Certificate_TestReportNo].ToString();
                    Item.TestReportDate = DateTime.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_TestReportDate].ToString());
                    Item.IssuedDate = DateTime.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_IssuedDate].ToString());
                    Item.ExpiredDate = DateTime.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_ExpiredDate].ToString());
                    Item.IssuedPlace = CommonConstants.IssuePalce;
                    Item.Signer = CommonConstants.Signer;
                    Item.SubBtsQuantity = Int32.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsQuantity].ToString());

                    if (dt.Rows[i][CommonConstants.Sheet_Certificate_MinAntenHeight].ToString().Length > 0)
                        Item.MinAntenHeight = Double.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_MinAntenHeight].ToString());

                    if (dt.Rows[i][CommonConstants.Sheet_Certificate_MaxHeightIn100m].ToString().Length > 0)
                        Item.MaxHeightIn100m = Double.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_MaxHeightIn100m].ToString());

                    Item.OffsetHeight = Item.MinAntenHeight - Item.MaxHeightIn100m;

                    //if (dt.Rows[i][CommonConstants.Sheet_Certificate_OffsetHeight].ToString().Length > 0)
                    //    Item.OffsetHeight = Double.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_OffsetHeight].ToString());

                    if (dt.Rows[i][CommonConstants.Sheet_Certificate_SafeLimit].ToString().Length > 0)
                        Item.SafeLimit = Double.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_SafeLimit].ToString());

                    Item.SubBtsAntenHeights = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsAntenHeights].ToString();
                    SubBtsAntenHeights = Item.SubBtsAntenHeights.Split(new char[] { ';' });

                    Item.SubBtsAntenNums = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsAntenNums].ToString();
                    SubBtsAntenNums = Item.SubBtsAntenNums.Split(new char[] { ';' });

                    Item.SharedAntens = dt.Rows[i][CommonConstants.Sheet_Certificate_SharedAntens].ToString();

                    Item.MeasuringExposure = bool.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_MeasuringExposure].ToString());

                    Item.SubBtsBands = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsBands].ToString();
                    SubBtsBands = Item.SubBtsBands.Split(new char[] { ';' });

                    Item.SubBtsCodes = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsCodes].ToString();
                    SubBtsCodes = Item.SubBtsCodes.Split(new char[] { ';' });

                    Item.SubBtsConfigurations = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsConfigurations].ToString();
                    SubBtsConfigurations = Item.SubBtsConfigurations.Split(new char[] { ';' });

                    Item.SubBtsEquipments = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsEquipments].ToString();
                    SubBtsEquipments = Item.SubBtsEquipments.Split(new char[] { ';' });

                    Item.SubBtsOperatorIDs = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsOperatorIDs].ToString();
                    SubBtsOperatorIDs = Item.SubBtsOperatorIDs.Split(new char[] { ';' });

                    Item.SubBtsPowerSums = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsPowerSums].ToString();
                    SubBtsPowerSums = Item.SubBtsPowerSums.Split(new char[] { ';' });

                    Certificate dbCertificate = _importService.findCertificate(Item.ID);

                    if (dbCertificate != null)
                    {
                        //_importService.Update(Item);
                        //_importService.RemoveSubBtsInCert(Item.ID);
                    }
                    else
                    {
                        _importService.Add(Item);

                        Bts dbBts = _importService.findBts(proFileID, Item.BtsCode);
                        if (dbBts != null)
                        {
                            _importService.Delete(dbBts);
                            _importService.Save();
                        }
                        _importService.Save();

                        for (int j = 0; j < Item.SubBtsQuantity; j++)
                        {
                            SubBtsInCert subBtsItem = new SubBtsInCert();
                            subBtsItem.CertificateID = Item.ID;
                            subBtsItem.BtsCode = SubBtsCodes[j];
                            subBtsItem.OperatorID = SubBtsOperatorIDs[j];
                            subBtsItem.AntenHeight = SubBtsAntenHeights[j];
                            subBtsItem.AntenNum = Int32.Parse(SubBtsAntenNums[j]);
                            subBtsItem.Band = SubBtsBands[j];
                            subBtsItem.Configuration = SubBtsConfigurations[j];
                            subBtsItem.Equipment = SubBtsEquipments[j];
                            subBtsItem.Manufactory = subBtsItem.Equipment.Substring(0, subBtsItem.Equipment.IndexOf(' '));
                            subBtsItem.PowerSum = SubBtsPowerSums[j];

                            _importService.Add(subBtsItem);
                            _importService.Save();
                        }
                    }
                }
            }
            return 1;
        }

        private int ImportNoCertificate(string excelConnectionString, int proFileID)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_NoCertificate);
            string operatorID = _importService.getApplicant(proFileID).OperatorID;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString()))
                {
                    var Item = new NoCertificate();
                    string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;

                    Item.ProfileID = proFileID;
                    Item.OperatorID = operatorID;
                    Item.BtsCode = dt.Rows[i][CommonConstants.Sheet_NoCertificate_BtsCode].ToString();
                    Item.Address = dt.Rows[i][CommonConstants.Sheet_NoCertificate_Address].ToString();
                    Item.CityID = dt.Rows[i][CommonConstants.Sheet_NoCertificate_CityID].ToString();
                    if (dt.Rows[i][CommonConstants.Sheet_NoCertificate_Longtitude].ToString().Length > 0)
                        Item.Longtitude = double.Parse(dt.Rows[i][CommonConstants.Sheet_NoCertificate_Longtitude].ToString());
                    if (dt.Rows[i][CommonConstants.Sheet_NoCertificate_Latitude].ToString().Length > 0)
                        Item.Latitude = double.Parse(dt.Rows[i][CommonConstants.Sheet_NoCertificate_Latitude].ToString());
                    if (dt.Rows[i][CommonConstants.Sheet_NoCertificate_InCaseOfID].ToString().Length > 0)
                        Item.InCaseOfID = int.Parse(dt.Rows[i][CommonConstants.Sheet_NoCertificate_InCaseOfID].ToString());

                    Item.LabID = dt.Rows[i][CommonConstants.Sheet_NoCertificate_LabID].ToString();

                    Item.TestReportNo = dt.Rows[i][CommonConstants.Sheet_NoCertificate_TestReportNo].ToString();
                    Item.TestReportDate = DateTime.Parse(dt.Rows[i][CommonConstants.Sheet_NoCertificate_TestReportDate].ToString());
                    Item.Reason = dt.Rows[i][CommonConstants.Sheet_NoCertificate_Reason].ToString();

                    _importService.Add(Item);

                    Bts dbBts = _importService.findBts(proFileID, Item.BtsCode);
                    if (dbBts != null)
                    {
                        _importService.Delete(dbBts);
                        _importService.Save();
                    }
                }
            }
            return 1;
        }
    }
}