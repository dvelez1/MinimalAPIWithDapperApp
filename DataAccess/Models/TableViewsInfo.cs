using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;
public class TableViewsInfo
{
    public string DatabaseName { get; set; }           // Source database
    public string ViewName { get; set; }               // Name of the view
    public string SchemaName { get; set; }             // Schema of the view
    public int ViewObjectId { get; set; }              // Object ID of the view
    public string ReferencedTable { get; set; }        // Table name being referenced
    public string ReferencedSchema { get; set; }       // Schema of the referenced table
    public int TableObjectId { get; set; }             // Object ID of the referenced table

}
