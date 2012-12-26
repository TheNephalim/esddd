using System.Linq;
using System.Reflection;
using Autofac;

namespace NDDDSample.Web.Initializers
{
    #region Usings

    using System;
    using System.ServiceModel;

    using NDDDSample.Infrastructure.Utils;
    using NDDDSample.Interfaces.BookingRemoteService.Common;

    #endregion

    public static class ComponentRegistrar
    {
        private static string bookingRemoteServiceWorkerRoleEndpoint = "localhost:8081";

        public static void AddComponentsTo(ContainerBuilder builder, string bookingEndpoint)
        {
            bookingRemoteServiceWorkerRoleEndpoint = bookingEndpoint;
            AddComponentsTo(builder);
        }

        public static void AddComponentsTo(ContainerBuilder builder)
        {
            AddCustomRepositoriesTo(builder);
        }

        private static void AddCustomRepositoriesTo(ContainerBuilder builder)
        {
//            var assembly = Assembly.Load("NDDDSample.Persistence.NHibernate");
//            assembly.GetTypes()
//                .Where(type => type.Namespace.Equals("NDDDSample.Domain.Model", StringComparison.CurrentCultureIgnoreCase))
//                .First
//            builder.Register<NDDDSample.Domain.Model>(
//                AllTypes.Pick()
//                    //Scan repository assembly for domain model interfaces implementation
//                    .FromAssemblyNamed("")
//                    .WithService.FirstNonGenericCoreInterface("NDDDSample.Domain.Model"));
//
//            builder.AddFacility<WcfFacility>();
//
//            builder.Register(
//                Component.For<MessageLifecycleBehavior>(),                
//                Component.For<IBookingServiceFacade>()                
//                    .Named("bookingServiceFacade")
//                    .LifeStyle.Transient
//                .ActAs(DefaultClientModel
//                    .On(WcfEndpoint.BoundTo(new NetTcpBinding())
//                        .At(String.Format("net.tcp://{0}/BookingServiceFacade", bookingRemoteServiceWorkerRoleEndpoint))
//                        )));  
//          
//              var interfaces = type.GetInterfaces()
//                                                 .Where(
//                                                 t =>
//                                                 t.IsGenericType == false && t.Namespace.StartsWith(interfaceNamespace));
//
//                                             if (interfaces.Count() > 0)
//                                             {
//                                                 return new[] {interfaces.ElementAt(0)};
//                                             }
            var assembly = Assembly.Load("NDDDSample.Persistence.NHibernate");
            string ndddsampleDomainModel = "NDDDSample.Domain.Model";
            var repoType = assembly.GetTypes().First(type => type.Namespace.Equals(ndddsampleDomainModel, StringComparison.CurrentCultureIgnoreCase));


            //            builder.AddFacility<WcfFacility>();

            //            builder.Register(
            //                Component.For<MessageLifecycleBehavior>(),                
            //                Component.For<IBookingServiceFacade>()                
            //                    .Named("bookingServiceFacade")
            //                    .LifeStyle.Transient
            //                .ActAs(DefaultClientModel
            //                    .On(WcfEndpoint.BoundTo(new NetTcpBinding())
            //                        .At(String.Format("net.tcp://{0}/BookingServiceFacade", bookingRemoteServiceWorkerRoleEndpoint))
            //                        )));  

//            var interfaces = repoType.GetInterfaces()
//                                                 .Where(
//                                                 t =>
//                                                 t.IsGenericType == false && t.Namespace.StartsWith(ndddsampleDomainModel));
//
//            if (interfaces.Count() > 0)
//            {
//                return new[] { interfaces.ElementAt(0) };
//            }

        }
    }
}