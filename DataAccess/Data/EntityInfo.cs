using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DbAccess;
using DataAccess.Models;
using DataAccess.Utilities;

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

}