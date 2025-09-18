using System.Data;

namespace Reporting.DbAccess;

public interface IDbAccess
{
    /// <summary>
    /// Executes a stored procedure with parameters and returns the result as a DataTable.
    /// </summary>
    /// <param name="storedProcedure">Name of the stored procedure</param>
    /// <param name="parameters">Dictionary of parameter names and values</param>
    /// <returns>DataTable containing the result set</returns>
    DataTable ExecuteStoredProcedure(string storedProcedure, Dictionary<string, object> parameters);

}
