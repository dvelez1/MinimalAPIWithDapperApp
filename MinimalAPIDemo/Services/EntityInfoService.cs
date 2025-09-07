using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIDemo.Utilities;
using Swashbuckle.AspNetCore.Annotations;

namespace MinimalAPIDemo.Services;

public static class EntityInfoService
{
    public static void EntityInfoApi(this WebApplication app)
    {
        app.MapGet("/GetTableColumnInformation", GetTableColumnInfo);
        app.MapGet("/GetTableStoredProcedureInfo", GetTableStoredProcedureInfo);
        app.MapGet("/GetTableTriggerInfo", GetTableTriggerInfo);
        app.MapGet("/ExportToExcelTableColumnInfo", ExportToExcelTableColumnInfo);
        app.MapGet("/ExportToExcelStoredProcedureInfo", ExportToExcelStoredProcedureInfo);
        app.MapGet("/ExportToExcelTableTriggerInfo", ExportToExcelTableTriggerInfo);
        app.MapGet("/ExportToExcelGetTableConstraintInfo", ExportToExcelGetTableConstraintInfo);
        app.MapGet("/ExportToExcelGetTableIndextInfo", ExportToExcelGetTableIndextInfo);
        app.MapGet("/ExportToExcelGetTableViewsInfo", ExportToExcelGetTableViewsInfo);
        app.MapGet("/ExportToExcelTableDocumentation", ExportToExcelTableDocumentation);
        app.MapPost("/ExportTableDocumentationToDirectory", ExportTableDocumentationToDirectory);
    }

    private static async Task<IResult> GetTableColumnInfo(string database, string tableName, IEntityInfo data)
    {
        try
        {
            return Results.Ok(await data.GetTableColumnInfo(database, tableName));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetTableStoredProcedureInfo(string database, string tableName, IEntityInfo data)
    {
        try
        {
            return Results.Ok(await data.GetTableStoredProcedureInfo(database, tableName));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetTableTriggerInfo(string database, string tableName, IEntityInfo data)
    {
        try
        {
            return Results.Ok(await data.GetTableTriggerInfo(database, tableName));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> ExportToExcelTableColumnInfo(string database, string tableName, IEntityInfo data)
    {
        try
        {
            var list = await data.GetTableColumnInfo(database, tableName);
            var excelBytes = DynamicExcelExporter.ExportListToExcel(list, "Table Properties");

            return Results.File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{database}_{tableName}_{DateTime.Now.ToString("yyyyMMdd_HHmm")}.xlsx"
            );

        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> ExportToExcelStoredProcedureInfo(string database, string tableName, IEntityInfo data)
    {
        try
        {
            var list = await data.GetTableStoredProcedureInfo(database, tableName);
            var excelBytes = DynamicExcelExporter.ExportListToExcel(list, "Stored Procedures");

            return Results.File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{database}_{tableName}_{DateTime.Now.ToString("yyyyMMdd_HHmm")}.xlsx"
            );

        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> ExportToExcelTableTriggerInfo(string database, string tableName, IEntityInfo data)
    {
        try
        {
            var list = await data.GetTableTriggerInfo(database, tableName);
            var excelBytes = DynamicExcelExporter.ExportListToExcel(list, "Triggers");

            return Results.File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{database}_{tableName}_{DateTime.Now.ToString("yyyyMMdd_HHmm")}.xlsx"
            );

        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> ExportToExcelGetTableConstraintInfo(string database, string tableName, IEntityInfo data)
    {
        try
        {
            var list = await data.GetTableConstraintInfo(database, tableName);
            var excelBytes = DynamicExcelExporter.ExportListToExcel(list, "Constraint");

            return Results.File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{database}_{tableName}_{DateTime.Now.ToString("yyyyMMdd_HHmm")}.xlsx"
            );

        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> ExportToExcelGetTableIndextInfo(string database, string tableName, IEntityInfo data)
    {
        try
        {
            var list = await data.GetTableIndextInfo(database, tableName);
            var excelBytes = DynamicExcelExporter.ExportListToExcel(list, "Indexes");

            return Results.File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{database}_{tableName}_{DateTime.Now.ToString("yyyyMMdd_HHmm")}.xlsx"
            );

        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> ExportToExcelGetTableViewsInfo(string database, string tableName, IEntityInfo data)
    {
        try
        {
            var list = await data.GetTableViewsInfo(database, tableName);
            var excelBytes = DynamicExcelExporter.ExportListToExcel(list, "Views");

            return Results.File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{database}_{tableName}_{DateTime.Now.ToString("yyyyMMdd_HHmm")}.xlsx"
            );

        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> ExportToExcelTableDocumentation(string database, string tableName, IEntityInfo data)
    {
        try
        {
            var tableProperties = data.GetTableColumnInfo(database, tableName);
            var storedProcedure = data.GetTableStoredProcedureInfo(database, tableName);
            var triggers = data.GetTableTriggerInfo(database, tableName);
            var tableContraints = data.GetTableConstraintInfo(database, tableName);
            var tableIndexes = data.GetTableIndextInfo(database, tableName);
            var tableViews = data.GetTableViewsInfo(database, tableName);

            await Task.WhenAll(tableProperties, storedProcedure, triggers, tableContraints, tableIndexes);

            var exportData = new Dictionary<string, IEnumerable<object>>
            {
                { $"Table - {tableName}", tableProperties.Result.Cast<object>() },
                { "Stored Procedures", storedProcedure.Result.Cast<object>() },
                { "Triggers", triggers.Result.Cast<object>() },
                { "Indexes", tableIndexes.Result.Cast<object>() },
                { "Views", tableViews.Result.Cast<object>() },
                { "Constraints", tableContraints.Result.Cast<object>() }
            };

            var excelBytes = DynamicExcelExporter.ExportMultipleListsToExcel(exportData);

            return Results.File(
              excelBytes,
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
              $"{database}_{tableName}_{DateTime.Now.ToString("yyyyMMdd_HHmm")}.xlsx");

        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> ExportTableDocumentationToDirectory(string database, IEntityInfo data)
    {
        try
        {
            var listOfTables = await data.GetDatabaseTables(database);
            string outputDirectory = @"C:\databaseDocumentation"; //Todo: Use AppSetting or System environment variables
            Directory.CreateDirectory(outputDirectory);

            await Parallel.ForEachAsync(listOfTables, async (table, cancellationToken) =>
            {
                var tableProperties = data.GetTableColumnInfo(table.DatabaseName, table.TableName);
                var storedProcedure = data.GetTableStoredProcedureInfo(table.DatabaseName, table.TableName);
                var triggers = data.GetTableTriggerInfo(table.DatabaseName, table.TableName);
                var tableContraints = data.GetTableConstraintInfo(table.DatabaseName, table.TableName);
                var tableIndexes = data.GetTableIndextInfo(table.DatabaseName, table.TableName);
                var tableViews = data.GetTableViewsInfo(table.DatabaseName, table.TableName);

                await Task.WhenAll(tableProperties, storedProcedure, triggers, tableContraints, tableIndexes);

                var exportData = new Dictionary<string, IEnumerable<object>>
                {
                    { $"Table - {table.TableName}", tableProperties.Result.Cast<object>() },
                    { "Stored Procedures", storedProcedure.Result.Cast<object>() },
                    { "Triggers", triggers.Result.Cast<object>() },
                    { "Indexes", tableIndexes.Result.Cast<object>() },
                    { "Views", tableViews.Result.Cast<object>() },
                    { "Constraints", tableContraints.Result.Cast<object>() }
                };

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string fileName = $"{database}_{table.TableName}_{timestamp}.xlsx";

                string tableDirectory = Path.Combine(outputDirectory, table.DatabaseName);
                Directory.CreateDirectory(tableDirectory);

                string fullPath = Path.Combine(tableDirectory, fileName);

                DynamicExcelExporter.ExportMultipleListsToExcelAndSave(exportData, fullPath);
            });

            return Results.Ok("Success transaction");

        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }


}
