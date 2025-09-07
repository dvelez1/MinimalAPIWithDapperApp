using DataAccess.DbAccess;
using DataAccess.Models;

namespace DataAccess.Data;
public class EntityInfo : IEntityInfo
{
    private readonly ISqlDataAccess _db;

    public EntityInfo(ISqlDataAccess db)
    {
        _db = db;
    }

    Task<IEnumerable<TableColumnInfo>> IEntityInfo.GetTableColumnInfo(string database, string tableName) =>
        _db.LoadData<TableColumnInfo, dynamic>("dbo.sp_get_table_structure", new { database, tableName }, "SchoolDB");

    Task<IEnumerable<StoredProcedureInfo>> IEntityInfo.GetTableStoredProcedureInfo(string database, string tableName) =>
        _db.LoadData<StoredProcedureInfo, dynamic>("dbo.sp_get_related_stored_procedures", new { database, tableName }, "SchoolDB");

    Task<IEnumerable<TableTriggerInfo>> IEntityInfo.GetTableTriggerInfo(string database, string tableName) =>
        _db.LoadData<TableTriggerInfo, dynamic>("dbo.sp_get_related_triggers", new { database, tableName }, "SchoolDB");

    Task<IEnumerable<TableConstraintInfo>> IEntityInfo.GetTableConstraintInfo(string database, string tableName) =>
    _db.LoadData<TableConstraintInfo, dynamic>("dbo.sp_get_table_constraints", new { database, tableName }, "SchoolDB");

    Task<IEnumerable<TableIndexInfo>> IEntityInfo.GetTableIndextInfo(string database, string tableName) =>
        _db.LoadData<TableIndexInfo, dynamic>("dbo.sp_get_table_indexes", new { database, tableName }, "SchoolDB");

    Task<IEnumerable<DatabaseTableInfo>> IEntityInfo.GetDatabaseTables(string database, string tableName) =>
        _db.LoadData<DatabaseTableInfo, dynamic>("dbo.sp_get_database_tables", new { database, tableName }, "SchoolDB");
}