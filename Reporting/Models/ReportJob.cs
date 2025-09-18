using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Models;

public class ReportJob
{
    public int JobId { get; set; }
    public string JobName { get; set; }
    public string StoredProcedure { get; set; }
    public string Parameters { get; set; } // JSON string
    public string ExportExtension { get; set; } // e.g., "xlsx", "csv", "pdf"
    public string ExportFileName { get; set; }
    public string ExportPath { get; set; }
    public bool IsActive { get; set; }
}