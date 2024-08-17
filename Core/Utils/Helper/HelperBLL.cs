using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Core.CExceptions;
using Core.Utils.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Utils.Helper;

[ExcludeFromCodeCoverage]
public class HelperBll(IQueryBuilderRepository queryBuilder, INawaDaoRepository nawaDao)
    : IHelperBll
{
    private static readonly string MachineName = Environment.MachineName;
        
    public void LogInfo(object objectMessage)
    {
        try
        {
            #region validasi parameter
            ArgumentNullException.ThrowIfNull(objectMessage);
            #endregion

            const string queryKey = "OneLoan.HelperBLL.LogInfo";
            var query = queryBuilder.GetQuery(queryKey);

            if (string.IsNullOrWhiteSpace(query))
            {
                throw new QueryNotFoundHelperBllException(queryKey);
            }

            FieldParameter[] parameters =
            [
                new FieldParameter("@LogStatus", DbType.String,"INFO"),
                new FieldParameter("@LogInfo", DbType.String, Convert.ToString(objectMessage)),
                new FieldParameter("@LogDescription", DbType.String, Convert.ToString(objectMessage)),
                new FieldParameter("@LogCreatedDate", DbType.DateTime, DateTime.Now)
            ];

            nawaDao.ExecuteNonQuery(query, parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine(Convert.ToString(ex));

            throw;
        }
    }

    public void LogError(object message, Exception exception)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(exception);

            const string queryKey = "OneLoan.HelperBLL.LogInfo";
            var query = queryBuilder.GetQuery(queryKey);

            if (string.IsNullOrWhiteSpace(query))
            {
                throw new NullReferenceException("query OneLoan.HelperBLL.LogInfo not found");
            }

            FieldParameter[] parameters =
            [
                new FieldParameter("@LogStatus", DbType.String, "ERROR"),
                new FieldParameter("@LogInfo", DbType.String,Convert.ToString(message)),
                new FieldParameter("@LogDescription", DbType.String, Convert.ToString(exception)),
                new FieldParameter("@LogCreatedDate", DbType.String,DateTime.Now)
            ];

            nawaDao.ExecuteNonQuery(query, parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine(Convert.ToString(ex));

            throw;
        }
    }


    public JObject ToJObject(ConcurrentDictionary<string, object> input)
    {
        ArgumentNullException.ThrowIfNull(input);
        JObject jObject = new();

        foreach (var kvp in input)
        {
            var property = new JProperty(kvp.Key, kvp.Value);
            jObject.Add(property);
        }

        return jObject;
    }

    public long InsertLogTransactionApi(string tag, string type, string level, string endPoint, string apiMethod, string message, object requestBody)
    {

        var value = InsertLogTransactionApi(tag, type, level, endPoint, apiMethod, message, requestBody, "", null, "", "");
        return value;
    }

    public long InsertLogTransactionApi(string tag, string type, string level, string endPoint, string apiMethod, string message, object requestBody, string api_request_id, object? request_header, string requestIdApp, string contractNoApp)
    {
        if (string.IsNullOrWhiteSpace(tag))
            throw new ArgumentNullException(nameof(tag));

        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentNullException(nameof(type));

        if (string.IsNullOrWhiteSpace(level))
            throw new ArgumentNullException(nameof(level));

        if (string.IsNullOrWhiteSpace(endPoint))
            throw new ArgumentNullException(nameof(endPoint));

        if (string.IsNullOrWhiteSpace(apiMethod))
            throw new ArgumentNullException(nameof(apiMethod));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentNullException(nameof(message));

        ArgumentNullException.ThrowIfNull(requestBody);

        const string queryKey = "OneLoan.HelperBLL.InsertLogTransactionAPI";
        var query = queryBuilder.GetQuery("OneLoan.HelperBLL.InsertLogTransactionAPI");

        if (string.IsNullOrWhiteSpace(query))
        {
            throw new QueryNotFoundHelperBllException(queryKey);
        }

        FieldParameter[] parameters =
        [
            new FieldParameter("@timestamp_request", DbType.Date, DateTime.Now),
            new FieldParameter("@tag", DbType.String, tag),
            new FieldParameter("@type", DbType.String,type),
            new FieldParameter("@host", DbType.String,MachineName),
            new FieldParameter("@level", DbType.String, level),
            new FieldParameter("@message", DbType.String, $"{endPoint}||{apiMethod}||{message}"),
            new FieldParameter("@request_body", DbType.String, JsonConvert.SerializeObject(requestBody)),
            new FieldParameter("@api_request_id",  DbType.String, api_request_id ),
            new FieldParameter("@header",  DbType.String, (request_header == null)? "": JsonConvert.SerializeObject(request_header)),
            new FieldParameter("@app_request_id",  DbType.String, requestIdApp ?? "" ),
            new FieldParameter("@app_contract_no",  DbType.String, contractNoApp ?? "" )
        ];

        var logId = nawaDao.ExecuteScalar(query, parameters);

        if (logId == null)
        {
            throw new FailedToExecuteQueryHelperBllException($"Failed to Insert Log Transaction API {endPoint}||{apiMethod}||{message}");
        }

        var value = Convert.ToInt64(logId);

        return value;
    }

    public void UpdateLogTransactionApi(long logId, object responseBody)
    {
        if (logId < 1)
            throw new InvalidDataException($"{nameof(logId)} value is less than 1");

        ArgumentNullException.ThrowIfNull(responseBody);

        const string queryKey = "OneLoan.HelperBLL.UpdateLogTransactionAPI";
        var query = queryBuilder.GetQuery(queryKey);

        if (string.IsNullOrWhiteSpace(query))
        {
            throw new QueryNotFoundHelperBllException(queryKey);
        }

        JsonConvert.SerializeObject(responseBody);

        FieldParameter[] parameters =
        [
            new FieldParameter("@timestamp_return", DbType.Date, DateTime.Now),
            new FieldParameter("@logID", DbType.Double, logId),
            new FieldParameter("@response_body", DbType.String, JsonConvert.SerializeObject(responseBody) )
        ];

        nawaDao.ExecuteNonQuery(query, parameters);
    }
}