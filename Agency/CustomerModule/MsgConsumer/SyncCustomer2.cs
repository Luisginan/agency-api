using System.Diagnostics.CodeAnalysis;
using Agency.CustomerModule.Models;
using Core.Base;
using Core.Systems;
using Core.Utils.Messaging;
using Newtonsoft.Json;

namespace Agency.CustomerModule.MsgConsumer;

[ExcludeFromCodeCoverage]
public class SyncCustomer2(ILogger<SyncCustomer2> logger,
    ILogger<ConsumerBase2<CustomerPayload>> baseLogger,
    IMessagingConsumer messagingConsumer,
    IMessagingConsumerDlq deadLetterConsumer
    
) :
    ConsumerBase2<CustomerPayload>(baseLogger, messagingConsumer, deadLetterConsumer)
{

    protected override string GetTopic()
    {
        return "top";
    }
    protected override string GetGroupId()
    {
        return "top-sub";
    }

    protected override string GetDeadLetterTopic()
    {
        return "top-deadletter";
    }

    protected override string GetDeadLetterGroupId()
    {
        return "top-deadletter-sub";
    }

    protected override Task OnReceiveMessage(string key, CustomerPayload message)
    {
        return Task.Run(() =>
        {
            var messageString = JsonConvert.SerializeObject(message, Formatting.Indented);
            logger.LogInformation("SyncCustomer OnReceiveMessage: {info}", "message:" + messageString);
          
            // Task.Delay(1000).Wait();
            // throw new ConsumerMessagingException("Error processing message for testing deadletter");
                
        });
    }
    
    protected override async Task OnDeadLetterReceiveMessage(string key, CustomerPayload message)
    {
        await Task.Run(() =>
        {
            var messageString = JsonConvert.SerializeObject(message, Formatting.Indented);
            logger.LogInformation("SyncCustomer OnDeadLetterReceiveMessage: {info}", "message:" + messageString);
        });
    }
}