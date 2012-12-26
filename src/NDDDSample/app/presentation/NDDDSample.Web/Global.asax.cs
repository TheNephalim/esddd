using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using NDDDSample.Infrastructure.DI;
using NDDDSample.Persistence.MongoDb;

namespace NDDDSample.Web
{
    #region Usings

    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;  
    using Controllers;
    using Initializers;

    using Microsoft.WindowsAzure.ServiceRuntime;

    #endregion

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public void Application_Start(object sender, EventArgs e)
        {
            var builder = new ContainerBuilder();
            log4net.Config.XmlConfigurator.Configure();
            InitializeServiceLocator(builder);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            RegisterRoutes(RouteTable.Routes);
            SampleDataGenerator.LoadSampleData();
        }

        /// <summary>
        /// Instantiate the container and add all Controllers that derive from 
        /// WindsorController to the container.  Also associate the Controller 
        /// with the WindsorContainer ControllerFactory.
        /// </summary>
        protected virtual void InitializeServiceLocator(ContainerBuilder builder)
        {
            
            builder.RegisterControllers(typeof (HomeController).Assembly);

            RegisterDependencies(builder);

            //TODO: Register repositories and services for controllers
           // ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(Container));
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
            var dependencies = GetDependencies<IRequestLifeTimeDependency>();
            foreach (var dependency in dependencies)
            {
                foreach (var interfaceType in
                    dependency.GetInterfaces().Where(interfaceType => interfaceType != typeof(IRequestLifeTimeDependency) && typeof(IRequestLifeTimeDependency).IsAssignableFrom(interfaceType)))
                {
                    builder.RegisterType(dependency).As(interfaceType).InstancePerLifetimeScope();
                }
            }
            bool isRunInTheCloud;
            try
            {
                isRunInTheCloud = RoleEnvironment.IsAvailable;
            }
            catch (Exception)
            {
                isRunInTheCloud = false;
            }

            if (isRunInTheCloud)
            {
                var current = RoleEnvironment.CurrentRoleInstance;
                

                var roleInstanceEndpoints = RoleEnvironment.Roles["BookingRemoteServiceWorkerRole"]
                    .Instances
                    .Where(instance => instance != current)
                    .Select(instance => instance.InstanceEndpoints["BookingRemoteServiceWorkerRoleEndpoint"]);

                var bookingInternalEndpoint = roleInstanceEndpoints.ElementAt(new Random().Next(roleInstanceEndpoints.Count())).IPEndpoint.ToString();
                              //  builder.Register()
                //ComponentRegistrar.AddComponentsTo(this.Container, bookingInternalEndpoint);
            }
            else
            {
               // ComponentRegistrar.AddComponentsTo(this.Container);
            }
        }

        string[] assemblyNames = new[] { "NDDDSample.Infrastructure", "NDDDSample.Persistence.MongoDb", 
            "NDDDSample.Interfaces.BookingRemoteService", "NDDDSample.Interfaces.BookingRemoteService.Common", "NDDDSample.Application" };
        private HashSet<Assembly> assemblies = new HashSet<Assembly>();
        public IEnumerable<Type> GetDependencies<T>()
        {
            foreach (var assembly in assemblyNames.Select(Assembly.Load))
            {
                RecursivelyAddNDDAssemblies(assembly);
            }
            var nonAbstractPublicClassTypes = assemblies.SelectMany(a => a.GetExportedTypes()).Where(t => t.IsClass && t.IsPublic && !t.IsAbstract).ToList();

            return nonAbstractPublicClassTypes.Where(t => typeof(T).IsAssignableFrom(t));
        }

        private void RecursivelyAddNDDAssemblies(Assembly assembly)
        {
            var referencedAssemblies = assembly.GetReferencedAssemblies().Where(a => a.FullName.StartsWith("NDDSample"));
            assemblies.Add(assembly);
            foreach (var referencedAssembly in referencedAssemblies)
            {
                RecursivelyAddNDDAssemblies(Assembly.Load(referencedAssembly));
            }
        }


        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = ""} // Parameter defaults
                );
        }
    }
}