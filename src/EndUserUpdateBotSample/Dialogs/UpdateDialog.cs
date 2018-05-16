using System;
using System.Linq;
using System.Threading.Tasks;
using EndUserUpdateBotSample.Models;
using EndUserUpdateBotSample.Repositories;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace EndUserUpdateBotSample.Dialogs
{
    [Serializable]
    public class UpdateDialog : IDialog<object>
    {
        private readonly IRegistrationRepository _repo;

        public UpdateDialog(IRegistrationRepository repo)
        {
            _repo = repo;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var message = activity.Text;
            var phoneNumber = activity.Id;
            var registration = (await _repo.GetByPhoneNumber(phoneNumber)).FirstOrDefault();
            if(registration == null)
            {
                await context.PostAsync("I'm sorry, but your phone number is not registered in my system");
            }
            else if(registration.Status == "Confirmed")
            {
                await context.PostAsync("You are already registered");
            }
            else if(registration.Status == "Unconfirmed")
            {
                if(registration.SecurityCode == message)
                {
                    registration.Status = "Confirmed";
                    registration.Context = CreateConversationContext(activity);
                    await _repo.UpdateAsync(registration);
                    await context.PostAsync("Thank you for confirming your registration");
                }
                else
                {
                    await context.PostAsync("Wrong security code");
                }
            }

            context.Wait(MessageReceivedAsync);
        }

        private ConversationContext CreateConversationContext(Activity activity)
        {
            // Extract data from the user's message that the bot will need later to send an ad hoc message to the user. 
            // Store the extracted data in a custom class "ConversationStarter" (not shown here).
            var conversationContext = new ConversationContext
            {
                ToId = activity.From.Id,
                ToName = activity.From.Name,
                FromId = activity.Recipient.Id,
                FromName = activity.Recipient.Name,
                ServiceUrl = activity.ServiceUrl,
                ChannelId = activity.ChannelId,
                ConversationId = activity.Conversation.Id
            };

            return conversationContext;
        }
    }
}