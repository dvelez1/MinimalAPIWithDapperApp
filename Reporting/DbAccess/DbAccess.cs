using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.DbAccess;
public class DbAccess : IDbAccess
{
    private readonly string _connectionString;

    public DbAccess(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DataTable ExecuteStoredProcedure(string storedProcedure, Dictionary<string, object> parameters)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(storedProcedure, conn) { CommandType = CommandType.StoredProcedure };
        foreach (var param in parameters)
            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

        var dt = new DataTable();
        new SqlDataAdapter(cmd).Fill(dt);
        return dt;
    }
}