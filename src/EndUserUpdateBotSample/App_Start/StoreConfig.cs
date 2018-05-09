using Autofac;
using EndUserUpdateBotSample.Repositories;
using Microsoft.Azure.Documents.Client;
using System;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;

namespace EndUserUpdateBotSample
{
    public class StoreConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            var uri = WebConfigurationManager.AppSettings["CosmosUri"];
            var key = WebConfigurationManager.AppSettings["CosmosKey"];
            var dbName = WebConfigurationManager.AppSettings["CosmosDBName"];
            var collectionName = WebConfigurationManager.AppSettings["CosmosCollectionName"];
            builder.Register((c, p) => new DocumentClient(new Uri(uri), key));
            builder.Register((c,p)=> new RegistrationRepository(c.Resolve<DocumentClient>(), dbName, collectionName)).As<IRegistrationRepository>();
        }

        public static void InitStore(IDependencyResolver resolver)
        {
            var dbName = WebConfigurationManager.AppSettings["CosmosDBName"];
            var collectionName = WebConfigurationManager.AppSettings["CosmosCollectionName"];
            resolver.GetService<IRegistrationRepository>().InitStore(dbName, collectionName).Wait();
        }
    }
}
