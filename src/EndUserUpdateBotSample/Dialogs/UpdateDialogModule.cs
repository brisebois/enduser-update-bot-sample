using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System;

namespace EndUserUpdateBotSample.Dialogs
{
    public class UpdateDialogModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // register the top level dialog
            builder.RegisterType<UpdateDialog>().As<IDialog<object>>().InstancePerDependency();
            builder.RegisterType<UpdateDialog>().InstancePerDependency();
            /*builder.Register((c, p) => p.TypedAs<Func<IDialog<object>>>())
                .AsSelf()
                .InstancePerMatchingLifetimeScope(DialogModule.LifetimeScopeTag);*/
        }
    }
}