using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Models;
public class ReportExportExtension
{
    public int Id { get; set; }

    public string? ExtensionDescription { get; set; }

    public DateTime? CreateDate { get; set; }

}
