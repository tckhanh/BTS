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
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    public class ImportController : BaseController
    {
        // GET: Import
        private IImportService _importService;

        private NumberFormatInfo provider;

        public ImportController(IImportService importService, IErrorService errorService) : base(errorService)
        {
            this._importService = importService;
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
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/ImportData/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);

                    string excelConnectionString = string.Empty;
                    //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0 ;HDR=Yes;IMEX=2\"";
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0 Xml; HDR=Yes\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0 Xml; HDR=Yes\"";
                    }

                    ExecuteDatabase(ImportInCaseOf, excelConnectionString);
                    ExecuteDatabase(ImportLab, excelConnectionString);
                    ExecuteDatabase(ImportCity, excelConnectionString);
                    ExecuteDatabase(ImportOperator, excelConnectionString);
                    ExecuteDatabase(ImportApplicant, excelConnectionString);
                    int ProfileID = ((Message)ExecuteDatabase(ImportProfile, excelConnectionString)).ID;
                    ExecuteDatabase(ImportBts, excelConnectionString, ProfileID);
                    ExecuteDatabase(ImportCertificate, excelConnectionString, ProfileID);
                    //ExcelIO.AddNewColumns(file.FileName, CommonConstants.Sheet_InCaseOf, "NewCol1;NewCol2");
                }
            }
            return View();
        }

        private int ImportInCaseOf(string excelConnectionString)
        {
            DataSet ds = ExcelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_InCaseOf);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var Item = new InCaseOf();
                Item.ID = Convert.ToInt32(ds.Tables[0].Rows[i][CommonConstants.Sheet_InCaseOf_ID]);
                Item.Name = ds.Tables[0].Rows[i][CommonConstants.Sheet_InCaseOf_Name].ToString();

                _importService.Add(Item);
                _importService.Save();
            }

            return 1;
        }

        private int ImportLab(string excelConnectionString)
        {
            DataSet ds = ExcelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Lab);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var Item = new Lab();
                Item.ID = ds.Tables[0].Rows[i][CommonConstants.Sheet_Lab_ID].ToString();
                Item.Name = ds.Tables[0].Rows[i][CommonConstants.Sheet_Lab_Name].ToString();
                Item.Address = ds.Tables[0].Rows[i][CommonConstants.Sheet_Lab_Address].ToString();
                Item.Phone = ds.Tables[0].Rows[i][CommonConstants.Sheet_Lab_Phone].ToString();
                Item.Fax = ds.Tables[0].Rows[i][CommonConstants.Sheet_Lab_Fax].ToString();

                _importService.Add(Item);
                _importService.Save();
            }

            return 1;
        }

        private int ImportCity(string excelConnectionString)
        {
            DataSet ds = ExcelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_City);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var Item = new City();
                Item.ID = ds.Tables[0].Rows[i][CommonConstants.Sheet_City_ID].ToString();
                Item.Name = ds.Tables[0].Rows[i][CommonConstants.Sheet_City_Name].ToString();

                _importService.Add(Item);
                _importService.Save();
            }

            return 1;
        }

        private int ImportOperator(string excelConnectionString)
        {
            DataSet ds = ExcelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Operator);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var Item = new Operator();
                Item.ID = ds.Tables[0].Rows[i][CommonConstants.Sheet_Operator_ID].ToString();
                Item.Name = ds.Tables[0].Rows[i][CommonConstants.Sheet_Operator_Name].ToString();

                _importService.Add(Item);
                _importService.Save();
            }

            return 1;
        }

        private int ImportApplicant(string excelConnectionString)
        {
            DataSet ds = ExcelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Applicant);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var Item = new Applicant();
                Item.ID = ds.Tables[0].Rows[i][CommonConstants.Sheet_Applicant_ID].ToString();
                Item.Name = ds.Tables[0].Rows[i][CommonConstants.Sheet_Applicant_Name].ToString();
                Item.Address = ds.Tables[0].Rows[i][CommonConstants.Sheet_Applicant_Address].ToString();
                Item.Phone = ds.Tables[0].Rows[i][CommonConstants.Sheet_Applicant_Phone].ToString();
                Item.Fax = ds.Tables[0].Rows[i][CommonConstants.Sheet_Applicant_Fax].ToString();
                Item.ContactName = ds.Tables[0].Rows[i][CommonConstants.Sheet_Applicant_ContactName].ToString();
                Item.OperatorID = ds.Tables[0].Rows[i][CommonConstants.Sheet_Applicant_OperatorID].ToString();

                _importService.Add(Item);
                _importService.Save();
            }
            return 1;
        }

        private int ImportProfile(string excelConnectionString)
        {
            DataSet ds = ExcelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Profile);
            var Item = new Profile();
            Item.ApplicantID = ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_ApplicantID].ToString();
            Item.ProfileNum = ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_ProfileNum].ToString();
            Item.ProfileDate = DateTime.Parse(ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_ProfileDate].ToString());
            Item.BtsQuantity = Int32.Parse(ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_BtsQuantity].ToString());
            Item.ApplyDate = DateTime.Parse(ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_ApplyDate].ToString());
            if (ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_ValidDate].ToString().Length > 0)
                Item.ValidDate = DateTime.Parse(ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_ValidDate].ToString());
            if (ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_Fee].ToString().Length > 0)
                Item.Fee = Int32.Parse(ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_Fee].ToString());
            if (ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_FeeAnnounceNum].ToString().Length > 0)
                Item.FeeAnnounceNum = ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_FeeAnnounceNum].ToString();
            if (ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_FeeAnnounceDate].ToString().Length > 0)
                Item.FeeAnnounceDate = DateTime.Parse(ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_FeeAnnounceDate].ToString());
            if (ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_FeeReceiptDate].ToString().Length > 0)
                Item.FeeReceiptDate = DateTime.Parse(ds.Tables[0].Rows[0][CommonConstants.Sheet_Profile_FeeReceiptDate].ToString());

            Profile dbProfile = _importService.findProfile(Item.ApplicantID, Item.ProfileNum, Item.ProfileDate);
            if (dbProfile != null)
            {
                Item.ID = dbProfile.ID;
                //_importService.Update(Item);
            }
            else
            {
                _importService.Add(Item);
                _importService.Save();
            }

            return Item.ID;
        }

        private int ImportBts(string excelConnectionString, int proFileID)
        {
            DataSet ds = ExcelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Bts);
            string operatorID = _importService.getApplicant(proFileID).OperatorID;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var Item = new Bts();
                Item.OperatorID = operatorID;
                Item.ProfileID = proFileID;
                Item.BtsCode = ds.Tables[0].Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString();
                Item.Address = ds.Tables[0].Rows[i][CommonConstants.Sheet_Bts_Address].ToString();
                Item.CityID = ds.Tables[0].Rows[i][CommonConstants.Sheet_Bts_CityID].ToString();
                if (ds.Tables[0].Rows[i][CommonConstants.Sheet_Bts_Longtitude].ToString().Length > 0)
                    Item.Longtitude = Convert.ToDouble(ds.Tables[0].Rows[i][CommonConstants.Sheet_Bts_Longtitude].ToString(), provider);
                if (ds.Tables[0].Rows[i][CommonConstants.Sheet_Bts_Latitude].ToString().Length > 0)
                    Item.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i][CommonConstants.Sheet_Bts_Latitude].ToString(), provider);
                if (ds.Tables[0].Rows[i][CommonConstants.Sheet_Bts_InCaseOfID].ToString().Length > 0)
                    Item.InCaseOfID = Int32.Parse(ds.Tables[0].Rows[i][CommonConstants.Sheet_Bts_InCaseOfID].ToString());

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

            return 1;
        }

        private int ImportCertificate(string excelConnectionString, int proFileID)
        {
            DataSet ds = ExcelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Certificate);
            string operatorID = _importService.getApplicant(proFileID).OperatorID;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var Item = new Certificate();
                string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;

                Item.ProfileID = proFileID;
                Item.OperatorID = operatorID;
                Item.ID = ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_CertificateNum].ToString();
                Item.BtsCode = ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_BtsCode].ToString();
                Item.Address = ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_Address].ToString();
                Item.CityID = ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_CityID].ToString();
                if (ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_Longtitude].ToString().Length > 0)
                    Item.Longtitude = Double.Parse(ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_Longtitude].ToString());
                if (ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_Latitude].ToString().Length > 0)
                    Item.Latitude = Double.Parse(ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_Latitude].ToString());
                if (ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_InCaseOfID].ToString().Length > 0)
                    Item.InCaseOfID = Int32.Parse(ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_InCaseOfID].ToString());

                Item.LabID = ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_LabID].ToString();

                Item.TestReportNo = ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_TestReportNo].ToString();
                Item.TestReportDate = DateTime.Parse(ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_TestReportDate].ToString());
                Item.IssuedDate = DateTime.Parse(ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_IssuedDate].ToString());
                Item.ExpiredDate = DateTime.Parse(ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_ExpiredDate].ToString());
                Item.IssuedPlace = CommonConstants.IssuePalce;
                Item.Signer = CommonConstants.Signer;
                Item.SubBtsQuantity = Int32.Parse(ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_SubBtsQuantity].ToString());

                if (ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_MinAntenHeight].ToString().Length > 0)
                    Item.MinAntenHeight = Double.Parse(ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_MinAntenHeight].ToString());

                if (ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_MaxHeightIn100m].ToString().Length > 0)
                    Item.MaxHeightIn100m = Double.Parse(ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_MaxHeightIn100m].ToString());

                if (ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_OffsetHeight].ToString().Length > 0)
                    Item.OffsetHeight = Double.Parse(ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_OffsetHeight].ToString());

                if (ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_SafeLimit].ToString().Length > 0)
                    Item.SafeLimit = Double.Parse(ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_SafeLimit].ToString());

                Item.SubBtsAntenHeights = ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_SubBtsAntenHeights].ToString();
                SubBtsAntenHeights = Item.SubBtsAntenHeights.Split(new char[] { ';' });

                Item.SubBtsAntenNums = ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_SubBtsAntenNums].ToString();
                SubBtsAntenNums = Item.SubBtsAntenNums.Split(new char[] { ';' });

                Item.SubBtsBands = ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_SubBtsBands].ToString();
                SubBtsBands = Item.SubBtsBands.Split(new char[] { ';' });

                Item.SubBtsCodes = ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_SubBtsCodes].ToString();
                SubBtsCodes = Item.SubBtsCodes.Split(new char[] { ';' });

                Item.SubBtsConfigurations = ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_SubBtsConfigurations].ToString();
                SubBtsConfigurations = Item.SubBtsConfigurations.Split(new char[] { ';' });

                Item.SubBtsEquipments = ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_SubBtsEquipments].ToString();
                SubBtsEquipments = Item.SubBtsEquipments.Split(new char[] { ';' });

                Item.SubBtsOperatorIDs = ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_SubBtsOperatorIDs].ToString();
                SubBtsOperatorIDs = Item.SubBtsOperatorIDs.Split(new char[] { ';' });

                Item.SubBtsPowerSums = ds.Tables[0].Rows[i][CommonConstants.Sheet_Certificate_SubBtsPowerSums].ToString();
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
            return 1;
        }
    }
}