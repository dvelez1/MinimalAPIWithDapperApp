namespace MinimalAPIDemo.Services;
using MinimalAPIDemo.Utilities;

public static class StoredProcedureInfoService
{
    public static void MapStoredProcedureInfoEndpoints(this WebApplication app)
    {
        app.MapGet("/GetStoredProcedureInfo/{database}/{storedProcedureName}", async (string database, string storedProcedureName, IStoredProceduresInfo db) =>
        {
            try
            {
                var result = await db.GetStoredProcedureInfo(database, storedProcedureName);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }

        })
        .WithName("GetStoredProcedureInfo")
        .WithTags("Stored Procedure Info");

        app.MapGet("/ExportToExcelGetStoredProcedureInfo/{database}/{storedProcedureName}", async (string database, string storedProcedureName, IStoredProceduresInfo db) =>
        {
            try
            {
                var list = await db.GetStoredProcedureInfo(database, storedProcedureName);
                var excelBytes = DynamicExcelExporter.ExportListToExcel(list, "Tables");

                return Results.File(
                    excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"{database}_{storedProcedureName}_{DateTime.Now.ToString("yyyyMMdd_HHmm")}.xlsx"
                );
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }

        })
        .WithName("ExportToExcelGetStoredProcedureInfo")
        .WithTags("Stored Procedure Info");
    }

}
