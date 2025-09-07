using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;
public class DatabaseTableInfo
{
    public string DatabaseName { get; set; }   // Name of the database
    public string SchemaName { get; set; }     // Schema (e.g., dbo)
    public string TableName { get; set; }      // Table name

}
