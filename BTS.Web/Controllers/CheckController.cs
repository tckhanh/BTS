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
        [ValidateAntiForgeryToken]
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

                        ExecuteDatabase(UpdateLastCertificateNo, excelConnectionStringforUpdate);

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

        private int UpdateLastCertificateNo(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Bts);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Bts_OperatorID].ToString()) && !string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString()))
                {
                    IEnumerable<string> certIDs = _importService.getLastOwnCertificateIDs(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode].ToString(), dt.Rows[i][CommonConstants.Sheet_Bts_OperatorID].ToString());
                    if (certIDs != null)
                    {
                        dt.Rows[i][CommonConstants.Sheet_Bts_LastCertificateNo] = string.Join("; ", certIDs);
                    }
                }
            }

            if (_excelIO.UpdateDataInSheet(excelConnectionString, CommonConstants.Sheet_Bts, CommonConstants.Sheet_Bts_BtsCode, CommonConstants.Sheet_Bts_LastCertificateNo, dt))
                return 1;
            else
                return 0;
        }
    }
}