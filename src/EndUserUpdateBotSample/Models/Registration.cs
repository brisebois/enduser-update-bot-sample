using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EndUserUpdateBotSample.Models
{
    public class Registration
    {
        // JsonProperty is required for the CosmosDB SDK to use camel casing when
        // using LINQ. Ref: https://github.com/Azure/azure-documentdb-dotnet/issues/317
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [DisplayName("Phone Number")]
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [DisplayName("Security Code")]
        [JsonProperty("securityCode")]
        public string SecurityCode { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        public ConversationContext Context { get; set; }
    }

    public class ConversationContext
    {
        public string ToId { get; set; }
        public string ToName { get; set; }
        public string FromId { get; set; }
        public string FromName { get; set; }
        public string ServiceUrl { get; set; }
        public string ChannelId { get; set; }
        public string ConversationId { get; set; }
    }
}