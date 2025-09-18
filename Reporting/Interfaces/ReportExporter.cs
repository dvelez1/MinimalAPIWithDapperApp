using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace Reporting.Interfaces;
public class ReportExporter : IReportExporter
{
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

    private void ExportToExcel(DataTable data, string filePath)
    {
        using var workbook = new XLWorkbook();
        workbook.Worksheets.Add(data, "Report");
        workbook.SaveAs(filePath);
    }

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

        File.WriteAllLines(filePath, lines);
    }


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