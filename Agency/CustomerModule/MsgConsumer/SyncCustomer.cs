using System.Diagnostics.CodeAnalysis;
using Agency.CustomerModule.Models;
using Core.Base;
using Core.Utils.Messaging;
using Newtonsoft.Json;

namespace Agency.CustomerModule.MsgConsumer;

[ExcludeFromCodeCoverage]
public class SyncCustomer(ILogger<SyncCustomer> logger,
    ILogger<ConsumerBase<CustomerPayload>> baseLogger,
    IMessagingConsumer messagingConsumer) :
    ConsumerBase<CustomerPayload>(baseLogger, messagingConsumer)
{

    protected override string GetGroupId()
    {
        return "top-sub";
    }

    protected override string GetTopic()
    {
        return "top";
    }


    protected override Task OnReceiveMessage(string key, CustomerPayload message)
    {
        return Task.Run(() =>
        {
            var messageString = JsonConvert.SerializeObject(message, Formatting.Indented);
            logger.LogInformation("SyncCustomer OnReceiveMessage: {info}", "message:" + messageString);
          
            Task.Delay(1000).Wait();
                
        });
    }
}