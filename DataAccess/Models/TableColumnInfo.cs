using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;
public class TableColumnInfo
{
    public string ColumnName { get; set; }               // COLUMN_NAME
    public string DataType { get; set; }                 // DATA_TYPE
    public int? CharacterMaximumLength { get; set; }     // CHARACTER_MAXIMUM_LENGTH
    public string IsNullable { get; set; }               // IS_NULLABLE ("YES"/"NO")
}