namespace Core.Utils.DB;

public interface ILogDb
{
    int ExecuteNonQuery(string cmdText, IEnumerable<FieldParameter> fieldParameter);
    T? ExecuteRow<T>(string cmdText, IEnumerable<FieldParameter> fieldParameter) where T : class, new();
}