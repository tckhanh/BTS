﻿using BTS.Common;
using BTS.Common.ViewModels;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Common;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BTS.Web.Controllers
{
    public class BaseController : Controller
    {
        private IErrorService _errorService;

        public BaseController(IErrorService errorService)
        {
            this._errorService = errorService;
        }

        protected bool ExecuteDatabase(Func<string, int> function, string excelConnectionString)
        {
            Error error = new Error();
            int idReturn = 0;
            try
            {
                idReturn = function.Invoke(excelConnectionString);
            }
            catch (DbEntityValidationException ex)
            {
                string description = "";
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

                error.Controller = ex.Source;
                error.Description = description;
                error.CreatedDate = DateTime.Now;
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
            }
            catch (DbUpdateException ex)
            {
                error.CreatedDate = DateTime.Now;
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
            }
            catch (Exception ex)
            {
                error.CreatedDate = DateTime.Now;
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
            }
            finally
            {
            }

            if (string.IsNullOrEmpty(error.Message))
            {
                return true;
                //return new Message(idReturn, false, "Success");
            }
            else
            {
                LogError(error);
                return false;
                //return new Message(idReturn, true, error.Message);
            }
        }

        protected bool ExecuteDatabase(Func<string, int, int> function, string excelConnectionString, out int ID)
        {
            Error error = new Error();
            ID = 0;
            try
            {
                ID = function.Invoke(excelConnectionString, 0);
            }
            catch (DbEntityValidationException ex)
            {
                string description = "";
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

                error.Controller = ex.Source;
                error.Description = description;
                error.CreatedDate = DateTime.Now;
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
            }
            catch (DbUpdateException ex)
            {
                error.CreatedDate = DateTime.Now;
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
            }
            catch (Exception ex)
            {
                error.CreatedDate = DateTime.Now;
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
            }
            finally
            {
            }

            if (string.IsNullOrEmpty(error.Message))
            {
                return true;
                //return new Message(idReturn, false, "Success");
            }
            else
            {
                LogError(error);
                return false;
                //return new Message(idReturn, true, error.Message);
            }
        }

        protected bool ExecuteDatabase(Func<string, int, int> function, string excelConnectionString, int ID)
        {
            Error error = new Error();
            int idReturn = 0;
            try
            {
                idReturn = function.Invoke(excelConnectionString, ID);
            }
            catch (DbEntityValidationException ex)
            {
                string description = "";
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

                error.Controller = ex.Source;
                error.Description = description;
                error.CreatedDate = DateTime.Now;
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
            }
            catch (DbUpdateException ex)
            {
                error.CreatedDate = DateTime.Now;
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
            }
            catch (Exception ex)
            {
                error.CreatedDate = DateTime.Now;
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
            }
            finally
            {
            }

            if (string.IsNullOrEmpty(error.Message))
            {
                return true;
                //return new Message(idReturn, false, "Success");
            }
            else
            {
                LogError(error);
                return false;
                //return new Message(idReturn, true, error.Message);
            }
        }

        private void LogError(Error error)
        {
            TempData["error"] = error.Message;
            try
            {
                error = _errorService.Create(error);
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
            if (Session[CommonConstants.CurrentCulture] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session[CommonConstants.CurrentCulture].ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session[CommonConstants.CurrentCulture].ToString());
            }
            else
            {
                Session[CommonConstants.CurrentCulture] = "vi";
                Thread.CurrentThread.CurrentCulture = new CultureInfo("vi");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi");
            }
        }

        // changing culture
        public ActionResult ChangeCulture(string ddlCulture, string returnUrl)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);

            Session[CommonConstants.CurrentCulture] = ddlCulture;
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
    }
}