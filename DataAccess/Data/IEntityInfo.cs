using DataAccess.Models;


namespace DataAccess.Data;
public interface IEntityInfo
{
    Task<IEnumerable<TableColumnInfo>> GetTableColumnInfo(string database, string tableName);
    Task<IEnumerable<StoredProcedureInfo>> GetTableStoredProcedureInfo(string database, string tableName);
    Task<IEnumerable<TableTriggerInfo>> GetTableTriggerInfo(string database, string tableName);
    Task<IEnumerable<TableConstraintInfo>> GetTableConstraintInfo(string database, string tableName);
    Task<IEnumerable<TableIndexInfo>> GetTableIndextInfo(string database, string tableName);
    Task<IEnumerable<TableViewsInfo>> GetTableViewsInfo(string database, string tableName);
    Task<IEnumerable<DatabaseTableInfo>> GetDatabaseTables(string database);
}
