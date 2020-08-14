using BTS.Common;
using BTS.Common.ViewModels;
using BTS.Data.ApplicationModels;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.App_Start;
using BTS.Web.Common;
using BTS.Web.Infrastructure.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BTS.Web.Controllers
{
    public class WebBaseController : Controller
    {
        private IErrorService _errorService;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public WebBaseController(IErrorService errorService)
        {
            _errorService = errorService;            
        }

        [HttpPost]
        public JsonResult GetUserRoles()
        {
            return Json(new
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Roles = UserManager.GetRolesAsync(User.Identity.GetUserId())
            });
        }

        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        protected ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        protected async Task<bool> addUserRole(string userId, string roleName)
        {
            string description = "";
            try
            {
                await UserManager.AddToRoleAsync(userId, roleName);
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }
                LogError(ex, description);
                throw;
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
            finally
            {
            }
        }

        protected async Task<bool> addUserRoles(string userId, IEnumerable<ApplicationRole> roles)
        {
            string description = "";
            try
            {
                foreach (var role in roles)
                {
                    await UserManager.AddToRoleAsync(userId, role.Name);
                }

                return true;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }
                LogError(ex, description);
                throw;
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
            finally
            {
            }
        }

        protected async Task<bool> removeUserRole(string userId, string roleName)
        {
            string description = "";
            try
            {
                await UserManager.RemoveFromRoleAsync(userId, roleName);
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }
                LogError(ex, description);
                throw;
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
            finally
            {
            }
        }

        protected async Task<bool> removeUserRoles(string userId)
        {
            string description = "";
            try
            {
                //Xóa Roles cũ Tạo Roles mới cho User
                var userRoles = await UserManager.GetRolesAsync(userId);
                foreach (var role in userRoles)
                {
                    await UserManager.RemoveFromRoleAsync(userId, role);
                }
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }
                LogError(ex, description);
                throw;
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
            finally
            {
            }
        }


        protected void ExecuteDatabase(Func<string, string> function, string fileLocation)
        {
            string strReturn;
            string description = "";
            try
            {
                strReturn = function.Invoke(fileLocation);
                if (strReturn != CommonConstants.Status_Success)
                {
                    LogError(strReturn);
                    throw new Exception(strReturn);
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }
                LogError(ex, description);
                throw;
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
            finally
            {

            }
        }


        // Thuc hien lenh ghi Log neu có loi va nem loi ra ngoai
        protected void ExecuteDatabase(Func<string, string, string> function, string fileLocation, string InputType)
        {
            string strReturn;
            string description = "";
            try
            {
                strReturn = function.Invoke(fileLocation, InputType);
                if (strReturn != CommonConstants.Status_Success)
                {
                    LogError(strReturn);
                    throw new Exception(strReturn);
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }
                LogError(ex, description);
                throw;
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
            finally
            {

            }
        }

        protected void ExecuteDatabase(Func<string, string, string, string> function, string fileLocation, string Group, string InputType)
        {
            string strReturn;
            string description = "";
            try
            {
                strReturn = function.Invoke(fileLocation, Group, InputType);
                if (strReturn != CommonConstants.Status_Success)
                {
                    LogError(strReturn);
                    throw new Exception(strReturn);
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }
                LogError(ex, description);
                throw;
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
            finally
            {

            }
        }
        protected void ExecuteDatabaseAsyn(Func<string, Task<string>> function, string excelConnectionString)
        {
            Task<string> strReturn;
            string description = "";
            try
            {
                strReturn = function.Invoke(excelConnectionString);
                if (strReturn?.ToString() != CommonConstants.Status_Success)
                {
                    LogError(strReturn?.ToString());
                    throw new Exception(strReturn?.ToString());
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }
                LogError(ex, description);
                throw;
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
            finally
            {
            }
        }

        protected void ExecuteDatabase(Func<string, string, string> function, string excelConnectionString, out string strReturn)
        {
            strReturn = "";
            string description = "";
            try
            {
                strReturn = function.Invoke(excelConnectionString, strReturn);
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }
                LogError(ex, description);
                throw;
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
            finally
            {
            }
        }

        public void LogError(string description)
        {
            TempData["error"] = description;
            try
            {
                Error error = new Error();
                error.CreatedDate = DateTime.Now;
                error.Message = description;
                error.Description = description;
                _errorService.Create(error);
                _errorService.Save();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Trace.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public void LogError(Exception e, string description = "")
        {
            TempData["error"] = e.Message;
            try
            {
                Error error = new Error();
                error.Controller = e.Source;
                error.CreatedDate = DateTime.Now;
                error.Message = e.Message;
                error.Description = description;
                error.StackTrace = e.StackTrace;
                _errorService.Create(error);
                _errorService.Save();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Trace.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        //initilizing culture on controller initialization
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            // nếu đã cấu hình <system.web> <globalization uiCulture = "vi-VN" culture = "vi-VN"/> </system.web> trong Web.config thì không cần đoạn mã bên dưới

            //if (Session[CommonConstants.CurrentCulture] != "vi")
            //{
            //    Thread.CurrentThread.CurrentCulture = new CultureInfo("vi");
            //    Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi");
            //    Session[CommonConstants.CurrentCulture] = "vi";
            //}
        }

        // changing culture
        public ActionResult ChangeCulture(string ddlCulture, string returnUrl)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);

            Session[CommonConstants.CULTURE_SESSION] = ddlCulture;
            return Redirect(returnUrl);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            //if (session == null)
            //{
            //    filterContext.Result = new RedirectToRouteResult(new
            //        RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Admin" }));
            //}
            base.OnActionExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            // Log the error somehow then continue
            LogError(filterContext.Exception);
            // Mark the error as handled and switch execution to the error view
            // If you forget to mark the exception as handled, your users will see the normal ASP.NET error page.

            //filterContext.ExceptionHandled = true;
            //this.View("Error").ExecuteResult(filterContext.Controller.ControllerContext);
        }

        public string getCityIDsScope()
        {

            if (User.Identity.IsAuthenticated == true)
                return User.Identity.getUserField("CityIDsScope") ?? UserManager.FindByName(User.Identity.Name).CityIDsScope;
            else
                return "";
        }

        public string getUserField_(string name)
        {
            if (User.Identity.IsAuthenticated == true)
                return User.Identity.getUserField(name) ?? UserManager.FindByName(User.Identity.Name).CityIDsScope;
            else
                return "";
        }

        public void loadLicense()
        {
            string licenseFile = Server.MapPath("~/Content/ReportTemplates/license.key");
            Stimulsoft.Base.StiLicense.LoadFromFile(licenseFile);
        }
    }
}