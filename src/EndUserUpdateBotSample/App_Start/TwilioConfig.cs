using Autofac;
using EndUserUpdateBotSample.Repositories;
using EndUserUpdateBotSample.Services;
using Microsoft.Azure.Documents.Client;
using System;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using Twilio.Clients;

namespace EndUserUpdateBotSample
{
    public class TwilioConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            var username = WebConfigurationManager.AppSettings["TwilioUsername"];
            var password = WebConfigurationManager.AppSettings["TwilioPassword"];
            var fromPhoneNumber = WebConfigurationManager.AppSettings["FromPhoneNumber"];
            builder.RegisterInstance<ITwilioRestClient>(new TwilioRestClient(username, password));
            builder.RegisterType<SMSClient>().As<ISMSClient>().WithParameter("fromPhoneNumber", fromPhoneNumber);
        }
    }
}
