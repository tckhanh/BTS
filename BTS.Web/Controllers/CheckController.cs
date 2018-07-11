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
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
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

        //[ValidateAntiForgeryToken]
        public JsonResult loadBTS()
        {
            string fileLocation = "", fileExtension = "";

            if (Request.Form.GetValues("fileLocation") != null)
                fileLocation = Request.Form.GetValues("fileLocation").FirstOrDefault();
            if (Request.Form.GetValues("fileExtension") != null)
                fileExtension = Request.Form.GetValues("fileExtension").FirstOrDefault();
            if (string.IsNullOrEmpty(fileLocation) || string.IsNullOrEmpty(fileExtension))
            {
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
            }

            string excelConnectionString = _excelIO.CreateConnectionString(fileLocation, fileExtension);
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Bts);
            List<Bts> dataResult = new List<Bts>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataResult.Add(new Bts()
                {
                    OperatorID = dt.Rows[i][CommonConstants.Sheet_Bts_OperatorID].ToString(),
                    BtsCode = dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString(),
                    Address = dt.Rows[i][CommonConstants.Sheet_Bts_Address].ToString(),
                    LastOwnCertificateIDs = dt.Rows[i][CommonConstants.Sheet_Bts_LastOwnCertificateIDs].ToString(),
                    ProfilesInProcess = dt.Rows[i][CommonConstants.Sheet_Bts_ProfileInProcess].ToString(),
                    ReasonsNoCertificate = dt.Rows[i][CommonConstants.Sheet_Bts_ReasonNoCertificate].ToString(),
                });
            }

            if (dt.Rows.Count > 0)
            {
                //var tbcat = from c in dataViewModel select new { c.Id, c.title, c.descriptions, action = "<a href='" + Url.Action("edit", "Category", new { id = c.Id }) + "'>Edit</a> | <a href='javascript:;' onclick='MyStore.Delete(" + c.Id + ")'>Delete</a>" };
                return Json(new { data = dataResult }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckBTS(HttpPostedFileBase file)
        {
            string fileExtension = "";
            string fileLocation = "";
            try
            {
                if (Request.Files["file"].ContentLength > 0)
                {
                    fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        string tmpFileName = Path.GetTempFileName();
                        fileLocation = Server.MapPath("~/AppFiles/Tmp/") + Request.Files["file"].FileName;

                        if (System.IO.File.Exists(fileLocation))
                            System.IO.File.Delete(fileLocation);

                        Request.Files["file"].SaveAs(fileLocation);

                        //_excelIO.AddNewColumns(file.FileName, CommonConstants.Sheet_InCaseOf, "NewCol1;NewCol2");
                        _excelIO.AddNewColumns(fileLocation, CommonConstants.Sheet_Bts, CommonConstants.Sheet_Bts_LastOwnCertificateIDs + ";" + CommonConstants.Sheet_Bts_LastNoOwnCertificateIDs + ";" + CommonConstants.Sheet_Bts_ProfileInProcess + ";" + CommonConstants.Sheet_Bts_ReasonNoCertificate);

                        string[] columnNames = new string[] {
                            CommonConstants.Sheet_Certificate_Longtitude,
                            CommonConstants.Sheet_Certificate_Latitude,
                            CommonConstants.Sheet_Certificate_MaxHeightIn100m,
                            CommonConstants.Sheet_Certificate_MinAntenHeight,
                            CommonConstants.Sheet_Certificate_OffsetHeight,
                            CommonConstants.Sheet_Certificate_SafeLimit,
                            CommonConstants.Sheet_Certificate_BtsCode};
                        _excelIO.FormatColumnDecimalToText(fileLocation, columnNames);

                        string excelConnectionStringforUpdate = _excelIO.CreateConnectionStringForUpdate(fileLocation, fileExtension);
                        ExecuteDatabase(UpdateCheckResult, excelConnectionStringforUpdate);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message, fileLocation = fileLocation, fileExtension = fileExtension }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = CommonConstants.Status_Success, message = "Check BTS Finished !", fileLocation = fileLocation, fileExtension = fileExtension }, JsonRequestBehavior.AllowGet);
        }

        private int UpdateCheckResult(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Bts);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Bts_OperatorID].ToString()) && !string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString()))
                {
                    IEnumerable<string> ownCertIDs = _importService.getLastOwnCertificateIDs(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString(), dt.Rows[i][CommonConstants.Sheet_Bts_OperatorID].ToString());
                    if (ownCertIDs != null)
                    {
                        dt.Rows[i][CommonConstants.Sheet_Bts_LastOwnCertificateIDs] = string.Join("; ", ownCertIDs);
                    }

                    IEnumerable<string> noOwncertIDs = _importService.getLastNoOwnCertificateIDs(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString(), dt.Rows[i][CommonConstants.Sheet_Bts_OperatorID].ToString());
                    if (noOwncertIDs != null)
                    {
                        dt.Rows[i][CommonConstants.Sheet_Bts_LastNoOwnCertificateIDs] = string.Join("; ", noOwncertIDs);
                    }

                    string dataString = "";
                    IEnumerable<Profile> ProfilesBtsInProcess = _importService.findProfilesBtsInProcess(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString(), dt.Rows[i][CommonConstants.Sheet_Bts_OperatorID].ToString());
                    foreach (var item in ProfilesBtsInProcess)
                    {
                        dataString += "Số " + item.ProfileNum + " ngày " + item.ProfileDate.ToString("dd/MM/yyyy") + " của " + item.ApplicantID + "\n";
                    }
                    dt.Rows[i][CommonConstants.Sheet_Bts_ProfileInProcess] = dataString;

                    dataString = "";
                    IEnumerable<NoCertificate> btsNoCertificate = _importService.findBtsNoCertificate(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString(), dt.Rows[i][CommonConstants.Sheet_Bts_OperatorID].ToString());
                    foreach (var item in btsNoCertificate)
                    {
                        dataString += item.Reason + "\n";
                    }
                    dt.Rows[i][CommonConstants.Sheet_Bts_ReasonNoCertificate] = dataString;
                }
            }

            if (_excelIO.UpdateDataInSheet(excelConnectionString, CommonConstants.Sheet_Bts, CommonConstants.Sheet_Bts_BtsCode, CommonConstants.Sheet_Bts_LastOwnCertificateIDs, dt))
                if (_excelIO.UpdateDataInSheet(excelConnectionString, CommonConstants.Sheet_Bts, CommonConstants.Sheet_Bts_BtsCode, CommonConstants.Sheet_Bts_LastNoOwnCertificateIDs, dt))
                    if (_excelIO.UpdateDataInSheet(excelConnectionString, CommonConstants.Sheet_Bts, CommonConstants.Sheet_Bts_BtsCode, CommonConstants.Sheet_Bts_ProfileInProcess, dt))
                        if (_excelIO.UpdateDataInSheet(excelConnectionString, CommonConstants.Sheet_Bts, CommonConstants.Sheet_Bts_BtsCode, CommonConstants.Sheet_Bts_ReasonNoCertificate, dt))
                            return 1;

            return 0;
        }
    }
}