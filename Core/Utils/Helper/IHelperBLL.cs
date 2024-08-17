namespace Core.Utils.Helper;


public interface IHelperBll
{
    void LogInfo(object objectMessage);
    void LogError(object message, Exception exception);
    long InsertLogTransactionApi(string tag, string type, string level, string endPoint, string apiMethod, string message, object requestBody);
    long InsertLogTransactionApi(string tag, string type, string level, string endPoint, string apiMethod, string message, object requestBody, string api_request_id, object? request_header, string requestIdApp, string contractNoApp);
    void UpdateLogTransactionApi(long logId, object responseBody);

}
