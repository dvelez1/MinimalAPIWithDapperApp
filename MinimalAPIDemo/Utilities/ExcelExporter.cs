using ClosedXML.Excel;
using System.Data;
using System.Dynamic;
using System.Reflection;

namespace MinimalAPIDemo.Utilities;


public static class DynamicExcelExporter
{
    public static byte[] ExportListToExcel(IEnumerable<dynamic> items, string sheetName = "Data")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        if (!items.Any())
            return Array.Empty<byte>();

        var firstItem = items.First();
        var properties = GetPropertyNames(firstItem);

        // Header
        for (int col = 0; col < properties.Count; col++)
        {
            worksheet.Cell(1, col + 1).Value = properties[col];
        }

        // Data
        int row = 2;
        foreach (var item in items)
        {
            for (int col = 0; col < properties.Count; col++)
            {
                var value = GetPropertyValue(item, properties[col]);
                var cell = worksheet.Cell(row, col + 1);

                if (value == null)
                {
                    cell.SetValue(string.Empty); // Avoid ambiguity
                }
                else
                {
                    cell.SetValue(value);

                    // Optional formatting based on type
                    switch (value)
                    {
                        case DateTime dt:
                            cell.Style.DateFormat.Format = "yyyy-mm-dd";
                            break;
                        case TimeSpan ts:
                            cell.Style.NumberFormat.Format = @"hh\:mm\:ss";
                            break;
                        case decimal or double or float:
                            cell.Style.NumberFormat.Format = "#,##0.00";
                            break;
                    }
                }
            }
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public static byte[] ExportDataTableToExcel(DataTable table, string sheetName = "Data")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        // Header
        for (int col = 0; col < table.Columns.Count; col++)
        {
            worksheet.Cell(1, col + 1).Value = table.Columns[col].ColumnName;
        }

        // Data
        for (int row = 0; row < table.Rows.Count; row++)
        {
            for (int col = 0; col < table.Columns.Count; col++)
            {
                worksheet.Cell(row + 2, col + 1).Value = (XLCellValue)table.Rows[row][col];
            }
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    // Helpers

    private static List<string> GetPropertyNames(dynamic obj)
    {
        if (obj is ExpandoObject expando)
            return ((IDictionary<string, object>)expando).Keys.ToList();

        if (obj is IDictionary<string, object> dict)
            return dict.Keys.ToList();

        if (obj is object[] array)
        {
            var headers = new List<string>();
            for (int i = 0; i < array.Length; i++)
            {
                headers.Add($"Column{i + 1}");
            }
            return headers;
        }

        var type = obj.GetType();
        var props = (PropertyInfo[])type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        return props.Select(p => p.Name).ToList();
    }

    private static object? GetPropertyValue(dynamic obj, string propertyName)
    {
        if (obj is ExpandoObject expando)
            return ((IDictionary<string, object>)expando).TryGetValue(propertyName, out var value) ? value : null;

        if (obj is IDictionary<string, object> dict)
            return dict.TryGetValue(propertyName, out var value) ? value : null;

        var type = obj.GetType();

        if (type.IsArray)
        {
            var array = (Array)obj;
            if (int.TryParse(propertyName.Replace("Column", ""), out int index) && index > 0 && index <= array.Length)
                return array.GetValue(index - 1);
        }

        return type.GetProperty(propertyName)?.GetValue(obj);
    }
}