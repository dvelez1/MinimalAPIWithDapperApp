using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Interfaces;
public interface IReportExporter
{
    /// <summary>
    /// Exports a DataTable to a file in the specified format.
    /// </summary>
    /// <param name="data">The data to export</param>
    /// <param name="filePath">Full path including filename and extension</param>
    /// <param name="format">Export format: xlsx, csv, pdf</param>
    void Export(DataTable data, string filePath, string format);
}