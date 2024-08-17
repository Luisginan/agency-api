namespace Core.Utils.Messaging;

public interface IMessagingProducer: IDisposable
{
    Task Produce(string topic, string key, string message);

}