using DataAccess.Models;

namespace DataAccess.Data;
public interface IStoredProceduresInfo
{
    Task<IEnumerable<StoredProcedureRelatedObject>> GetStoredProcedureInfo(string database, string storedProcedureName);

}
