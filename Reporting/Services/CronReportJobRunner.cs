using Cronos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Reporting.DbAccess;
using Reporting.Interfaces;
using Reporting.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Reporting.Services;

public class CronReportJobRunner : BackgroundService
{
    private readonly IDbAccess _dbAccess;
    private readonly IReportExporter _reportExporter;
    private readonly IConfiguration _config;
    private readonly Dictionary<int, DateTimeOffset> _lastRun = new();

    public CronReportJobRunner(IDbAccess dbAccess, IReportExporter reportExporter, IConfiguration config)
    {
        _dbAccess = dbAccess;
        _reportExporter = reportExporter;
        _config = config;
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        Log("CronReportJobRunner starting...");
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTimeOffset.UtcNow;
            var jobs = LoadActiveJobsFromDb("ReportingDB");

            foreach (var job in jobs)
            {
                await TryExecuteJobAsync(job, now, stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Log("CronReportJobRunner stopping...");
        return base.StopAsync(cancellationToken);
    }

    private async Task TryExecuteJobAsync(ReportJob job, DateTimeOffset now, CancellationToken token)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(job.CronExpression))
            {
                Log($"Job '{job.JobName}' skipped: Missing CRON expression.");
                return;
            }

            if (!CronExpression.TryParse(job.CronExpression.Trim(), CronFormat.Standard, out var cron))
            {
                Log($"Job '{job.JobName}' skipped: Invalid CRON '{job.CronExpression}'.");
                return;
            }

            var next = cron.GetNextOccurrence(now.AddMinutes(-1), TimeZoneInfo.Local);

            if (next.HasValue && Math.Abs((now - next.Value).TotalSeconds) < 60)
            {
                if (_lastRun.TryGetValue(job.JobId, out var lastRunTime) && lastRunTime == next.Value)
                {
                    Log($"Job '{job.JobName}' already executed at {next.Value:HH:mm}.");
                    return;
                }

                _lastRun[job.JobId] = next.Value;

                var fullPath = Path.Combine(job.ExportPath, $"{job.ExportFileName}{DateTime.Now:yyyyMMdd_HHmmss}.{job.ExportExtension}");
                _reportExporter.Export(job, fullPath);
                Log($"Job '{job.JobName}' (ID: {job.JobId}) completed.");
            }
            else
            {
                Log($"Job '{job.JobName}' not scheduled to run at {now:HH:mm}.");
            }
        }
        catch (Exception ex)
        {
            Log($"Job '{job.JobName}' failed: {ex.Message}");
        }
    }

    private List<ReportJob> LoadActiveJobsFromDb(string connectionDb = "Default")
    {
        using var conn = new SqlConnection(_config.GetConnectionString(connectionDb));

        var jobs = conn.Query<ReportJob>("SELECT * FROM ReportJobs WHERE IsActive = 1").ToList();
        var sheets = conn.Query<ReportJobsAdditionalSheet>("SELECT * FROM report_jobs_aditional_sheets WHERE IsActive = 1").ToList();

        var groupedSheets = sheets
            .Where(s => s.JobId != 0)
            .GroupBy(s => s.JobId)
            .ToDictionary(g => g.Key, g => g.OrderBy(s => s.SortOrder).ToList());

        foreach (var job in jobs)
        {
            if (groupedSheets.TryGetValue(job.JobId, out var additionalSheets))
            {
                job.AdditionalSheets = additionalSheets;
            }
        }

        return jobs;
    }

    private void Log(string message)
    {
        Console.WriteLine($"[{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss}] {message}");
    }
}
