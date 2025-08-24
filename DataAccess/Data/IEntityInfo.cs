using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;


namespace DataAccess.Data;
public interface IEntityInfo
{
    Task<IEnumerable<TableColumnInfo>> GetTableColumnInfo(string database, string tableName);
    Task<IEnumerable<StoredProcedureInfo>> GetTableStoredProcedureInfo(string database, string tableName);
    Task<IEnumerable<TableTriggerInfo>> GetTableTriggerInfo(string database, string tableName);
    Task<IEnumerable<TableConstraintInfo>> GetTableConstraintInfo(string database, string tableName);
}
