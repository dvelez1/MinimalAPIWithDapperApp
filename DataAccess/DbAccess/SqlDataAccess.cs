using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DataAccess.DbAccess;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

        return await connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

        await connection.ExecuteAsync(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);
    }

    // Improvement methods with DynamicParameters
    public async Task<IEnumerable<T>> LoadDataWithDynamicParameters<T>(string storedProcedure, DynamicParameters parameters, string connectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

        // If you need a return Value (Example: Count or Rows or some error or required information
        //parameters.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
        var data = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        // Example is a return value is required
        //var returnValue = parameters.Get<int>("@ReturnVal");

        // Additonal note: We can use another return value with direction: ParameterDirection.Output (Same implementation as ParameterDirection.ReturnValue

        return data;
    }

    public async Task SaveDataWithDynamicParameters<T>(string storedProcedure, DynamicParameters parameters, string connectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        //parameters.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue); // Return identity or RowCount as flag the everithing was well

        await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        //return  returnValue = parameters.Get<int>("@ReturnVal");
    }



}
