using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIDemo.Utilities;
using Swashbuckle.AspNetCore.Annotations;

namespace MinimalAPIDemo.Services;

public static class DatabaseAdminTool
{
    public static void DatabaseAdminToolsApi(this WebApplication app)
    {
        app.MapGet("/GetDatabaseSizeAndGrowth", GetDatabaseSizeAndGrowth);
        app.MapGet("/GetDatabaseIndexFragmentatio", GetDatabaseIndexFragmentatio);
        app.MapGet("/GetQueryPerformance", GetQueryPerformance);
        app.MapGet("/GetBackupStatus", GetBackupStatus);
        app.MapGet("/GetErrorLogsAndAlerts", GetErrorLogsAndAlerts);
        app.MapGet("/GetJobMonitoring", GetJobMonitoring);

    }

    private static async Task<IResult> GetDatabaseSizeAndGrowth()
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

    private static async Task<IResult> GetDatabaseIndexFragmentatio()
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

    private static async Task<IResult> GetQueryPerformance()
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

    private static async Task<IResult> GetBackupStatus()
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

    private static async Task<IResult> GetErrorLogsAndAlerts()
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

    private static async Task<IResult> GetJobMonitoring()
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


