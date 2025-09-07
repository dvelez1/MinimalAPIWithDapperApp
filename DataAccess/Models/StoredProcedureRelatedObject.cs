using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;
public class StoredProcedureRelatedObject
{
    public string DatabaseName { get; set; }              // Name of the database
    public string StoredProcedureName { get; set; }       // Name of the stored procedure
    public string RelatedObjectName { get; set; }         // Name of the related table/view (null for linked servers)
    public string SchemaName { get; set; }                // Schema of the related object
    public int? ObjectId { get; set; }                    // Object ID (null for linked servers)
    public string ObjectType { get; set; }                // "Table", "View", or "LinkedServer"
    public string LinkedServerName { get; set; }          // Name of the linked server (only populated for linked servers)

}
