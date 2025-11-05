using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Models;
public class ReportJobsAdditionalSheet
{
    public int Id { get; set; }

    public int JobId { get; set; }

    public string? StoredProcedure { get; set; }

    public string? Parameters { get; set; }

    public bool? IsActive { get; set; }

    public string? SheetName { get; set; }

    public int? SortOrder { get; set; }

}
