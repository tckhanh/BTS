using AutoMapper;
using BTS.Common;
using BTS.Common.ViewModels;
using BTS.Data;
using BTS.ExcelLib;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using Profile = BTS.Model.Models.Profile;

namespace BTS.Web.Areas.Controllers
{
    [AuthorizeRoles(CommonConstants.Data_CanImport_Role)]
    public class ImportDataController : BaseController
    {
        // GET: Import
        private IImportService _importService;

        private EpplusIO _excelIO;
        private NumberFormatInfo provider;
        private string profileID;

        public ImportDataController(IImportService importService, IErrorService errorService) : base(errorService)
        {
            this._importService = importService;
            _excelIO = new EpplusIO(errorService);
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
        public ActionResult Index(HttpPostedFileBase file, string ImportAction, string InputType, bool IsSigned)
        {
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files["file"].FileName);
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx" || fileExtension == ".xlsm")
                {
                    //string fileLocation = Server.MapPath("~/AppFiles/Tmp/") + Request.Files["file"].FileName;
                    string fileLocation = Server.MapPath("~/AppFiles/Tmp/") + "InputData" + DateTime.Now.ToString("yyyymmssfff") + fileExtension;
                    try
                    {
                        if (System.IO.File.Exists(fileLocation))
                            System.IO.File.Delete(fileLocation);

                        Request.Files["file"].SaveAs(fileLocation);

                        string[] sheetNames = new string[] {
                            CommonConstants.Sheet_Certificate,
                            CommonConstants.Sheet_NoCertificate,
                        };

                        string[] columnNames = new string[] {
                            CommonConstants.Sheet_Certificate_Longtitude,
                            CommonConstants.Sheet_Certificate_Latitude,
                            CommonConstants.Sheet_Certificate_MaxHeightIn100m,
                            CommonConstants.Sheet_Certificate_MinAntenHeight,
                            CommonConstants.Sheet_Certificate_OffsetHeight,
                            CommonConstants.Sheet_Certificate_SafeLimitHeight,
                            CommonConstants.Sheet_Certificate_BtsCode};
                        _excelIO.FormatColumns(fileLocation, sheetNames, columnNames, "@");

                        //string extendedProperties = "Excel 12.0;HDR=YES;IMEX=1";
                        //string connectionString1 = string.Format(CultureInfo.CurrentCulture, "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"{1}\"", fileLocation, extendedProperties);

                        ExecuteDatabase(ImportInCaseOf, fileLocation);
                        ExecuteDatabase(ImportLab, fileLocation);
                        ExecuteDatabase(ImportCity, fileLocation);
                        ExecuteDatabase(ImportOperator, fileLocation);
                        ExecuteDatabase(ImportApplicant, fileLocation);

                        ExecuteDatabase(ImportProfile, fileLocation);
                        if (ImportAction == CommonConstants.ImportBTS)
                            ExecuteDatabase(ImportBts, fileLocation, InputType, profileID);
                        if (ImportAction == CommonConstants.ImportCER)
                        {
                            ExecuteDatabase(ImportCertificate, fileLocation, InputType, profileID, IsSigned);
                            ExecuteDatabase(ImportNoCertificate, fileLocation, InputType, profileID, IsSigned);
                        }

                        //_excelIO.AddNewColumns(file.FileName, CommonConstants.Sheet_InCaseOf, "NewCol1;NewCol2");
                    }
                    catch (Exception e)
                    {
                        // Base Controller đã ghi Log Error rồi
                        //return Json(new { status = CommonConstants.Status_Error, message = e.Message}, JsonRequestBehavior.AllowGet);
                        //return Json(new { status = CommonConstants.Status_Error, message = e.Message + "\n" + e.StackTrace }, JsonRequestBehavior.AllowGet);
                        return Json(new { status = CommonConstants.Status_Error, message = e.Message }, JsonRequestBehavior.AllowGet);
                        

                    }
                }
            }
            return Json(new { status = CommonConstants.Status_Success, message = "Import Certificate Finished !" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Area()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Area(HttpPostedFileBase file, string ImportAction, string InputType)
        {
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files["file"].FileName);
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx" || fileExtension == ".xlsm")
                {
                    //string fileLocation = Server.MapPath("~/AppFiles/Tmp/") + Request.Files["file"].FileName;
                    string fileLocation = Server.MapPath("~/AppFiles/Tmp/") + "Area" + DateTime.Now.ToString("yyyymmssfff") + fileExtension;
                    try
                    {
                        if (System.IO.File.Exists(fileLocation))
                            System.IO.File.Delete(fileLocation);

                        Request.Files["file"].SaveAs(fileLocation);

                        string[] sheetNames = new string[] {
                            CommonConstants.Sheet_Area,
                        };


                        string[] columnNames = new string[] {
                            CommonConstants.Sheet_Area_WardId,
                            CommonConstants.Sheet_Area_DistrictId };
                        _excelIO.FormatColumns(fileLocation, sheetNames, columnNames, "@");

                        //string extendedProperties = "Excel 12.0;HDR=YES;IMEX=1";
                        //string connectionString1 = string.Format(CultureInfo.CurrentCulture, "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"{1}\"", fileLocation, extendedProperties);
                        if (ImportAction == CommonConstants.ImportArea)
                        {
                            ExecuteDatabase(ImportDistrict, fileLocation, InputType);
                            ExecuteDatabase(ImportWard, fileLocation, InputType);
                        }
                        else if (ImportAction == CommonConstants.ImportDistrict)
                        {
                            ExecuteDatabase(ImportDistrict, fileLocation, InputType);
                        }
                        else if (ImportAction == CommonConstants.ImportWard)
                        {
                            ExecuteDatabase(ImportWard, fileLocation, InputType);
                        }
                    }
                    catch (Exception e)
                    {
                        // Base Controller đã ghi Log Error rồi
                        return Json(new { status = CommonConstants.Status_Error, message = e.Message + "\n" + e.StackTrace }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { status = CommonConstants.Status_Success, message = "Import Certificate Finished !" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Equipment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Equipment(HttpPostedFileBase file, string ImportAction, string InputType)
        {
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files["file"].FileName);
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx" || fileExtension == ".xlsm")
                {
                    //string fileLocation = Server.MapPath("~/AppFiles/Tmp/") + Request.Files["file"].FileName;
                    string fileLocation = Server.MapPath("~/AppFiles/Tmp/") + "Equipment" + DateTime.Now.ToString("yyyymmssfff") + fileExtension;
                    try
                    {
                        if (System.IO.File.Exists(fileLocation))
                            System.IO.File.Delete(fileLocation);

                        Request.Files["file"].SaveAs(fileLocation);

                        string[] sheetNames = new string[] {
                            CommonConstants.Sheet_Equipment,
                        };

                        string[] columnNames = new string[] {
                            CommonConstants.Sheet_Equipment_Tx,
                            CommonConstants.Sheet_Equipment_MobiFone,
                            CommonConstants.Sheet_Equipment_Viettel,
                            CommonConstants.Sheet_Equipment_VinaPhone,
                            CommonConstants.Sheet_Equipment_VNMobile,
                            CommonConstants.Sheet_Equipment_Gtel
                        };
                        _excelIO.FormatColumns(fileLocation, sheetNames, columnNames, "@");

                        //string extendedProperties = "Excel 12.0;HDR=YES;IMEX=1";
                        //string connectionString1 = string.Format(CultureInfo.CurrentCulture, "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"{1}\"", fileLocation, extendedProperties);
                        if (ImportAction == CommonConstants.SelectAll)
                        {
                            ExecuteDatabase(ImportEquipment, fileLocation, CommonConstants.Sheet_Equipment_MobiFone, InputType);
                            ExecuteDatabase(ImportEquipment, fileLocation, CommonConstants.Sheet_Equipment_Viettel, InputType);
                            ExecuteDatabase(ImportEquipment, fileLocation, CommonConstants.Sheet_Equipment_VinaPhone, InputType);
                            ExecuteDatabase(ImportEquipment, fileLocation, CommonConstants.Sheet_Equipment_VNMobile, InputType);
                            ExecuteDatabase(ImportEquipment, fileLocation, CommonConstants.Sheet_Equipment_Gtel, InputType);
                        }
                        else
                        {
                            ExecuteDatabase(ImportEquipment, fileLocation, ImportAction, InputType);
                        }                        
                    }
                    catch (Exception e)
                    {
                        // Base Controller đã ghi Log Error rồi
                        return Json(new { status = CommonConstants.Status_Error, message = e.Message + "\n" + e.StackTrace }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { status = CommonConstants.Status_Success, message = "Import Certificate Finished !" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSampleFile(string fileName)
        {
            string fileLocation = Server.MapPath("~/AppFiles/Samples/") + fileName;
            int stt = 0;
            DataTable dt;
            if (!string.IsNullOrEmpty(fileName))
            {
                IEnumerable<InCaseOfTabVM> inCaseOfTabVMs = Mapper.Map<IEnumerable<InCaseOfTabVM>>(_importService.GetInCaseOfs());
                stt = 0;
                foreach (var itemVm in inCaseOfTabVMs)
                {
                    stt++;
                    itemVm.No = stt;
                }
                dt = inCaseOfTabVMs.ToDataTable();
                _excelIO.UpdateDataInSheet(fileLocation, CommonConstants.Sheet_InCaseOf, dt);

                IEnumerable<LabTabVM> labTabVMs = Mapper.Map<IEnumerable<LabTabVM>>(_importService.GetLabs());
                stt = 0;
                foreach (var itemVm in labTabVMs)
                {
                    stt++;
                    itemVm.No = stt;
                }
                dt = labTabVMs.ToDataTable();
                _excelIO.UpdateDataInSheet(fileLocation, CommonConstants.Sheet_Lab, dt);


                IEnumerable<CityTabVM> CityTabVMs = Mapper.Map<IEnumerable<CityTabVM>>(_importService.GetCities());
                stt = 0;
                foreach (var itemVm in CityTabVMs)
                {
                    stt++;
                    itemVm.No = stt;
                }
                dt = CityTabVMs.ToDataTable();
                _excelIO.UpdateDataInSheet(fileLocation, CommonConstants.Sheet_City, dt);


                IEnumerable<OperatorTabVM> operatorTabVMs = Mapper.Map<IEnumerable<OperatorTabVM>>(_importService.GetOperators());
                stt = 0;
                foreach (var itemVm in operatorTabVMs)
                {
                    stt++;
                    itemVm.No = stt;
                }
                dt = operatorTabVMs.ToDataTable();
                _excelIO.UpdateDataInSheet(fileLocation, CommonConstants.Sheet_Operator, dt);


                IEnumerable<ApplicantTabVM> applicantTabVMs = Mapper.Map<IEnumerable<ApplicantTabVM>>(_importService.GetApplicants());
                stt = 0;
                foreach (var itemVm in applicantTabVMs)
                {
                    stt++;
                    itemVm.No = stt;
                }
                dt = applicantTabVMs.ToDataTable();
                _excelIO.UpdateDataInSheet(fileLocation, CommonConstants.Sheet_Applicant, dt);

                IEnumerable<AreaTabVM> AreaTabVMs = Mapper.Map<IEnumerable<AreaTabVM>>(_importService.GetAreaTabs());
                stt = 0;
                foreach (var itemVm in AreaTabVMs)
                {
                    stt++;
                    itemVm.No = stt;
                }
                dt = AreaTabVMs.ToDataTable();
                _excelIO.UpdateDataInSheet(fileLocation, CommonConstants.Sheet_Area, dt);

                IEnumerable<EquipmentTabVM> EquipmentTabVMs = Mapper.Map<IEnumerable<EquipmentTabVM>>(_importService.GetEquipmentTabs());
                stt = 0;
                foreach (var itemVm in EquipmentTabVMs)
                {
                    stt++;
                    itemVm.No = stt;
                }
                dt = EquipmentTabVMs.ToDataTable();
                _excelIO.UpdateDataInSheet(fileLocation, CommonConstants.Sheet_Equipment, dt);

                return Json(new { status = CommonConstants.Status_Success, message = "Lấy dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = CommonConstants.Status_Error, message = "Lỗi lấy dữ liệu" }, JsonRequestBehavior.AllowGet);
            }
        }

        private string ImportInCaseOf(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_InCaseOf);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_InCaseOf_ID]?.ToString()))
                {
                    var Item = new InCaseOf();
                    Item.Id = Convert.ToInt32(dt.Rows[i][CommonConstants.Sheet_InCaseOf_ID]);
                    Item.Name = dt.Rows[i][CommonConstants.Sheet_InCaseOf_Name]?.ToString().Trim();
                    Item.CreatedBy = User.Identity.Name;
                    Item.CreatedDate = DateTime.Now;

                    _importService.Add(Item);
                    _importService.Save();
                }
            }
            return CommonConstants.Status_Success;
        }

        private string ImportLab(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Lab);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Lab_ID]?.ToString()))
                {
                    var Item = new Lab();
                    Item.Id = dt.Rows[i][CommonConstants.Sheet_Lab_ID]?.ToString();
                    Item.Name = dt.Rows[i][CommonConstants.Sheet_Lab_Name]?.ToString().Trim();
                    Item.Address = dt.Rows[i][CommonConstants.Sheet_Lab_Address]?.ToString();
                    Item.Phone = dt.Rows[i][CommonConstants.Sheet_Lab_Phone]?.ToString();
                    Item.Fax = dt.Rows[i][CommonConstants.Sheet_Lab_Fax]?.ToString();
                    Item.CreatedBy = User.Identity.Name;
                    Item.CreatedDate = DateTime.Now;

                    _importService.Add(Item);
                    _importService.Save();
                }
            }

            return CommonConstants.Status_Success;
        }

        private string ImportCity(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_City);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_City_ID]?.ToString()))
                {
                    var Item = new City();
                    Item.Id = dt.Rows[i][CommonConstants.Sheet_City_ID]?.ToString();
                    Item.Name = dt.Rows[i][CommonConstants.Sheet_City_Name]?.ToString().Trim();
                    if (dt.Columns.Contains(CommonConstants.Sheet_City_Area))
                        Item.Area = dt.Rows[i][CommonConstants.Sheet_City_Area]?.ToString().Trim();
                    Item.CreatedBy = User.Identity.Name;
                    Item.CreatedDate = DateTime.Now;

                    _importService.Add(Item);
                    _importService.Save();
                }
            }

            return CommonConstants.Status_Success;
        }

        private string ImportDistrict(string excelConnectionString, string InputType)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Area);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Area_DistrictId]?.ToString()))
                {
                    var Item = new District();
                    Item.Id = dt.Rows[i][CommonConstants.Sheet_Area_DistrictId]?.ToString();
                    Item.Name = dt.Rows[i][CommonConstants.Sheet_Area_DistrictName]?.ToString().Trim();
                    Item.CityId = dt.Rows[i][CommonConstants.Sheet_Area_CityID]?.ToString().Trim();
                    Item.CreatedBy = User.Identity.Name;
                    Item.CreatedDate = DateTime.Now;

                    if (InputType == CommonConstants.InputType_AddNew || InputType == CommonConstants.InputType_AddMore)
                    {
                        _importService.Add(Item);
                    }
                    else if (InputType == CommonConstants.InputType_UpdateAdd)
                    {
                        District existedItem = _importService.getDistrict(Item.Id);
                        if (existedItem == null)
                        {
                            _importService.Add(Item);
                        }
                        else
                        {
                            _importService.Update(Item);
                        }
                    }
                    _importService.Save();
                }
            }

            return CommonConstants.Status_Success;
        }

        private string ImportWard(string excelConnectionString, string InputType)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Area);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Area_WardId]?.ToString()))
                {
                    var Item = new Ward();
                    Item.Id = dt.Rows[i][CommonConstants.Sheet_Area_WardId]?.ToString();
                    Item.Name = dt.Rows[i][CommonConstants.Sheet_Area_WardName]?.ToString().Trim();
                    Item.DistrictId = dt.Rows[i][CommonConstants.Sheet_Area_DistrictId]?.ToString().Trim();
                    Item.CreatedBy = User.Identity.Name;
                    Item.CreatedDate = DateTime.Now;

                    if (InputType == CommonConstants.InputType_AddNew || InputType == CommonConstants.InputType_AddMore)
                    {
                        _importService.Add(Item);
                    }
                    else if (InputType == CommonConstants.InputType_UpdateAdd)
                    {
                        District existedItem = _importService.getDistrict(Item.Id);
                        if (existedItem == null)
                        {
                            _importService.Add(Item);
                        }
                        else
                        {
                            _importService.Update(Item);
                        }
                    }
                    _importService.Save();
                }
            }

            return CommonConstants.Status_Success;
        }

        private string ImportEquipment(string excelConnectionString, string OperatorRootID, string InputType)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Equipment);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Equipment_Name]?.ToString()))
                {
                    var Item = new Equipment();
                    Item.Name = dt.Rows[i][CommonConstants.Sheet_Equipment_Name]?.ToString().Trim();
                    Item.Band = dt.Rows[i][CommonConstants.Sheet_Equipment_Band]?.ToString().Trim();
                    Item.Tx = Int32.Parse(dt.Rows[i][CommonConstants.Sheet_Equipment_Tx]?.ToString());
                    Item.OperatorRootID = OperatorRootID;
                    if (dt.Rows[i][OperatorRootID]?.ToString().Length > 0)
                        Item.MaxPower = Double.Parse(dt.Rows[i][OperatorRootID]?.ToString());

                    Item.CreatedBy = User.Identity.Name;
                    Item.CreatedDate = DateTime.Now;

                    if (InputType == CommonConstants.InputType_AddNew || InputType == CommonConstants.InputType_AddMore)
                    {
                        if (Item.MaxPower != 0)
                        {
                            _importService.Add(Item);
                        }                        
                    }
                    else if (InputType == CommonConstants.InputType_UpdateAdd)
                    {
                        Equipment existedItem = _importService.getEquipment(Item.Name, Item.Band, Item.OperatorRootID);
                        if (existedItem == null)
                        {
                            _importService.Add(Item);
                        }
                        else
                        {
                            Item.Id = existedItem.Id;
                            _importService.Update(Item);
                        }
                    }
                    _importService.Save();
                }
            }

            return CommonConstants.Status_Success;
        }
        private string ImportOperator(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Operator);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Operator_ID]?.ToString()))
                {
                    var Item = new Operator();
                    Item.RootId = dt.Rows[i][CommonConstants.Sheet_Operator_RootID]?.ToString();
                    Item.Id = dt.Rows[i][CommonConstants.Sheet_Operator_ID]?.ToString();
                    Item.Name = dt.Rows[i][CommonConstants.Sheet_Operator_Name]?.ToString().Trim();
                    Item.CreatedBy = User.Identity.Name;
                    Item.CreatedDate = DateTime.Now;

                    _importService.Add(Item);
                    _importService.Save();
                }
            }

            return CommonConstants.Status_Success;
        }

        private string ImportApplicant_Old(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Applicant);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Applicant_ID]?.ToString()))
                {
                    var Item = new Applicant();
                    Item.Id = dt.Rows[i][CommonConstants.Sheet_Applicant_ID]?.ToString();
                    Item.Name = dt.Rows[i][CommonConstants.Sheet_Applicant_Name]?.ToString().Trim();
                    Item.Address = dt.Rows[i][CommonConstants.Sheet_Applicant_Address]?.ToString();
                    Item.Phone = dt.Rows[i][CommonConstants.Sheet_Applicant_Phone]?.ToString();
                    Item.Fax = dt.Rows[i][CommonConstants.Sheet_Applicant_Fax]?.ToString();
                    Item.ContactName = dt.Rows[i][CommonConstants.Sheet_Applicant_ContactName]?.ToString();
                    Item.OperatorID = dt.Rows[i][CommonConstants.Sheet_Applicant_OperatorID]?.ToString();
                    Item.CreatedBy = User.Identity.Name;
                    Item.CreatedDate = DateTime.Now;

                    _importService.Add(Item);
                    _importService.Save();
                }
            }
            return CommonConstants.Status_Success;
        }

        private string ImportApplicant(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Applicant);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Applicant_ID]?.ToString()))
                {
                    var Item = new Applicant();
                    Item.Id = dt.Rows[i][CommonConstants.Sheet_Applicant_ID]?.ToString();
                    Item.Name = dt.Rows[i][CommonConstants.Sheet_Applicant_Name]?.ToString().Trim();
                    Item.Address = dt.Rows[i][CommonConstants.Sheet_Applicant_Address]?.ToString();
                    Item.Phone = dt.Rows[i][CommonConstants.Sheet_Applicant_Phone]?.ToString();
                    Item.Fax = dt.Rows[i][CommonConstants.Sheet_Applicant_Fax]?.ToString();
                    Item.ContactName = dt.Rows[i][CommonConstants.Sheet_Applicant_ContactName]?.ToString();

                    if (dt.Columns.Contains(CommonConstants.Sheet_Applicant_OperatorID))
                    {
                        Item.OperatorID = dt.Rows[i][CommonConstants.Sheet_Applicant_OperatorID]?.ToString();
                    }

                    if (String.IsNullOrEmpty(Item.OperatorID) && (dt.Columns.Contains(CommonConstants.Sheet_Applicant_OperatorName)))
                    {
                        Item.OperatorID = _importService.getOperatorByName(dt.Rows[i][CommonConstants.Sheet_Applicant_OperatorName]?.ToString()).Id;
                    }

                    if (String.IsNullOrEmpty(Item.OperatorID))
                    {
                        return CommonConstants.Status_Error + ": Doanh nghiệp cung cấp dịch vụ chưa được khai báo hoặc đã được khai báo với Mã khác";
                    }

                    Item.CreatedBy = User.Identity.Name;
                    Item.CreatedDate = DateTime.Now;

                    _importService.Add(Item);
                    _importService.Save();
                }
            }
            return CommonConstants.Status_Success;
        }

        private string ImportProfile(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Profile);
            var Item = new Profile();
            profileID = "";
            if (!string.IsNullOrEmpty(dt.Rows[1][CommonConstants.Sheet_Profile_ApplicantID]?.ToString()))
            {
                if (dt.Columns.Contains(CommonConstants.Sheet_Profile_ApplicantID))
                {
                    Item.ApplicantID = dt.Rows[1][CommonConstants.Sheet_Profile_ApplicantID]?.ToString();
                }

                if (String.IsNullOrEmpty(Item.ApplicantID) && (dt.Columns.Contains(CommonConstants.Sheet_Profile_ApplicantName)))
                {
                    Item.ApplicantID = _importService.getOperatorByName(dt.Rows[1][CommonConstants.Sheet_Profile_ApplicantName]?.ToString()).Id;
                }

                if (String.IsNullOrEmpty(Item.ApplicantID) || _importService.findApplicant(Item.ApplicantID) == null)
                {
                    return CommonConstants.Status_Error + ": Đơn vị nộp hồ sơ chưa được khai báo hoặc đã được khai báo với Mã khác";
                }


                Item.ProfileNum = dt.Rows[1][CommonConstants.Sheet_Profile_ProfileNum]?.ToString();
                Item.ProfileDate = DateTime.Parse(dt.Rows[1][CommonConstants.Sheet_Profile_ProfileDate]?.ToString());
                Item.BtsQuantity = Int32.Parse(dt.Rows[1][CommonConstants.Sheet_Profile_BtsQuantity]?.ToString());
                Item.AcceptedBtsQuantity = Int32.Parse(dt.Rows[1][CommonConstants.Sheet_Profile_AcceptedBtsQuantity]?.ToString());
                Item.ApplyDate = DateTime.Parse(dt.Rows[1][CommonConstants.Sheet_Profile_ApplyDate]?.ToString());
                if (dt.Rows[1][CommonConstants.Sheet_Profile_Fee]?.ToString().Length > 0)
                {
                    //int fee;
                    //int.TryParse(dt.Rows[1][CommonConstants.Sheet_Profile_Fee]?.ToString(), NumberStyles.Number, provider, out fee);
                    Item.Fee = Int64.Parse(dt.Rows[1][CommonConstants.Sheet_Profile_Fee]?.ToString(), NumberStyles.Number);
                }

                if (dt.Rows[1][CommonConstants.Sheet_Profile_FeeAnnounceNum]?.ToString().Length > 0)
                    Item.FeeAnnounceNum = dt.Rows[1][CommonConstants.Sheet_Profile_FeeAnnounceNum]?.ToString();
                if (dt.Rows[1][CommonConstants.Sheet_Profile_FeeAnnounceDate]?.ToString().Length > 0)
                    Item.FeeAnnounceDate = DateTime.Parse(dt.Rows[1][CommonConstants.Sheet_Profile_FeeAnnounceDate]?.ToString());
                if (dt.Rows[1][CommonConstants.Sheet_Profile_FeeReceiptDate]?.ToString().Length > 0)
                    Item.FeeReceiptDate = DateTime.Parse(dt.Rows[1][CommonConstants.Sheet_Profile_FeeReceiptDate]?.ToString());
                Item.CreatedBy = User.Identity.Name;
                Item.CreatedDate = DateTime.Now;

                Profile dbProfile = _importService.findProfile(Item.ApplicantID, Item.ProfileNum, Item.ProfileDate);
                // Neu Profile da ton tai thi cap nhat Profile
                if (dbProfile != null)
                {
                    profileID = dbProfile.Id;
                    Profile dbPro = _importService.getProfile(profileID);
                    dbPro.BtsQuantity = Item.BtsQuantity;
                    dbPro.AcceptedBtsQuantity = Item.AcceptedBtsQuantity;
                    dbPro.ApplyDate = Item.ApplyDate;
                    dbPro.Fee = Item.Fee;
                    dbPro.FeeAnnounceNum = Item.FeeAnnounceNum;
                    dbPro.FeeAnnounceDate = Item.FeeAnnounceDate;
                    dbPro.FeeReceiptDate = Item.FeeReceiptDate;
                    dbPro.UpdatedBy = User.Identity.Name;
                    dbPro.UpdatedDate = DateTime.Now;
                    _importService.Update(dbPro);
                    _importService.Save();
                }
                else
                {
                    _importService.Add(Item);
                    _importService.Save();
                    profileID = Item.Id;
                }
            }
            return CommonConstants.Status_Success;
        }

        private string ImportBts(string excelConnectionString, string InputType, string profileID)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Bts);
            string operatorID = _importService.getApplicant(profileID).OperatorID;

            if (InputType == CommonConstants.InputType_AddMore)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode]?.ToString()))
                    {
                        var Item = new Bts();
                        Item.OperatorID = operatorID;
                        Item.ProfileID = profileID;
                        Item.BtsCode = dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode]?.ToString();
                        Item.Address = dt.Rows[i][CommonConstants.Sheet_Bts_Address]?.ToString();
                        Item.CityID = dt.Rows[i][CommonConstants.Sheet_Bts_CityID]?.ToString();
                        if (dt.Rows[i][CommonConstants.Sheet_Bts_Longtitude]?.ToString().Length > 0)
                            Item.Longtitude = double.Parse(dt.Rows[i][CommonConstants.Sheet_Bts_Longtitude]?.ToString());
                        if (dt.Rows[i][CommonConstants.Sheet_Bts_Latitude]?.ToString().Length > 0)
                            Item.Latitude = double.Parse(dt.Rows[i][CommonConstants.Sheet_Bts_Latitude]?.ToString());
                        if (dt.Rows[i][CommonConstants.Sheet_Bts_InCaseOfID]?.ToString().Length > 0)
                            Item.InCaseOfID = Int32.Parse(dt.Rows[i][CommonConstants.Sheet_Bts_InCaseOfID]?.ToString());

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
                        Item.CreatedBy = User.Identity.Name;
                        Item.CreatedDate = DateTime.Now;

                        Bts existBts = _importService.findBts(Item.ProfileID, Item.BtsCode);
                        if (existBts != null)
                        {
                            _importService.Delete(existBts);
                            _importService.Save();
                        }

                        _importService.Add(Item);

                        _importService.Save();
                    }
                }
            }
            if (InputType == CommonConstants.InputType_AddNew)
            {
                if (InputType == CommonConstants.InputType_AddNew && _importService.findBtssByProfile(profileID).Count() > 0)
                {
                    Profile dbPro = _importService.getProfile(profileID);
                    return "Các trạm gốc BTS của hồ sơ kiểm định số " + dbPro.ProfileNum + " đã tồn tại rồi";
                }

                // Them cac tram BTS cua Profile vao

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode]?.ToString()))
                    {
                        var Item = new Bts();
                        Item.OperatorID = operatorID;
                        Item.ProfileID = profileID;
                        Item.BtsCode = dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode]?.ToString();
                        Item.Address = dt.Rows[i][CommonConstants.Sheet_Bts_Address]?.ToString();
                        Item.CityID = dt.Rows[i][CommonConstants.Sheet_Bts_CityID]?.ToString();
                        if (dt.Rows[i][CommonConstants.Sheet_Bts_Longtitude]?.ToString().Length > 0)
                            Item.Longtitude = double.Parse(dt.Rows[i][CommonConstants.Sheet_Bts_Longtitude]?.ToString());
                        if (dt.Rows[i][CommonConstants.Sheet_Bts_Latitude]?.ToString().Length > 0)
                            Item.Latitude = double.Parse(dt.Rows[i][CommonConstants.Sheet_Bts_Latitude]?.ToString());
                        if (dt.Rows[i][CommonConstants.Sheet_Bts_InCaseOfID]?.ToString().Length > 0)
                            Item.InCaseOfID = Int32.Parse(dt.Rows[i][CommonConstants.Sheet_Bts_InCaseOfID]?.ToString());

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
                        Item.CreatedBy = User.Identity.Name;
                        Item.CreatedDate = DateTime.Now;

                        _importService.Add(Item);

                        _importService.Save();
                    }
                }

            }

            return CommonConstants.Status_Success;
        }

        private string ImportCertificate(string excelConnectionString, string InputType, string profileID, bool IsSigned)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_Certificate);
            string operatorID = _importService.getApplicant(profileID).OperatorID;

            if (dt.Rows.Count > 0)
            {
                if (Int32.Parse(dt.Rows[0][CommonConstants.Sheet_Certificate_CerBtsNum]?.ToString()) <= 0)
                {
                    return CommonConstants.Status_Success;
                }
            }

            if (InputType == CommonConstants.InputType_AddNew)
            {
                if (_importService.findCertsByProfile(profileID).Count() > 0)
                {
                    Profile dbPro = _importService.getProfile(profileID);
                    return "Hồ sơ kiểm định số " + dbPro.ProfileNum + " đã có 1 số trạm BTS được cấp GCNKĐ rồi";
                }

                // Them cac Giấy CNKĐ cua Profile vao

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Certificate Item = FillInCertificate(profileID, operatorID, dt, i);
                    Item.IsSigned = IsSigned;

                    if (!string.IsNullOrEmpty(Item.BtsCode?.ToString()))
                    {
                        Certificate existedItemID = _importService.getCertificate(Item.Id);
                        if (existedItemID != null)
                        {
                            if (existedItemID.BtsCode != Item.BtsCode || existedItemID.ProfileID != Item.ProfileID)
                                return "Lỗi: Giấy Chứng nhận kiểm định số " + Item.Id + " đã được cấp ở Hồ sơ số " + existedItemID.ProfileID + " rồi";
                            else
                            {
                                // Số Giấy chứng nhận này đã được thêm vào lần cấp trước của hồ sơ rồi.
                            }
                        }
                        else
                        {
                            // Thêm các Giấy chứng nhận mới khác của hồ sơ ở lần thứ N.
                            Certificate existCertificate = _importService.findCertificate(Item.BtsCode, Item.ProfileID);
                            if (existCertificate == null)
                            {
                                _importService.Add(Item);
                                _importService.Save();

                                Bts dbBts = _importService.findBts(profileID, Item.BtsCode);
                                if (dbBts != null)
                                {
                                    _importService.Delete(dbBts);
                                    _importService.Save();
                                }
                                RemoveSubBtsInCert(Item);
                                AddSubBtsInCert(Item);
                            }
                            else
                            {
                                if (existCertificate.Id != Item.Id)
                                    return "Lỗi: Trạm gốc " + Item.BtsCode + " trong hồ sơ đã được cấp Giấy CNKĐ rồi (Số: " + existCertificate.Id + ")";
                                else
                                {
                                    // Số Giấy chứng nhận này đã được thêm vào bị trùng lập trong hồ sơ.
                                    return "Lỗi: Trạm gốc " + Item.BtsCode + " trong hồ sơ đã được cấp trùng (Số Giấy CNKĐ: " + existCertificate.Id + ")";
                                }
                            }
                        }

                        //Certificate existedItemID = _importService.getCertificate(Item.Id);

                        //if (existedItemID != null)
                        //{
                        //    if (existedItemID.BtsCode != Item.BtsCode || existedItemID.ProfileID != Item.ProfileID)
                        //        return "Giấy Chứng nhận kiểm định số " + Item.Id + " đã tôn tại rồi";
                        //    else
                        //    {

                        //    }
                        //}

                        //Certificate existCertificate = _importService.findCertificate(Item.BtsCode, Item.ProfileID);
                        //if (existCertificate != null)
                        //{
                        //    return "Trạm gốc " + Item.BtsCode + " trong hồ sơ đã được cấp Giấy CNKĐ rồi (số: " + Item.Id + ")";
                        //}

                        //_importService.Add(Item);
                        //_importService.Save();

                        //Bts dbBts = _importService.findBts(profileID, Item.BtsCode);
                        //if (dbBts != null)
                        //{
                        //    _importService.Delete(dbBts);
                        //    _importService.Save();
                        //}

                        //for (int j = 0; j < Item.SubBtsQuantity; j++)
                        //{
                        //    SubBtsInCert subBtsItem = new SubBtsInCert();
                        //    subBtsItem.CertificateID = Item.Id;
                        //    subBtsItem.BtsSerialNo = j + 1;
                        //    subBtsItem.BtsCode = SubBtsCodes[j];
                        //    subBtsItem.OperatorID = SubBtsOperatorIDs[j];
                        //    subBtsItem.AntenHeight = SubBtsAntenHeights[j];
                        //    subBtsItem.AntenNum = Int32.Parse(SubBtsAntenNums[j]);
                        //    subBtsItem.Band = SubBtsBands[j];
                        //    subBtsItem.Configuration = SubBtsConfigurations[j];
                        //    subBtsItem.Equipment = SubBtsEquipments[j];
                        //    if (subBtsItem.Equipment.IndexOf(' ') >= 0)
                        //    {
                        //        subBtsItem.Manufactory = subBtsItem.Equipment.Substring(0, subBtsItem.Equipment.IndexOf(' '));
                        //    }
                        //    else
                        //    {
                        //        subBtsItem.Manufactory = SubBtsEquipments[j];
                        //    }
                        //    subBtsItem.PowerSum = SubBtsPowerSums[j];

                        //    subBtsItem.CreatedBy = User.Identity.Name;
                        //    subBtsItem.CreatedDate = DateTime.Now;


                        //    _importService.Add(subBtsItem);
                        //    _importService.Save();
                        //}

                    }
                }

            }

            if (InputType == CommonConstants.InputType_AddMore)
            {
                // Them cac Giấy CNKĐ cua Profile vao

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Certificate Item = FillInCertificate(profileID, operatorID, dt, i);
                    Item.IsSigned = IsSigned;

                    if (!string.IsNullOrEmpty(Item.BtsCode?.ToString()))
                    {
                        Certificate existedItemID = _importService.getCertificate(Item.Id);
                        if (existedItemID != null)
                        {
                            if (existedItemID.BtsCode != Item.BtsCode || existedItemID.ProfileID != Item.ProfileID)
                                return "Lỗi: Giấy Chứng nhận kiểm định số " + Item.Id + " đã được cấp ở Hồ sơ số " + existedItemID.ProfileID + " rồi";
                            else
                            {
                                // Số Giấy chứng nhận này đã được thêm vào lần cấp trước của hồ sơ rồi => Không thêm nữa.
                            }
                        }
                        else
                        {
                            // Thêm các Giấy chứng nhận mới khác của hồ sơ ở lần thứ N.
                            Certificate existCertificate = _importService.findCertificate(Item.BtsCode, Item.ProfileID);
                            if (existCertificate == null)
                            {
                                _importService.Add(Item);
                                _importService.Save();

                                Bts dbBts = _importService.findBts(profileID, Item.BtsCode);
                                if (dbBts != null)
                                {
                                    _importService.Delete(dbBts);
                                    _importService.Save();
                                }
                                RemoveSubBtsInCert(Item);
                                AddSubBtsInCert(Item);
                            }
                            else
                            {
                                if (existCertificate.Id != Item.Id)
                                    return "Lỗi: Trạm gốc " + Item.BtsCode + " trong hồ sơ đã được cấp Giấy CNKĐ rồi (Số: " + existCertificate.Id + ")";
                                else
                                {
                                    // Số Giấy chứng nhận này đã được thêm vào lần cấp trước của hồ sơ rồi=> Không thêm nữa..
                                }
                            }
                        }
                    }
                }
            }

            if (InputType == CommonConstants.InputType_UpdateAdd)
            {
                // Them cac Giấy CNKĐ cua Profile vao

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Certificate Item = FillInCertificate(profileID, operatorID, dt, i);
                    Item.IsSigned = IsSigned;

                    if (!string.IsNullOrEmpty(Item.BtsCode?.ToString()))
                    {
                        Certificate existedItemID = _importService.getCertificate(Item.Id);
                        if (existedItemID != null)
                        {
                            if (existedItemID.BtsCode != Item.BtsCode || existedItemID.ProfileID != Item.ProfileID)
                                return "Lỗi: Giấy Chứng nhận kiểm định số " + Item.Id + " đã được cấp ở Hồ sơ số " + existedItemID.ProfileID + " rồi";
                            else
                            {
                                // Số Giấy chứng nhận này đã được thêm vào lần cấp trước của hồ sơ rồi => Cập nhật lại
                                _importService.Update(Item);
                                _importService.Save();

                                RemoveSubBtsInCert(Item);
                                AddSubBtsInCert(Item);
                            }
                        }
                        else
                        {
                            // Thêm các Giấy chứng nhận mới khác của hồ sơ ở lần thứ N.
                            Certificate existCertificate = _importService.findCertificate(Item.BtsCode, Item.ProfileID);
                            if (existCertificate == null)
                            {
                                _importService.Add(Item);
                                _importService.Save();

                                Bts dbBts = _importService.findBts(profileID, Item.BtsCode);
                                if (dbBts != null)
                                {
                                    _importService.Delete(dbBts);
                                    _importService.Save();
                                }
                                RemoveSubBtsInCert(Item);
                                AddSubBtsInCert(Item);
                            }
                            else
                            {
                                if (existCertificate.Id != Item.Id)
                                    return "Lỗi: Trạm gốc " + Item.BtsCode + " trong hồ sơ đã được cấp Giấy CNKĐ rồi (Số: " + existCertificate.Id + ")";
                                else
                                {
                                    // Số Giấy chứng nhận này đã được thêm vào lần cấp trước của hồ sơ rồi => Cập nhật lại
                                    _importService.Update(Item);
                                    _importService.Save();

                                    RemoveSubBtsInCert(Item);
                                    AddSubBtsInCert(Item);
                                }
                            }
                        }
                    }
                }
            }
            return CommonConstants.Status_Success;
        }

        private bool RemoveSubBtsInCert(Certificate Item)
        {
            _importService.RemoveSubBtsInCert(Item.Id);
            return true;
        }

        private bool AddSubBtsInCert (Certificate Item)
        {
            int StartIndex, StopIndex;
            string Technology;
            string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsTechnologies, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;
            SubBtsAntenHeights = Item.SubBtsAntenHeights.Split(new char[] { ';' });
            SubBtsAntenNums = Item.SubBtsAntenNums.Split(new char[] { ';' });
            SubBtsBands = Item.SubBtsBands.Split(new char[] { ';' });
            //SubBtsTechnologies = Item.SubBtsTechnologies.Split(new char[] { ';' });
            SubBtsCodes = Item.SubBtsCodes.Split(new char[] { ';' });
            SubBtsConfigurations = Item.SubBtsConfigurations.Split(new char[] { ';' });
            SubBtsEquipments = Item.SubBtsEquipments.Split(new char[] { ';' });
            SubBtsOperatorIDs = Item.SubBtsOperatorIDs.Split(new char[] { ';' });
            SubBtsPowerSums = Item.SubBtsPowerSums.Split(new char[] { ';' });

            for (int j = 0; j < Item.SubBtsQuantity; j++)
            {
                StartIndex = SubBtsBands[j].IndexOf("(");
                StopIndex = SubBtsBands[j].IndexOf(")");
                if (StartIndex > -1 && StopIndex > -1 && StopIndex > StartIndex)
                {
                    Technology = SubBtsBands[j].Substring(StartIndex + 1, StopIndex - StartIndex - 1);
                }
                else
                {
                    Technology = "";
                }
                SubBtsInCert subBtsItem = new SubBtsInCert();
                subBtsItem.CertificateID = Item.Id;
                subBtsItem.BtsSerialNo = j + 1;
                subBtsItem.BtsCode = SubBtsCodes[j];
                subBtsItem.OperatorID = SubBtsOperatorIDs[j];
                subBtsItem.AntenHeight = SubBtsAntenHeights[j];
                subBtsItem.AntenNum = Int32.Parse(SubBtsAntenNums[j]);
                subBtsItem.Band = SubBtsBands[j];
                subBtsItem.Technology = Technology;
                subBtsItem.Configuration = SubBtsConfigurations[j];
                subBtsItem.Equipment = SubBtsEquipments[j];
                if (subBtsItem.Equipment.IndexOf(' ') >= 0)
                {
                    subBtsItem.Manufactory = subBtsItem.Equipment.Substring(0, subBtsItem.Equipment.IndexOf(' '));
                }
                else
                {
                    subBtsItem.Manufactory = SubBtsEquipments[j];
                }
                subBtsItem.PowerSum = SubBtsPowerSums[j];

                subBtsItem.CreatedBy = User.Identity.Name;
                subBtsItem.CreatedDate = DateTime.Now;


                _importService.Add(subBtsItem);
                _importService.Save();
            }
            return true;
        }

        private Certificate FillInCertificate(string profileID, string operatorID, DataTable dt, int i)
        {
            DateTime datetimeData;
            var Item = new Certificate();

            Item.ProfileID = profileID;
            Item.OperatorID = operatorID;
            Item.Id = dt.Rows[i][CommonConstants.Sheet_Certificate_CertificateNum]?.ToString();
            Item.BtsCode = dt.Rows[i][CommonConstants.Sheet_Certificate_BtsCode]?.ToString();
            Item.Address = dt.Rows[i][CommonConstants.Sheet_Certificate_Address]?.ToString();
            Item.CityID = dt.Rows[i][CommonConstants.Sheet_Certificate_CityID]?.ToString();
            if (dt.Rows[i][CommonConstants.Sheet_Certificate_Longtitude]?.ToString().Length > 0)
                Item.Longtitude = double.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_Longtitude]?.ToString());
            if (dt.Rows[i][CommonConstants.Sheet_Certificate_Latitude]?.ToString().Length > 0)
                Item.Latitude = double.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_Latitude]?.ToString());
            if (dt.Rows[i][CommonConstants.Sheet_Certificate_InCaseOfID]?.ToString().Length > 0)
                Item.InCaseOfID = Int32.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_InCaseOfID]?.ToString());

            Item.LabID = dt.Rows[i][CommonConstants.Sheet_Certificate_LabID]?.ToString();

            Item.TestReportNo = dt.Rows[i][CommonConstants.Sheet_Certificate_TestReportNo]?.ToString();

            if (DateTime.TryParse(dt.Rows[i][CommonConstants.Sheet_Certificate_TestReportDate]?.ToString(), out datetimeData))
            {
                Item.TestReportDate = datetimeData;
            }

            if (DateTime.TryParse(dt.Rows[i][CommonConstants.Sheet_Certificate_IssuedDate]?.ToString(), out datetimeData))
            {
                Item.IssuedDate = datetimeData;
            }
                
            if (DateTime.TryParse(dt.Rows[i][CommonConstants.Sheet_Certificate_ExpiredDate]?.ToString(), out datetimeData))
            {
                Item.ExpiredDate = datetimeData;
            }

            Item.SubBtsQuantity = Int32.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsQuantity]?.ToString());

            if (dt.Rows[i][CommonConstants.Sheet_Certificate_IsPoleOnGround]?.ToString().Length > 0)
                Item.IsPoleOnGround = bool.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_IsPoleOnGround]?.ToString());

            if (dt.Rows[i][CommonConstants.Sheet_Certificate_IsSafeLimit]?.ToString().Length > 0)
                Item.IsSafeLimit = bool.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_IsSafeLimit]?.ToString());

            if (dt.Rows[i][CommonConstants.Sheet_Certificate_SafeLimitHeight]?.ToString().Length > 0)
                Item.SafeLimitHeight = Double.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_SafeLimitHeight]?.ToString());

            Item.IsHouseIn100m = bool.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_IsHouseIn100m]?.ToString());

            if (dt.Rows[i][CommonConstants.Sheet_Certificate_MaxHeightIn100m]?.ToString().Length > 0)
                Item.MaxHeightIn100m = Double.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_MaxHeightIn100m]?.ToString());

            if (dt.Rows[i][CommonConstants.Sheet_Certificate_MaxPowerSum]?.ToString().Length > 0)
                Item.MaxPowerSum = Double.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_MaxPowerSum]?.ToString());

            Item.IsMeasuringExposure = bool.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_IsMeasuringExposure]?.ToString());

            if (dt.Rows[i][CommonConstants.Sheet_Certificate_MinAntenHeight]?.ToString().Length > 0)
                Item.MinAntenHeight = Double.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_MinAntenHeight]?.ToString());

            Item.OffsetHeight = Item.MinAntenHeight - Item.MaxHeightIn100m;

            //if (dt.Rows[i][CommonConstants.Sheet_Certificate_OffsetHeight]?.ToString().Length > 0)
            //    Item.OffsetHeight = Double.Parse(dt.Rows[i][CommonConstants.Sheet_Certificate_OffsetHeight]?.ToString());

            Item.SubBtsCodes = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsCodes]?.ToString();

            Item.SubBtsAntenHeights = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsAntenHeights]?.ToString();

            Item.SubBtsAntenNums = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsAntenNums]?.ToString();

            //Item.SharedAntens = dt.Rows[i][CommonConstants.Sheet_Certificate_SharedAntens]?.ToString();

            Item.SubBtsBands = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsBands]?.ToString();

            Item.SubBtsBands = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsBands]?.ToString();

            //Item.SubBtsTechnologies = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsTechnologies]?.ToString();

            Item.SubBtsConfigurations = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsConfigurations]?.ToString();

            Item.SubBtsEquipments = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsEquipments]?.ToString();

            Item.SubBtsOperatorIDs = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsOperatorIDs]?.ToString();

            Item.SubBtsPowerSums = dt.Rows[i][CommonConstants.Sheet_Certificate_SubBtsPowerSums]?.ToString();

            //Item.VerifyUnit = CommonConstants.VerifyUnit;
            //Item.IssuedPlace = CommonConstants.IssuePalce;
            //Item.Signer = CommonConstants.Signer;
            //Item.SignerRole = CommonConstants.SignerRole;
            //Item.SignerSubRole = CommonConstants.SignerSubRole;


            // Bo sung them cho data 2020

            Item.VerifyUnit = dt.Rows[i][CommonConstants.Sheet_Certificate_VerifyUnit]?.ToString();
            Item.IssuedPlace = dt.Rows[i][CommonConstants.Sheet_Certificate_IssuedPlace]?.ToString();
            Item.Signer = dt.Rows[i][CommonConstants.Sheet_Certificate_Signer]?.ToString();
            Item.SignerRole = dt.Rows[i][CommonConstants.Sheet_Certificate_SignerRole]?.ToString();
            Item.SignerSubRole = dt.Rows[i][CommonConstants.Sheet_Certificate_SignerSubRole]?.ToString();
            Item.Verifier1 = dt.Rows[i][CommonConstants.Sheet_Certificate_Verifier1]?.ToString();
            Item.Verifier2 = dt.Rows[i][CommonConstants.Sheet_Certificate_Verifier2]?.ToString();

            Item.CreatedBy = User.Identity.Name;
            Item.CreatedDate = DateTime.Now;
            return Item;
        }

        private string ImportNoCertificate(string excelConnectionString, string InputType, string proFileID, bool IsSigned)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_NoCertificate);
            string operatorID = _importService.getApplicant(proFileID).OperatorID;

            if (dt.Rows.Count > 0)
            {
                if (Int32.Parse(dt.Rows[0][CommonConstants.Sheet_NoCertificate_NoCerBtsNum]?.ToString()) <= 0)
                {
                    return CommonConstants.Status_Success;
                }
            }


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_Bts_BtsCode]?.ToString()))
                {
                    var Item = new NoCertificate();

                    Item.IsSigned = IsSigned;

                    Item.ProfileID = proFileID;
                    Item.OperatorID = operatorID;
                    Item.BtsCode = dt.Rows[i][CommonConstants.Sheet_NoCertificate_BtsCode]?.ToString();
                    Item.Address = dt.Rows[i][CommonConstants.Sheet_NoCertificate_Address]?.ToString();
                    Item.CityID = dt.Rows[i][CommonConstants.Sheet_NoCertificate_CityID]?.ToString();
                    if (dt.Rows[i][CommonConstants.Sheet_NoCertificate_Longtitude]?.ToString().Length > 0)
                        Item.Longtitude = double.Parse(dt.Rows[i][CommonConstants.Sheet_NoCertificate_Longtitude]?.ToString());
                    if (dt.Rows[i][CommonConstants.Sheet_NoCertificate_Latitude]?.ToString().Length > 0)
                        Item.Latitude = double.Parse(dt.Rows[i][CommonConstants.Sheet_NoCertificate_Latitude]?.ToString());
                    if (dt.Rows[i][CommonConstants.Sheet_NoCertificate_InCaseOfID]?.ToString().Length > 0)
                        Item.InCaseOfID = Int32.Parse(dt.Rows[i][CommonConstants.Sheet_NoCertificate_InCaseOfID]?.ToString());

                    Item.LabID = dt.Rows[i][CommonConstants.Sheet_NoCertificate_LabID]?.ToString();

                    Item.TestReportNo = dt.Rows[i][CommonConstants.Sheet_NoCertificate_TestReportNo]?.ToString();
                    Item.TestReportDate = DateTime.Parse(dt.Rows[i][CommonConstants.Sheet_NoCertificate_TestReportDate]?.ToString());
                    Item.ReasonNoCertificate = dt.Rows[i][CommonConstants.Sheet_NoCertificate_ReasonNoCertificate]?.ToString();

                    Item.CreatedBy = User.Identity.Name;
                    Item.CreatedDate = DateTime.Now;

                    NoCertificate dbNoCertificate = _importService.findNoCertificate(Item.BtsCode, Item.ProfileID);
                    if (dbNoCertificate != null)
                    {
                        NoCertificate dbUpdate = _importService.getNoCertificate(dbNoCertificate.Id);

                        dbNoCertificate.IsSigned = Item.IsSigned;

                        dbNoCertificate.OperatorID = Item.OperatorID;
                        dbNoCertificate.Address = Item.Address;
                        dbNoCertificate.CityID = Item.CityID;
                        dbNoCertificate.Longtitude = Item.Longtitude;
                        dbNoCertificate.Latitude = Item.Latitude;
                        dbNoCertificate.InCaseOfID = Item.InCaseOfID;
                        dbNoCertificate.LabID = Item.LabID;
                        dbNoCertificate.TestReportNo = Item.TestReportNo;
                        dbNoCertificate.TestReportDate = Item.TestReportDate;
                        dbNoCertificate.ReasonNoCertificate = Item.ReasonNoCertificate;
                        dbNoCertificate.CreatedBy = Item.CreatedBy;
                        dbNoCertificate.CreatedDate = Item.CreatedDate;
                        _importService.Update(dbNoCertificate);
                    }
                    else
                    {
                        _importService.Add(Item);
                    }
                    _importService.Save();




                    Bts dbBts = _importService.findBts(proFileID, Item.BtsCode);
                    if (dbBts != null)
                    {
                        _importService.Delete(dbBts);
                        _importService.Save();
                    }
                }
            }
            return CommonConstants.Status_Success;
        }
    }
}