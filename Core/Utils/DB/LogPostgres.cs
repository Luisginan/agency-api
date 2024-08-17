using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Core.Config;
using Core.Utils.Security;
using Microsoft.Extensions.Options;
using NawaDataDAL.Common;

namespace Core.Utils.DB;
[ExcludeFromCodeCoverage]
public class LogPostgres : ILogDb
{
    public LogPostgres(IVault vault, IOptions<DatabaseConfig> option)
    {
        var databaseConfig = vault.RevealSecret(option.Value);
         
        NawaDAO.Config(databaseConfig.Type,
            databaseConfig.Server,
            databaseConfig.Database,
            databaseConfig.User,
            databaseConfig.Password,
            databaseConfig.Port,
            databaseConfig.CommandTimeout,
            databaseConfig.PoolSize,
            databaseConfig.ConnectTimeout
        );
    }

    public int ExecuteNonQuery(string cmdText, IEnumerable<FieldParameter> fieldParameter)
    {
        var parameters = fieldParameter.Select(x => x.GetQueryParameter()).ToArray();
        return NawaDAO.ExecuteNonQuery(cmdText, parameters);
    }

    public T? ExecuteRow<T>(string cmdText, IEnumerable<FieldParameter> fieldParameter) where T : class, new()
    {
        var parameters = fieldParameter.Select(x => x.GetQueryParameter()).ToArray();
        return Execute<T>(cmdText, parameters);
    }
    
    private static T? Execute<T>(string cmdText, QueryParameter[] parameter)
        where T : class, new()
    {
        var dataRow = NawaDAO.ExecuteRow(cmdText, parameter);
        return ConvertDataRow<T>(dataRow);
    }
    
    private static T? ConvertDataRow<T>(DataRow? row) where T : class, new()
    {
        if (row == null) return default;
        
        T obj = new();

        foreach (DataColumn column in row.Table.Columns)
        {
            var propertyName = column.ColumnName;

            var propertyInfo = GetPropertyByAttributeName<T>(propertyName);
            if (propertyInfo == null)
            {
                propertyInfo = typeof(T).GetProperty(propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            }


            if (propertyInfo == null || row[column] == DBNull.Value) continue;
            
            var value = Convert.ChangeType(row[column], propertyInfo.PropertyType);
            propertyInfo.SetValue(obj, value);
        }

        return obj;
    }
    
    private static PropertyInfo? GetPropertyByAttributeName<T>(string name) where T : class
    {
        var type = typeof(T);
        var properties = type.GetProperties();
        PropertyInfo? result = null;
        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<FieldAttribute>();

            if (attribute == null) continue;
            
            if (attribute.Name != name) continue;
                
            result = property;
            break;
        }

        return result;
    }
}