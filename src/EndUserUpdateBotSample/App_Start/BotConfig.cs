using Autofac;
using EndUserUpdateBotSample.Dialogs;
using EndUserUpdateBotSample.Repositories;
using Microsoft.Azure.Documents.Client;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;

namespace EndUserUpdateBotSample
{
    public class BotConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            // Register the Bot Builder module
            builder.RegisterModule(new DialogModule());
            builder.RegisterModule(new UpdateDialogModule());
            builder.RegisterModule(new AzureModule(typeof(WebApiApplication).Assembly));

            var uri = WebConfigurationManager.AppSettings["CosmosUri"];
            var key = WebConfigurationManager.AppSettings["CosmosKey"];
            var store = new DocumentDbBotDataStore(new DocumentClient(new Uri(uri), key));

            builder.Register(c => store)
                .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                .AsSelf()
                .SingleInstance();

            var appId = WebConfigurationManager.AppSettings["MicrosoftAppId"];
            var appPassword = WebConfigurationManager.AppSettings["MicrosoftAppPassword"];

            IConnectorClient func(Uri baseUri)
            {
                var credentials = new MicrosoftAppCredentials(appId, appPassword);
                MicrosoftAppCredentials.TrustServiceUrl(baseUri.AbsoluteUri);
                return new ConnectorClient(baseUri, credentials);
            }
            builder.Register<Func<Uri, IConnectorClient>>(c => func);
        }
    }
}
