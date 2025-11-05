using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Models;

public class ReportJob
{
    public int JobId { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string StoredProcedure { get; set; } = string.Empty;
    public string Parameters { get; set; } = string.Empty; // JSON string
    public string ExportExtension { get; set; } // e.g., "xlsx", "csv", "pdf"
    public string ExportFileName { get; set; } = string.Empty;
    public string ExportPath { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool ChildRecords { get; set; }
    public string SheetName { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public string CronExpression { get; set; } = string.Empty;
    public string CronDescription { get; set; } = string.Empty;

}