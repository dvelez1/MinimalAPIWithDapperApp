namespace DataAccess.Models;
public class TableConstraintInfo
{
    public string Database { get; set; }              // Name of the database
    public string Table { get; set; }                 // Name of the table
    public string ConstraintName { get; set; }        // Name of the constraint
    public string ConstraintType { get; set; }        // Type: PRIMARY_KEY, FOREIGN_KEY, UNIQUE, CHECK
    public string ColumnName { get; set; }            // Column involved in the constraint
    public int? ReferencedTableID { get; set; }       // Object ID of referenced table (nullable)
    public string ReferencedTable { get; set; }       // Name of referenced table (nullable)
    public string ReferencedColumn { get; set; }      // Name of referenced column (nullable)
}
