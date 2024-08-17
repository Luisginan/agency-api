namespace Core.Utils.DB;

public interface INawaDaoRepository
{
    int Insert<T>(T model) where T : class, new();
    long Insert2<T>(T model) where T : class, new();
    int Update<T>(T model, int key) where T : class, new();
    int Delete<T>(int key) where T : class, new();
    T? Get<T>(int key) where T : class, new();
    int ExecuteNonQuery(string cmdText, IEnumerable<FieldParameter> fieldParameter);

    object ExecuteScalar(string cmdText, IEnumerable<FieldParameter> fieldParameter);

    T? ExecuteRow<T>(string cmdText, IEnumerable<FieldParameter> fieldParameter) where T : class, new();

    List<T> ExecuteTable<T>(string cmdText, IEnumerable<FieldParameter> fieldParameter) where T : class, new();
    
    IConnection GetConnection();
    
}