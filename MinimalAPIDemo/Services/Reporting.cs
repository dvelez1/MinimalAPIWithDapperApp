namespace MinimalAPIDemo.Services;

public static class Reporting
{

    public static void ReportingEndPoints(this WebApplication app)
    {
        app.MapPost("/run-jobs", (ReportJobRunner runner) =>
        {
            runner.RunAllJobs();
            return Results.Ok("✅ All report jobs executed.");
        }).WithName("RunAllActiveReports")
        .WithTags("Reporting");
    }
}
