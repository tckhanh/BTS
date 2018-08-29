using BTS.Common;
using BTS.Common.ViewModels;
using BTS.Data.ApplicationModels;
using BTS.ExcelLib;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    [AuthorizeRoles(CommonConstants.Data_CanImport_Role)]
    public class ImportUserController_28082018 : BaseController
    {
        // GET: Import
        private IImportService _importService;

        private ExcelIO _excelIO;
        private NumberFormatInfo provider;

        public ImportUserController_28082018(IImportService importService, IErrorService errorService) : base(errorService)
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
            TempData["ImagePath"] = User.Identity.GetImagePath();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HttpPostedFileBase file)
        {
            int result = 0;
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/AppFiles/Tmp/") + Request.Files["file"].FileName;
                    try
                    {
                        if (System.IO.File.Exists(fileLocation))
                            System.IO.File.Delete(fileLocation);

                        Request.Files["file"].SaveAs(fileLocation);

                        string[] columnNames = new string[] {
                            };
                        _excelIO.FormatColumnDecimalToText(fileLocation, columnNames);

                        string excelConnectionString = _excelIO.CreateConnectionString(fileLocation, fileExtension);
                        int ProfileID = 0;

                        ExecuteDatabase(ImportUser, excelConnectionString);
                    }
                    catch (Exception e)
                    {
                        // Base Controller đã ghi Log Error rồi
                        return Json(new { status = CommonConstants.Status_Error, message = e.Message }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { status = CommonConstants.Status_Success, message = "Nhập khẩu người dùng thành công !" }, JsonRequestBehavior.AllowGet);
        }

        private int ImportUser(string excelConnectionString)
        {
            DataTable dt = _excelIO.ReadSheet(excelConnectionString, CommonConstants.Sheet_User);
            IdentityResult result = new IdentityResult();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][CommonConstants.Sheet_User_FullName].ToString()))
                {
                    ApplicationUser newAppUser = new ApplicationUser();
                    newAppUser.FullName = dt.Rows[i][CommonConstants.Sheet_User_FullName].ToString();
                    newAppUser.BirthDay = DateTime.Parse(dt.Rows[i][CommonConstants.Sheet_User_BirthDay].ToString());
                    newAppUser.FatherLand = dt.Rows[i][CommonConstants.Sheet_User_FatherLand].ToString();
                    newAppUser.Level = dt.Rows[i][CommonConstants.Sheet_User_Level].ToString();
                    newAppUser.EducationalField = dt.Rows[i][CommonConstants.Sheet_User_EducationalField].ToString();
                    if (dt.Rows[i][CommonConstants.Sheet_User_EntryDate].ToString().Length > 0)
                        newAppUser.EntryDate = DateTime.Parse(dt.Rows[i][CommonConstants.Sheet_User_EntryDate].ToString());
                    if (dt.Rows[i][CommonConstants.Sheet_User_OfficialDate].ToString().Length > 0)
                        newAppUser.OfficialDate = DateTime.Parse(dt.Rows[i][CommonConstants.Sheet_User_OfficialDate].ToString());
                    if (dt.Rows[i][CommonConstants.Sheet_User_EndDate].ToString().Length > 0)
                        newAppUser.EndDate = DateTime.Parse(dt.Rows[i][CommonConstants.Sheet_User_EndDate].ToString());
                    newAppUser.WorkingDuration = dt.Rows[i][CommonConstants.Sheet_User_WorkingDuration].ToString();
                    newAppUser.JobPositions = dt.Rows[i][CommonConstants.Sheet_User_JobPositions].ToString();
                    newAppUser.PhoneNumber = dt.Rows[i][CommonConstants.Sheet_User_Telephone].ToString();
                    newAppUser.ImagePath = "/AppFiles/Images/" + dt.Rows[i][CommonConstants.Sheet_User_Image].ToString();
                    newAppUser.UserName = dt.Rows[i][CommonConstants.Sheet_User_UserName].ToString();
                    newAppUser.Email = dt.Rows[i][CommonConstants.Sheet_User_Email].ToString();
                    newAppUser.CreatedBy = User.Identity.Name;
                    newAppUser.CreatedDate = DateTime.Now;
                    ApplicationUser user = UserManager.FindByName(newAppUser.UserName);

                    if (user == null)
                    {
                        result = UserManager.Create(newAppUser, dt.Rows[i][CommonConstants.Sheet_User_Password].ToString());
                        if (!result.Succeeded)
                        {
                            throw new Exception("Lỗi khi nhập khẩu người dùng [" + newAppUser.UserName + "]:" + string.Join("\n", result.Errors));
                        }
                    }
                }
            }
            return 1;
        }
    }
}