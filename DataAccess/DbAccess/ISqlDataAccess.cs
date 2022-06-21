
using Dapper;

namespace DataAccess.DbAccess;

public interface ISqlDataAccess
{
    Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Default");
    Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "Default");

    // Improvement methods
    Task<IEnumerable<T>> LoadDataWithDynamicParameters<T>(string storedProcedure, DynamicParameters parameters, string connectionId = "Default");
    Task SaveDataWithDynamicParameters<T>(string storedProcedure, DynamicParameters parameters, string connectionId = "Default");
}