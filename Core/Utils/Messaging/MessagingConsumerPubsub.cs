using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Core.Config;
using Core.Systems;
using Core.Utils.Security;
using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Options;

namespace Core.Utils.Messaging;
[ExcludeFromCodeCoverage]
public class MessagingConsumerPubsub(
    ILogger<MessagingConsumerPubsub> logger,
    IOptions<MessagingConfig> messagingConfig,
    IMetrics metrics,
    IVault vault)
    : IMessagingConsumer, IMessagingConsumerDlq
{
    private readonly ActivitySource _activitySource = new("messaging.consumer");
    private readonly MessagingConfig _messagingConfig = vault.RevealSecret(messagingConfig.Value);
    private bool _isListening;
    public void Dispose()
    {
       
    }

    public async Task Listening(CancellationToken stoppingToken)
    {
        _isListening = true;
        await PullMessages(stoppingToken);
    }
    
    public async Task StopListening()
    {
        if (_subscriber == null) return;
        if (!_isListening) return;
        
        _isListening = false;
        await _subscriber.StopAsync(CancellationToken.None);
        await _subscriber.DisposeAsync();
    }
    
    private SubscriberClient? _subscriber;

    public event Func<string, string, Task>? OnReceiveMessage;
    public string GroupId { get; set; } = "";
    public string Topic { get; set; } = "";

    public string FullTopic => GetFullTopic();
    public string FullGroupId => GetFullGroupId();

    private async Task PullMessages(CancellationToken stoppingToken)
    {
        using var pullActivity = _activitySource.StartActivity(ActivityKind.Consumer);
        try
        {
            metrics.InitCounter(GetMetricNameCounter(),
                GetMetricNameCounter());
            metrics.InitHistogram(GetMetricNameHistogramSuccess(),
                GetMetricNameHistogramSuccess(),
                GetBucketHistogram());
            metrics.InitHistogram(GetMetricNameHistogramFailed(),
                GetMetricNameHistogramFailed(),
                GetBucketHistogram());
            
            if (stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("MessagingConsumerPubsub OnPullMessages: {info}", "Cancellation requested, stopping message processing.");
                return;
            }
            var subscriptionName = SubscriptionName.FromProjectSubscription(_messagingConfig.ProjectId, FullGroupId);

            _subscriber = await SubscriberClient.CreateAsync(subscriptionName);
              
            var startTask = _subscriber.StartAsync(async (message,cancellationToken) => 
                await ConsumeMessage(message, cancellationToken));
               
            await startTask;
               
        }
        catch (Exception e)
        {
            logger.LogError("MessagingConsumerPubsub OnPullMessages: {info}", $"Error= {e.Message}");
            SetActivityOnError(pullActivity, e);
        }
    }

    private static double[] GetBucketHistogram()
    {
        return [1.0, 3.0, 5.0];
    }

    private string GetMetricNameHistogramFailed()
    {
        return "duration_consume_topic_failed_" + FullTopic.Replace("-", "");
    }

    private string GetMetricNameHistogramSuccess()
    {
        return "duration_consume_topic_success_" + FullTopic.Replace("-", "");
    }

    private string GetMetricNameCounter()
    {
        return "consume_topic_" + FullTopic.Replace("-", "");
    }

    private void SetActivityOnError(Activity? pullActivity, Exception e)
    {
        pullActivity?.SetTag("topic", FullTopic);
        pullActivity?.SetTag("groupId", FullGroupId);
        pullActivity?.SetTag("exception", e.Message);
        pullActivity?.SetTag("stackTrace", e.StackTrace);
        pullActivity?.SetTag("exceptionType", e.GetType().ToString());
        pullActivity?.SetTag("exceptionSource", e.Source);
        pullActivity?.SetTag("exceptionTargetSite", e.TargetSite?.ToString());
        pullActivity?.SetTag("exceptionData", e.Data.ToString());
        pullActivity?.SetStatus(ActivityStatusCode.Error, "Error when pulling messages: " + e.Message);
    }

    private async Task<SubscriberClient.Reply> ConsumeMessage(
        PubsubMessage message, CancellationToken cancellationToken)
    {
        using var startAsyncActivity = _activitySource.StartActivity();
        if (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("MessagingConsumerPubsub ConsumeMessage: {info}", 
                "Cancellation requested, stopping message processing.");
            return await Task.FromResult(SubscriberClient.Reply.Nack);
        }
                
        var sw = new Stopwatch();
        try
        {
            var text = message.Data.ToStringUtf8();
            logger.LogDebug("MessagingConsumerPubsub ConsumeMessage Start: {info}", 
                $"Topic= {GetFullTopic()}. Message= {text}");

            sw.Start();
            metrics.SetCounter(GetMetricNameCounter());
            
            if (OnReceiveMessage != null)
                await OnReceiveMessage(message.MessageId, text);
            
            if (message.Attributes != null)
            {
                foreach (var attribute in message.Attributes)
                {
                    Console.WriteLine($"{attribute.Key} = {attribute.Value}");
                }
            }

            sw.Stop();
            logger.LogInformation("MessagingConsumerPubsub ConsumeMessage Success : {info}",
                $"Message= {text} Duration: {sw.ElapsedMilliseconds} ms");
            metrics.SetHistogram(GetMetricNameHistogramSuccess(), sw.Elapsed.TotalSeconds);

            startAsyncActivity?.SetTag("topic", FullTopic);
            startAsyncActivity?.SetTag("messageId", message.MessageId);
            startAsyncActivity?.SetTag("groupId", FullGroupId);

            return await Task.FromResult(SubscriberClient.Reply.Ack);

        }
        catch (Exception e)
        {
            sw.Stop();
            logger.LogError(
                "MessagingConsumerPubsub ConsumeMessage Failed: {info}",
                $"Message= Topic: {GetFullTopic()}. {message.Data.ToStringUtf8()} Duration: {sw.ElapsedMilliseconds} ms Error: {e.Message}");
            metrics.SetHistogram(GetMetricNameHistogramFailed(), sw.Elapsed.TotalSeconds);

           SetActivityOnError(startAsyncActivity, e);
            return await Task.FromResult(SubscriberClient.Reply.Nack);
        }
    }
    
    private string GetFullTopic()
    {
        return string.IsNullOrEmpty(_messagingConfig.TopicSuffix) ? Topic : $"{Topic}-{_messagingConfig.TopicSuffix}";
    }
    
    private string GetFullGroupId()
    {
        if (string.IsNullOrEmpty(_messagingConfig.TopicSuffix))
        {
            return GroupId;
        }
        else
        {
            var realGroupId = GroupId;
            var result = realGroupId.Substring(0, realGroupId.Length - 3);

            return $"{result}{_messagingConfig.TopicSuffix}-sub";
        }
    }
}