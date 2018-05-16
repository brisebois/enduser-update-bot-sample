using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EndUserUpdateBotSample.Models;
using EndUserUpdateBotSample.Repositories;
using Microsoft.Bot.Connector;

namespace EndUserUpdateBotSample.Controllers
{
    public class UpdatesController : Controller
    {
        private readonly IRegistrationRepository _repository;
        private readonly Func<Uri, IConnectorClient> _connectorClientFactory;

        public UpdatesController(IRegistrationRepository repository, Func<Uri, IConnectorClient> connectorClientFactory)
        {
            _repository = repository;
            _connectorClientFactory = connectorClientFactory;
        }

        // GET: Updates/Publish
        public ActionResult Publish()
        {
            return View();
        }

        // POST: Updates/Publish
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Publish([Bind(Include = "Message")] Update update)
        {
            if (ModelState.IsValid)
            {
                var confirmedRegistrations = await _repository.GetByStatus("Confirmed");
                var sendsAsync = confirmedRegistrations.Select(r => SendUpdate(r, update.Message)).ToArray();
                await Task.WhenAll(sendsAsync);
            }

            return RedirectToAction("Publish");
        }

        private async Task SendUpdate(Registration registration, string updateMessage)
        {
            var context = registration.Context;
            var userAccount = new ChannelAccount(context.ToId, context.ToName);
            var botAccount = new ChannelAccount(context.FromId, context.FromName);
            var connector = _connectorClientFactory(new Uri(context.ServiceUrl));

            // Create a new message.
            IMessageActivity message = Activity.CreateMessageActivity();
            if (!string.IsNullOrEmpty(context.ConversationId) && !string.IsNullOrEmpty(context.ChannelId))
            {
                // If conversation ID and channel ID was stored previously, use it.
                message.ChannelId = context.ChannelId;
            }
            else
            {
                // Conversation ID was not stored previously, so create a conversation. 
                // Note: If the user has an existing conversation in a channel, this will likely create a new conversation window.
                context.ConversationId = (await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount)).Id;
            }

            // Set the address-related properties in the message and send the message.
            message.From = botAccount;
            message.Recipient = userAccount;
            message.Conversation = new ConversationAccount(id: context.ConversationId);
            message.Text = updateMessage;
            message.Locale = "en-us";
            await connector.Conversations.SendToConversationAsync((Activity)message);
        }
    }
}
