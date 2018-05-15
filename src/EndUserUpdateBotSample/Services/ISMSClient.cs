using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EndUserUpdateBotSample.Services
{
    public interface ISMSClient
    {
        Task SendMessageAsync(string message, string phoneNumber);
    }
}