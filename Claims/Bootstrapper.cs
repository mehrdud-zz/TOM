using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;

using ClaimsPoC.Clients.Controllers;
using Factories;

namespace ClaimsPoC
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));



            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();    

            container.RegisterType<IClientFactory, ClientFactory>();
            //   container.RegisterType<ClientFactory>(new InjectionConstructor());

            container.RegisterType<IUserFactory, UserFactory>();
            container.RegisterType<UserFactory>(new InjectionConstructor());


            container.RegisterType<IClaimFactory, ClaimFactory>();
            container.RegisterType<ClaimFactory>(new InjectionConstructor());

            container.RegisterType<IClaimStatusFactory, ClaimStatusFactory>();
            container.RegisterType<ClaimStatusFactory>(new InjectionConstructor());

            container.RegisterType<IClaimTemplateFactory, ClaimTemplateFactory>();
            container.RegisterType<ClaimTemplateFactory>(new InjectionConstructor());

            container.RegisterType<IFieldTypeFactory, FieldTypeFactory>();
            container.RegisterType<FieldTypeFactory>(new InjectionConstructor());

            container.RegisterType<ICountryFactory, CountryFactory>();
            container.RegisterType<CountryFactory>(new InjectionConstructor());

            container.RegisterType<IClaimFieldTemplateFactory, ClaimFieldTemplateFactory>();
            container.RegisterType<ClaimFieldTemplateFactory>(new InjectionConstructor());

            container.RegisterType<IReportFactory, ReportFactory>();
            container.RegisterType<ReportFactory>(new InjectionConstructor());


            container.RegisterType<Acturis.Factories.IActurisClaimFactory, Acturis.Factories.ActurisClaimFactory>();
            container.RegisterType<Acturis.Factories.ActurisClaimFactory>(new InjectionConstructor());

            container.RegisterType<Eclipse.Factories.IEclipseClaimFactory, Eclipse.Factories.EclipseClaimFactory>();
            container.RegisterType<Eclipse.Factories.EclipseClaimFactory>(new InjectionConstructor());

            container.RegisterType<Eclipse.Factories.IEclipsePolicyFactory, Eclipse.Factories.EclipsePolicyFactory>();
            container.RegisterType<Eclipse.Factories.EclipsePolicyFactory>(new InjectionConstructor());


            container.RegisterType<IPageSetupFactory, PageSetupFactory>();
            container.RegisterType<PageSetupFactory>(new InjectionConstructor());

            container.RegisterType<IPageElementFactory, PageElementFactory>();
            container.RegisterType<PageElementFactory>(new InjectionConstructor());


            container.RegisterType<IReportFieldFactory, ReportFieldFactory>();
            container.RegisterType<ReportFieldFactory>(new InjectionConstructor());

            container.RegisterType<IClaimFieldGroupTemplateFactory, ClaimFieldGroupTemplateFactory>();
            container.RegisterType<ClaimFieldGroupTemplateFactory>(new InjectionConstructor()); 

            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }

    }
}