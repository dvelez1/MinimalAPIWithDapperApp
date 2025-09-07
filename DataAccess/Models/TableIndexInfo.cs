using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;
public class TableIndexInfo
{
    public string DatabaseName { get; set; }         // Source database
    public string TableName { get; set; }            // Table name
    public string IndexName { get; set; }            // Index name
    public int IndexId { get; set; }                 // Internal index ID
    public string IndexType { get; set; }            // Clustered, Nonclustered, etc.
    public bool IsUnique { get; set; }               // True if index is unique
    public bool IsPrimaryKey { get; set; }           // True if index is a PK
    public string ColumnName { get; set; }           // Column included in index
    public int KeyOrdinal { get; set; }              // Position in index key
    public bool IsDescending { get; set; }           // True if descending sort

}
