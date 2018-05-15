using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using EndUserUpdateBotSample.Dialogs;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EndUserUpdateBotSample
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;

            Conversation.UpdateContainer(
                builder =>
                {
                    // Register the Bot Builder module
                    builder.RegisterModule(new DialogModule());
                    builder.RegisterModule(new UpdateDialogModule());
                    builder.RegisterModule(new AzureModule(typeof(WebApiApplication).Assembly));

                    // Register your MVC controllers. (MvcApplication is the name of
                    // the class in Global.asax.)
                    builder.RegisterControllers(typeof(WebApiApplication).Assembly);

                    // Register your Web API controllers.
                    builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);
                    builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

                    // OPTIONAL: Register model binders that require DI.
                    builder.RegisterModelBinders(typeof(WebApiApplication).Assembly);
                    builder.RegisterModelBinderProvider();

                    // OPTIONAL: Register web abstractions like HttpContextBase.
                    builder.RegisterModule<AutofacWebTypesModule>();

                    // OPTIONAL: Enable property injection in view pages.
                    builder.RegisterSource(new ViewRegistrationSource());

                    // OPTIONAL: Enable property injection into action filters.
                    builder.RegisterFilterProvider();

                    StoreConfig.Configure(builder);
                    TwilioConfig.Configure(builder);

                    AreaRegistration.RegisterAllAreas();
                    GlobalConfiguration.Configure(WebApiConfig.Register);
                    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                    RouteConfig.RegisterRoutes(RouteTable.Routes);
                    BundleConfig.RegisterBundles(BundleTable.Bundles);
                    //WebApiConfig.Register(GlobalConfiguration.Configuration);
                });

            // Set the dependency resolver to be Autofac.
            config.DependencyResolver = new AutofacWebApiDependencyResolver(Conversation.Container);
            var resolver = new AutofacDependencyResolver(Conversation.Container);
            DependencyResolver.SetResolver(resolver);
            StoreConfig.InitStore(resolver);
        }

        public static ILifetimeScope FindContainer()
        {
            var config = GlobalConfiguration.Configuration;
            var resolver = (AutofacWebApiDependencyResolver)config.DependencyResolver;
            return resolver.Container;
        }
    }
}
