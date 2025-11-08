using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Reporting.DbAccess;
using Reporting.Interfaces;
using Reporting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Services;

public class ReportJobRunner
{
    private readonly IDbAccess _dbAccess;
    private readonly IReportExporter _reportExporter;
    private readonly IConfiguration _config;
    public int MyProperty { get; set; }

    public ReportJobRunner(IDbAccess dbAccess, IReportExporter reportExporter, IConfiguration config)
    {
        _dbAccess = dbAccess;
        _reportExporter = reportExporter;
        _config = config;
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true; // Option to recognize or match database _ with PascalCase on DataModel Ex: DB: user_id -> Match or map with on Model -> UserId
    }

    public void RunAllJobs()
    {
        var jobs = LoadActiveJobsFromDb("ReportingDB");

        foreach (var job in jobs)
        {
            try
            {
                Dictionary<string, object> parameters = new();

                if (!string.IsNullOrWhiteSpace(job.Parameters))
                {
                    parameters = JsonConvert.DeserializeObject<Dictionary<string, object>>(job.Parameters)
                                 ?? new Dictionary<string, object>();
                }

                var data = _dbAccess.ExecuteStoredProcedure(job.StoredProcedure, parameters);

                var fullPath = Path.Combine(job.ExportPath, job.ExportFileName);
                _reportExporter.Export(data, fullPath, job.ExportExtension);

                Console.WriteLine($"Job '{job.JobName}' completed: {fullPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Job '{job.JobName}' failed: {ex.Message}");
                // Optional: log to DB or file
            }
        }

    }

    private List<ReportJob> LoadActiveJobsFromDb(string connectionDb = "Default")
    {
        using var conn = new SqlConnection(_config.GetConnectionString(connectionDb));

        // Query active jobs
        string jobSql = @"SELECT * FROM ReportJobs WHERE IsActive = 1";
        var jobs = conn.Query<ReportJob>(jobSql).ToList();

        // Query active additional sheets
        string sheetSql = @"SELECT * FROM report_jobs_aditional_sheets WHERE IsActive = 1";
        var sheets = conn.Query<ReportJobsAdditionalSheet>(sheetSql).ToList();

        // Map additional sheets to their parent jobs
        var groupedSheets = sheets
            .Where(s => s.JobId != 0)
            .GroupBy(s => s.JobId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var job in jobs)
        {
            if (groupedSheets.TryGetValue(job.JobId, out var additionalSheets))
            {
                job.AdditionalSheets = additionalSheets;
            }
        }

        return jobs;
    }

}