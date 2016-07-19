using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Candidate.Web;
using Service;

namespace Candidates.Web
{
    public class AutofacWeb
    {
        public static IContainer Setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<CandidateService>()
                .As<ICandidateService>().InstancePerLifetimeScope();
            builder.RegisterInstance(Settings.Instance).ExternallyOwned();

            builder.RegisterType<CsvFileJob>().AsSelf();
            builder.RegisterType<EmailJob>().AsSelf();
          //  builder.RegisterType<CsvFileJob>().Named<IJob>("CsvFileJob").WithParameter(
          //                     (pi, c) => pi.ParameterType == (typeof(Settings)),
          //                     (pi, c) => c.ResolveNamed<ICandidateService>("service")
          //                      );
          


            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            //Set dependency resolver
            var container = builder.Build();
            return container;
            // Setup the dependency resolver
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //return new AutofacDependencyResolver(container);
        }
    }
}