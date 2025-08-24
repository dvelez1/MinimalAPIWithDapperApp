namespace MinimalAPIDemo.Services;

public static  class ApplicationMonitoringTool
{
    public static void ApplicationMonitoringToolApi(this WebApplication app)
    {
        app.MapGet("/ApplicationLogReconciler", ApplicationLogReconciler);
        app.MapGet("/CheckApplicationOrServicesAreUp", CheckApplicationOrServicesAreUp);
        app.MapGet("/CheckServicesStatus", CheckServicesStatus);

    }

    private static async Task<IResult> ApplicationLogReconciler()
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

    private static async Task<IResult> CheckApplicationOrServicesAreUp()
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

    private static async Task<IResult> CheckServicesStatus()
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
