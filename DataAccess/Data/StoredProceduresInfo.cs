using DataAccess.DbAccess;
using DataAccess.Models;

namespace DataAccess.Data;

public class StoredProceduresInfo:IStoredProceduresInfo
{
    private readonly ISqlDataAccess _db;
    public StoredProceduresInfo(ISqlDataAccess db)
    {
        _db = db;
    }

    Task<IEnumerable<StoredProcedureRelatedObject>> IStoredProceduresInfo.GetStoredProcedureInfo(string database, string storedProcedureName) =>
        _db.LoadData<StoredProcedureRelatedObject, dynamic>("dbo.sp_get_stored_procedures_related_objects", new { database, storedProcedureName }, "SchoolDB");
}
