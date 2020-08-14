using BTS.Data;
using BTS.Web.Infrastructure.Core;
using BTS.Web.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace BTS.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //HttpConfiguration config = GlobalConfiguration.Configuration;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfiguration.Configure();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();
            //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex is ThreadAbortException)
                return;
            WriteLog.LogError(ex);
        }
    }
}