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
        app.MapGet("/ExportToExcelTableDocumentation", ExportToExcelTableDocumentation);
        app.MapPost("/ServiceToDocumentDatabaseOnLocalPath", ServiceToDocumentDatabaseOnLocalPath);
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
            var excelBytes = DynamicExcelExporter.ExportListToExcel(list,"Table Properties");

            return Results.File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{database}_{tableName}.xlsx"
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
            var excelBytes = DynamicExcelExporter.ExportListToExcel(list,"Stored Procedures");

            return Results.File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{database}_{tableName}.xlsx"
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
            var excelBytes = DynamicExcelExporter.ExportListToExcel(list,"Triggers");

            return Results.File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{database}_{tableName}.xlsx"
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
                $"{database}_{tableName}.xlsx"
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
            var tableProperties =  data.GetTableColumnInfo(database, tableName);
            var storedProcedure =  data.GetTableStoredProcedureInfo(database, tableName);
            var triggers =  data.GetTableTriggerInfo(database, tableName);
            var tableContraints =  data.GetTableConstraintInfo(database, tableName);

            await Task.WhenAll(tableProperties, storedProcedure, triggers, tableContraints);

            var exportData = new Dictionary<string, IEnumerable<object>>
            {
                { $"Table - {tableName}", tableProperties.Result.Cast<object>() },
                { "Stored Procedures", storedProcedure.Result.Cast<object>() },
                { "Triggers", triggers.Result.Cast<object>() },
                { "Constraints", tableContraints.Result.Cast<object>() }
            };

            var excelBytes = DynamicExcelExporter.ExportMultipleListsToExcel(exportData);

            return Results.File(
              excelBytes,
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
              $"{database}_{tableName}.xlsx");


        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> ServiceToDocumentDatabaseOnLocalPath(string database, string tableName, IEntityInfo data)
    {
        try
        { 
            return Results.Ok("Under Construction");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }


}
