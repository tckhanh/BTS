using BTS.Common;
using BTS.Model.Models;
using BTS.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace BTS.Web.Controllers
{
    public class ImportController : BaseController
    {
        // GET: Import
        private IImportService _importService;

        public ImportController(IImportService importService, IErrorService errorService) : base(errorService)
        {
            this._importService = importService;
        }

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
                }
            }
            return View();
        }

        private bool ImportInCaseOf(string excelConnectionString)
        {
            DataSet ds = new DataSet();
            //Create Connection to Excel work book and add oledb namespace
            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
            // Import Data from Sheet_InCaseOf
            string query = string.Format("Select * from [{0}]", CommonConstants.Sheet_InCaseOf);
            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
            {
                dataAdapter.Fill(ds);
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var Item = new InCaseOf();
                Item.ID = Convert.ToInt32(ds.Tables[0].Rows[i][CommonConstants.Sheet_InCaseOf_ID]);
                Item.Name = ds.Tables[0].Rows[i][CommonConstants.Sheet_InCaseOf_Name].ToString();
                _importService.Add(Item);
            }
            excelConnection.Close();

            _importService.Save();

            return true;
        }

        private bool ImportLab(string excelConnectionString)
        {
            DataSet ds = new DataSet();
            //Create Connection to Excel work book and add oledb namespace
            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
            // Import Data from Sheet_Lab
            string query = string.Format("Select * from [{0}]", CommonConstants.Sheet_Lab);
            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
            {
                dataAdapter.Fill(ds);
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var Item = new Lab();
                Item.ID = ds.Tables[0].Rows[i][CommonConstants.Sheet_Lab_ID].ToString();
                Item.Name = ds.Tables[0].Rows[i][CommonConstants.Sheet_Lab_Name].ToString();
                Item.Address = ds.Tables[0].Rows[i][CommonConstants.Sheet_Lab_Address].ToString();
                Item.Phone = ds.Tables[0].Rows[i][CommonConstants.Sheet_Lab_Phone].ToString();
                Item.Fax = ds.Tables[0].Rows[i][CommonConstants.Sheet_Lab_Fax].ToString();
                _importService.Add(Item);
            }
            excelConnection.Close();
            _importService.Save();

            return true;
        }

        private bool ImportCity(string excelConnectionString)
        {
            DataSet ds = new DataSet();
            //Create Connection to Excel work book and add oledb namespace
            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
            // Import Data from Sheet_City
            string query = string.Format("Select * from [{0}]", CommonConstants.Sheet_City);
            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
            {
                dataAdapter.Fill(ds);
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var Item = new City();
                Item.ID = ds.Tables[0].Rows[i][CommonConstants.Sheet_City_ID].ToString();
                Item.Name = ds.Tables[0].Rows[i][CommonConstants.Sheet_City_Name].ToString();
                _importService.Add(Item);
            }
            excelConnection.Close();
            _importService.Save();

            return true;
        }

        private bool ImportOperator(string excelConnectionString)
        {
            DataSet ds = new DataSet();
            //Create Connection to Excel work book and add oledb namespace
            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
            // Import Data from Sheet_City
            string query = string.Format("Select * from [{0}]", CommonConstants.Sheet_Operator);
            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
            {
                dataAdapter.Fill(ds);
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var Item = new Operator();
                Item.ID = ds.Tables[0].Rows[i][CommonConstants.Sheet_Operator_ID].ToString();
                Item.Name = ds.Tables[0].Rows[i][CommonConstants.Sheet_Operator_Name].ToString();
                _importService.Add(Item);
            }
            excelConnection.Close();
            _importService.Save();

            return true;
        }

        private bool ImportApplicant(string excelConnectionString)
        {
            DataSet ds = new DataSet();
            //Create Connection to Excel work book and add oledb namespace
            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
            // Import Data from Sheet_City
            string query = string.Format("Select * from [{0}]", CommonConstants.Sheet_Applicant);
            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
            {
                dataAdapter.Fill(ds);
            }

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
            }
            excelConnection.Close();
            _importService.Save();

            return true;
        }
    }
}