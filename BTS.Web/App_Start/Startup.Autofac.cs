using Autofac;
using BTS.Data.Infrastructure;
using Owin;
using Autofac.Integration.Mvc;
using System.Reflection;
using BTS.Data;
using BTS.Data.Repository;
using BTS.Service;
using System.Web.Mvc;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using BTS.Model.Models;
using System.Web;
using Microsoft.Owin.Security.DataProtection;
using BTS.Data.InfraError;

namespace BTS.Web.App_Start
{
    public partial class Startup
    {
        private void ConfigAutofac(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            // Register your Web API controllers.

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            //Register WebApi Controllers

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterType<ErrorUnitOfWork>().As<IErrorUnitOfWork>().InstancePerRequest();
            builder.RegisterType<ErrorDbFactory>().As<IErrorDbFactory>().InstancePerRequest();

            builder.RegisterType<BTSDbContext>().AsSelf().InstancePerRequest();

            //Asp.net Identity
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();

            // Repositories
            builder.RegisterAssemblyTypes(typeof(CertificateRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Services
            builder.RegisterAssemblyTypes(typeof(CertificateService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            Autofac.IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
            //Set the WebApi DependencyResolver
        }
    }
}