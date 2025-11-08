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

    public CronReportJobRunner(IDbAccess dbAccess, IReportExporter reportExporter, IConfiguration config)
    {
        _dbAccess = dbAccess;
        _reportExporter = reportExporter;
        _config = config;
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true; // Option to recognize or match database _ with PascalCase on DataModel Ex: DB: user_id -> Match or map with on Model -> UserId
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTimeOffset.Now;
            var jobs = LoadActiveJobsFromDb("ReportingDB");

            foreach (var job in jobs)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(job.CronExpression))
                    {
                        Console.WriteLine($"Job '{job.JobName}' skipped: Missing CRON expression.");
                        continue;
                    }

                    CronExpression cron;
                    try
                    {
                        cron = CronExpression.Parse(job.CronExpression.Trim(), CronFormat.Standard);
                    }
                    catch (Exception parseEx)
                    {
                        Console.WriteLine($"Job '{job.JobName}' skipped: Invalid CRON '{job.CronExpression}' — {parseEx.Message}");
                        continue;
                    }

                    var next = cron.GetNextOccurrence(now.AddMinutes(-1), TimeZoneInfo.Local);

                    if (next.HasValue && Math.Abs((now - next.Value).TotalSeconds) < 60)
                    {
                        Dictionary<string, object> parameters = new();

                        if (!string.IsNullOrWhiteSpace(job.Parameters))
                        {
                            try
                            {
                                parameters = JsonConvert.DeserializeObject<Dictionary<string, object>>(job.Parameters)
                                             ?? new Dictionary<string, object>();
                            }
                            catch (Exception paramEx)
                            {
                                Console.WriteLine($"Job '{job.JobName}' skipped: Invalid parameters JSON — {paramEx.Message}");
                                continue;
                            }
                        }

                        var data = _dbAccess.ExecuteStoredProcedure(job.StoredProcedure, parameters);
                        var fullPath = Path.Combine(job.ExportPath, $"{job.ExportFileName}{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.{job.ExportExtension}");
                        _reportExporter.Export(data, fullPath, job.ExportExtension);

                        Console.WriteLine($"Job '{job.JobName}' completed: {fullPath}");
                    }
                    else
                    {
                        Console.WriteLine($"Job '{job.JobName}' not scheduled to run at {now:HH:mm}.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Job '{job.JobName}' failed: {ex.Message}");
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private List<ReportJob> LoadActiveJobsFromDb(string connectionDb = "Default")
    {
        // TODO: Create the method 
        // return _dbAccess.GetActiveReportJobs(dbName);

        using var conn = new SqlConnection(_config.GetConnectionString(connectionDb));
        string sql = @"SELECT * FROM ReportJobs WHERE IsActive = 1";
        return conn.Query<ReportJob>(sql).ToList();

    }
}
