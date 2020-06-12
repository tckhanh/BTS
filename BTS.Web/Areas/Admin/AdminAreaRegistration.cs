using System.Web.Mvc;

namespace BTS.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new {controller ="ApplicationUser", action = "Index", id = UrlParameter.Optional },
                new[] { "BTS.Web.Areas.Admin.Controllers" }
            );
        }
    }
}