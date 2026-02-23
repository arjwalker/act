using Microsoft.Data.SqlClient;

namespace ActViewer.Data;

public sealed class ActDb
{
    private readonly string _cs;

    public ActDb(IConfiguration config)
    {
        _cs = config.GetConnectionString("ActDb")
              ?? throw new InvalidOperationException("Missing ConnectionStrings:ActDb");
    }

    public SqlConnection OpenConnection()
    {
        var conn = new SqlConnection(_cs);
        conn.Open();
        return conn;
    }
}
