using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Core.Utils.DB;

[ExcludeFromCodeCoverage]
public class ConsumerLogPostgres(ILogDb nawaDaoRepository, ILogger<ConsumerLogPostgres> logger) : IConsumerLog
{
    public async Task InsertAsync(Message<object> message)
    {
        var task = new Task(() => {
            try
            {
                var payLoad = JsonConvert.SerializeObject(message.PayLoad);
                var logData = new MessagingLog
                {
                    Topic = message.Topic,
                    GroupId = message.GroupId,
                    AppId = message.AppId,
                    Key = message.Key,
                    PayLoad = payLoad,
                    Retry = message.Retry,
                    Error = message.Error ?? "",
                    Method = "consumer"
                };
                nawaDaoRepository.ExecuteNonQuery("insert into messaging_log (topic, group_id, app_id, key, payload, retry, error, method) values (@topic, @group_id, @app_id, @key, @payload, @retry,@error, @method)", new FieldParameter[]
                {
                    new("@topic", logData.Topic),
                    new("@group_id", logData.GroupId),
                    new("@app_id", logData.AppId ?? ""),
                    new("@key", logData.Key),
                    new("@payload", logData.PayLoad),
                    new("@retry", logData.Retry),
                    new("@error", logData.Error),
                    new("@method", logData.Method)
                });
               logger.LogDebug("ConsumerLogPostgres InsertAsync: {info}", "Key= " + logData.Key);
            }
            catch (Exception e)
            {
              logger.LogError("ConsumerLogPostgres InsertAsync: {info}", "Error= " + e.Message);
            }
        }); 
        task.Start();
        await task;
    }

    public async Task UpdateAsync(string key, Message<object> message)
    {
       var task = new Task(() => {
                try
                {
                    var payLoad = JsonConvert.SerializeObject(message.PayLoad);
                    var logData = new MessagingLog
                    {
                        Topic = message.Topic,
                        GroupId = message.GroupId,
                        AppId = message.AppId,
                        Key = message.Key,
                        PayLoad = payLoad,
                        Retry = message.Retry,
                        Error = message.Error ?? "",
                        Method = "consumer"
                    };
                    nawaDaoRepository.ExecuteNonQuery("update messaging_log set topic = @topic, group_id = @group_id, app_id = @app_id, payload = @payload, retry = @retry, error = @error, method = @method where key = @key", new FieldParameter[]
                    {
                        new("@topic", logData.Topic),
                        new("@group_id", logData.GroupId),
                        new("@app_id", logData.AppId ?? ""),
                        new("@key", logData.Key),
                        new("@payload", logData.PayLoad),
                        new("@retry", logData.Retry),
                        new("@error", logData.Error),
                        new("@method", logData.Method)
                    });
                    logger.LogDebug("ConsumerLogPostgres UpdateAsync: {info}", "Key= " + logData.Key);
                }
                catch (Exception e)
                {
                    logger.LogError("ConsumerLogPostgres UpdateAsync: {info}", "Error= " + e.Message);
                }
       }); 
       task.Start();
       await task;
    }

    public async Task<Message<object>?> GetAsync(string key)
    {
        var task = new Task<Message<object>?>(() =>
        {
            try
            {
                var logData = nawaDaoRepository.ExecuteRow<MessagingLog>("select * from messaging_log where key = @key",
                [
                    new FieldParameter("@key", key)
                ]);
                
               
                if (logData == null)
                    return null;
                
                var payLoad = JsonConvert.DeserializeObject<object>(logData.PayLoad);
                var data = new Message<object>
                {
                    Topic = logData.Topic,
                    GroupId = logData.GroupId,
                    AppId = logData.AppId,
                    Key = logData.Key,
                    PayLoad = payLoad,
                    Retry = logData.Retry,
                    Error = logData.Error
                };
                
                logger.LogDebug("ConsumerLogPostgres GetAsync: {info}", "Key= " + key);
                return data;
            }
            catch (Exception e)
            {
                logger.LogError("ConsumerLogPostgres GetAsync: {info}", "Error= " + e.Message);
                return null;
            }
        });
        
        task.Start();
        await task;
        return task.Result;
    }

    public async Task DeleteAsync(string key)
    {
       var task = new Task(() => {
            try
            {
                nawaDaoRepository.ExecuteNonQuery("delete from messaging_log where key = @key", new FieldParameter[]
                {
                    new("@key", key)
                });
                logger.LogDebug("ConsumerLogPostgres DeleteAsync: {info}", "Key= " + key);
            }
            catch (Exception e)
            {
                logger.LogError("ConsumerLogPostgres DeleteAsync: {info}", "Error= " + e.Message);
            }
       });
       
       task.Start();
       await task;
    }
}