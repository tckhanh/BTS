using BTS.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Infrastructure.Extensions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles)
        {
            Roles = String.Join(",", roles);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (filterContext.HttpContext.Request.IsAuthenticated)
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            // put whatever data you want which will be sent
                            // to the client
                            status = CommonConstants.Status_Error,
                            message = "Bạn Không được cấp quyền để thực hiện chức năng này"
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            // put whatever data you want which will be sent
                            // to the client
                            status = CommonConstants.Status_TimeOut,
                            message = "Xin lỗi! Đã quá thời gian chờ. Bạn đã bị đăng xuất ra khỏi hệ thống."
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            else
            {
                if (filterContext.HttpContext.Request.IsAuthenticated)
                {
                    filterContext.Controller.TempData["error"] = "Bạn Không được cấp quyền để thực hiện chức năng này";
                    //filterContext.Result = new RedirectResult(HttpContext.Current.Request.UrlReferrer.ToString());
                }
                else
                {
                    base.HandleUnauthorizedRequest(filterContext);
                }
            }
        }
    }
}