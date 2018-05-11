using BTS.Data;
using BTS.Model.Models;
using BTS.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BTS.Web.App_Start.Startup))]

namespace BTS.Web.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            ConfigureAuth(app);
            ConfigAutofac(app);
            //CreateRolesandUsers(context);
            //CreateOperator(context);
            //CreateSlide(context);
            //CreatePage(context);
            //CreateContactDetail(context);
            //CreateConfigTitle(context);
        }
    }
}