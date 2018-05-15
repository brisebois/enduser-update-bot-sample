using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EndUserUpdateBotSample.Dialogs
{
    public class UpdateDialogModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // register the top level dialog
            builder.RegisterType<UpdateDialog>().As<IDialog<object>>().InstancePerDependency();
            builder.Register((c,p) => new UpdateDialog()).AsSelf().InstancePerDependency();
        }
    }
}