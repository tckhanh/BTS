using BTS.Common;
using BTS.Common.ViewModels;
using BTS.ExcelLib;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    public class CheckController : BaseController
    {
        // GET: Import
        private IImportService _importService;

        private NumberFormatInfo provider;
        private ExcelIO _excelIO;

        public CheckController(IImportService importService, IErrorService errorService) : base(errorService)
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
        public ActionResult CheckBTS(HttpPostedFileBase file)
        {
            try
            {
                if (Request.Files["file"].ContentLength > 0)
                {
                    string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        string tmpFileName = Path.GetTempFileName();
                        string fileLocation = Server.MapPath("~/ImportData/") + Request.Files["file"].FileName;

                        if (System.IO.File.Exists(fileLocation))
                            System.IO.File.Delete(fileLocation);

                        Request.Files["file"].SaveAs(fileLocation);
                        _excelIO.AddNewColumns(fileLocation, CommonConstants.Sheet_Bts, CommonConstants.Sheet_Bts_LastCertificateNo);
                        _excelIO.FormatColumnDecimalToText(fileLocation);

                        string excelConnectionString, excelConnectionStringforUpdate = string.Empty;
                        //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0 ;HDR=Yes;IMEX=2\"";
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0 Xml; HDR=Yes;IMEX=1; TypeGuessRows=0;ImportMixedTypes=Text\"";
                        excelConnectionStringforUpdate = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0 Xml; HDR=Yes; ReadOnly=false; IMEX=0; TypeGuessRows=0;ImportMixedTypes=Text\"";
                        //connection String for xls file format.
                        if (fileExtension == ".xls")
                        {
                            //excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"";
                            excelConnectionStringforUpdate = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes; ReadOnly=false; IMEX=0; TypeGuessRows=0;ImportMixedTypes=Text\"";
                        }
                        //connection String for xlsx file format.
                        else if (fileExtension == ".xlsx")
                        {
                            //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0 Xml; HDR=Yes;IMEX=1; TypeGuessRows=0;ImportMixedTypes=Text\"";
                            excelConnectionStringforUpdate = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0 Xml; HDR=Yes; ReadOnly=false; IMEX=0; TypeGuessRows=0;ImportMixedTypes=Text\"";
                        }

                        ExecuteDatabase(UpdateLastCertificate, excelConnectionStringforUpdate);

                        //_excelIO.AddNewColumns(file.FileName, CommonConstants.Sheet_InCaseOf, "NewCol1;NewCol2");
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = CommonConstants.Status_Error, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = CommonConstants.Status_Success, Message = "Check BTS Finished !" }, JsonRequestBehavior.AllowGet);
        }

        private int UpdateLastCertificate(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Bts);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Bts_OperatorID].ToString()) && !string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString()))
                {
                    Certificate cert = _importService.getLastOwnCertificate(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString(), dt.Rows[i][CommonConstants.Sheet_Bts_OperatorID].ToString());
                    if (cert != null)
                        dt.Rows[i][CommonConstants.Sheet_Bts_LastCertificateNo] = cert.ID;
                }
            }

            if (_excelIO.UpdateDataInSheet(excelConnectionString, CommonConstants.Sheet_Bts, CommonConstants.Sheet_Bts_BtsCode, CommonConstants.Sheet_Bts_LastCertificateNo, dt))
                return 1;
            else
                return 0;
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
                if (dt.Rows[0][CommonConstants.Sheet_Profile_ValidDate].ToString().Length > 0)
                    Item.ValidDate = DateTime.Parse(dt.Rows[0][CommonConstants.Sheet_Profile_ValidDate].ToString());
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

                    Certificate lastOwnCertificate = _importService.getLastOwnCertificate(Item.BtsCode, operatorID);
                    if (lastOwnCertificate != null)
                    {
                        Item.LastOwnCertificateID = lastOwnCertificate.ID;
                        Item.LastOwnOperatorID = lastOwnCertificate.OperatorID;
                    }

                    Certificate lastNoOwnCertificate = _importService.getLastNoOwnCertificate(Item.BtsCode, operatorID);
                    if (lastNoOwnCertificate != null)
                    {
                        Item.LastNoOwnCertificateID = lastNoOwnCertificate.ID;
                        Item.LastNoOwnOperatorID = lastNoOwnCertificate.OperatorID;
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
                            dbBts.IssuedCertificateID = Item.ID;
                            dbBts.UpdatedDate = DateTime.Now;
                            _importService.Update(dbBts);
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
    }
}