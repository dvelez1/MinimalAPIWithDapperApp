using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Reporting.DbAccess;
using Reporting.Models;
using Reporting.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Colors = QuestPDF.Helpers.Colors;

namespace Reporting.Interfaces;
public class ReportExporter : IReportExporter
{
    private readonly IDbAccess _dbAccess;
    private readonly IConfiguration _config;

    public ReportExporter(IDbAccess dbAccess, IConfiguration config)
    {
        _dbAccess = dbAccess;
        _config = config;

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
    public void Export(DataTable data, string filePath, string format)
    {
        switch (format.ToLower())
        {
            case "xlsx":
                ExportToExcel(data, filePath);
                break;

            case "csv":
                ExportToCsv(data, filePath);
                break;

            case "pdf":
                ExportToPdf(data, filePath);
                break;

            default:
                throw new NotSupportedException($"Unsupported format: {format}");
        }
    }

    public void Export(ReportJob reportJob, string filePath)
    {
        var dataTables = new Dictionary<string, DataTable>();

        Dictionary<string, object> parameters = new();

        if (!string.IsNullOrWhiteSpace(reportJob.Parameters))
        {
            try
            {
                parameters = JsonConvert.DeserializeObject<Dictionary<string, object>>(reportJob.Parameters)
                             ?? new Dictionary<string, object>();
            }
            catch (Exception paramEx)
            {
                Console.WriteLine($"Job '{reportJob.JobName}' skipped: Invalid parameters JSON — {paramEx.Message}");
            }
        }


        // Main sheet
        var data = _dbAccess.ExecuteStoredProcedure(reportJob.StoredProcedure, parameters);
        if (reportJob.ExportExtension == "xlsx" && data != null && data.Rows.Count > 0)
        {
            var sheetName = string.IsNullOrWhiteSpace(reportJob.SheetName) ? "MainSheet" : reportJob.SheetName;
            dataTables[sheetName] = data;
        }

        // Additional sheets
        if (reportJob.ExportExtension == "xlsx" &&
            reportJob.ChildRecords == true &&
            reportJob.AdditionalSheets?.Count > 0)
        {
            foreach (var sheet in reportJob.AdditionalSheets)
            {
                Dictionary<string, object> childParams = new();

                if (!string.IsNullOrWhiteSpace(sheet.Parameters))
                {
                    try
                    {
                        childParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(sheet.Parameters)
                                      ?? new Dictionary<string, object>();
                    }
                    catch (Exception childEx)
                    {
                        Console.WriteLine($"Child sheet '{sheet.SheetName}' skipped: Invalid parameters JSON — {childEx.Message}");
                        continue;
                    }
                }

                var childData = _dbAccess.ExecuteStoredProcedure(sheet.StoredProcedure, childParams);
                if (childData != null && childData.Rows.Count > 0)
                {
                    var childSheetName = string.IsNullOrWhiteSpace(sheet.SheetName)
                        ? $"ChildSheet{reportJob.AdditionalSheets.IndexOf(sheet) + 1}"
                        : sheet.SheetName;

                    dataTables[childSheetName] = childData;
                }
            }
        }



        switch (reportJob.ExportExtension.ToLower())
        {
            case "xlsx":
                DynamicExcelExporter.ExportDataTablesAndSaveOrReturn(dataTables, filePath);
                break;

            case "csv":

                ExportToCsv(data, filePath);
                break;

            case "pdf":
                //ExportToPdf(data, filePath);
                break;

            default:
                throw new NotSupportedException($"Unsupported format:");
        }

    }



    // TODO: Need to be more detailed with formatting and timestamp. Code Improvement too
    private void ExportToExcel(DataTable data, string filePath, string sheetName = "report")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(data, sheetName);

        // Add timestamp below the data
        int lastRow = worksheet.LastRowUsed().RowNumber() + 2;
        worksheet.Cell(lastRow, 1).Value = $"Exported at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";

        workbook.SaveAs(filePath);
    }

    // TODO: Need to be more detailed with formatting and timestamp. Code Improvement too
    private void ExportToCsv(DataTable data, string filePath)
    {
        var lines = new List<string>();
        var columnNames = data.Columns.Cast<DataColumn>().Select(col => col.ColumnName);
        lines.Add(string.Join(",", columnNames));

        foreach (DataRow row in data.Rows)
        {
            var fields = row.ItemArray.Select(field => $"\"{field}\"");
            lines.Add(string.Join(",", fields));
        }

        // Add timestamp as a footer
        lines.Add($"\"Exported at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\"");

        File.WriteAllLines(filePath, lines);
    }

    //TODO: Pending correction
    private void ExportToPdf(DataTable data, string filePath)
    {
        var document = QuestPDF.Fluent.Document.Create(doc =>
        {
            doc.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        for (int i = 0; i < data.Columns.Count; i++)
                        {
                            columns.RelativeColumn(); // You can use ConstantColumn(width) if needed
                        }
                    });

                    // Header row
                    foreach (DataColumn column in data.Columns)
                    {
                        table.Cell().Element(CellStyle).Text(column.ColumnName).SemiBold();
                    }

                    // Data rows
                    foreach (DataRow row in data.Rows)
                    {
                        foreach (var cell in row.ItemArray)
                        {
                            table.Cell().Element(CellStyle).Text(cell?.ToString() ?? string.Empty);
                        }
                    }

                    static QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) =>
                        container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                });
            });
        });

        document.GeneratePdf(filePath);

    }


}