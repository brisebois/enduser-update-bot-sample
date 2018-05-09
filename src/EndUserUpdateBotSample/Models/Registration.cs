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
        public string Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        public string SecurityCode { get; set; }
        public string Status { get; set; }
    }
}