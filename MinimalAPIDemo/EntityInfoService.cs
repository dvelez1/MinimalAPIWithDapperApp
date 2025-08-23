using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MinimalAPIDemo;

public static class EntityInfoService
{
    public static void EntityInfoApi(this WebApplication app)
    {
        app.MapGet("/GetTableColumnInformation", GetTableColumnInfo);
        app.MapGet("/GetTableStoredProcedureInfo", GetTableStoredProcedureInfo);
        app.MapGet("/GetTableTriggerInfo", GetTableTriggerInfo);
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
}
