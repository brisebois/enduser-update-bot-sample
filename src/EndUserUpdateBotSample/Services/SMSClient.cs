using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace EndUserUpdateBotSample.Services
{
    public class SMSClient : ISMSClient
    {
        private readonly ITwilioRestClient _client;
        private readonly string _fromPhonenumber;

        public SMSClient(ITwilioRestClient client, string fromPhoneNumber)
        {
            _client = client;
            _fromPhonenumber = fromPhoneNumber;

            var accountSid = "ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            // Your Auth Token from twilio.com/console
            var authToken = "auth_token";

            _client = new TwilioRestClient(accountSid, authToken);
        }

        public async Task SendMessageAsync(string message, string phoneNumber)
        {
            var messageResource = await MessageResource.CreateAsync(
                to: new PhoneNumber(phoneNumber),
                from: new PhoneNumber(_fromPhonenumber),
                body: message, client: _client);
        }
    }
}