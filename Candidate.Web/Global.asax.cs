using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Candidate.Web.App_Start;
using Candidates.Web;
using Service;

namespace Candidate.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.DefaultConnectionFactory.CreateConnection("CandidateDbContext");
            Database.SetInitializer(new EntitiesContextInitializer());
            //AutofacWeb.Setup(GlobalConfiguration.Configuration);

            // Setup the MVC dependency resolver
            var container = AutofacWeb.Setup();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
           //Setup Job Processor
            var csvfilejob = container.Resolve<CsvFileJob>(
                new TypedParameter(typeof (Settings), DependencyResolver.Current.GetService<Settings>()),
                new TypedParameter(typeof (ICandidateService), DependencyResolver.Current.GetService<ICandidateService>()),
                new TypedParameter(typeof (DateTime), DateTime.UtcNow.AddDays(-1))
                );

            //SETUP AND START JOBS PROCESSOR
            Processor.Setup(csvfilejob, DependencyResolver.Current.GetService<EmailJob>());



            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
