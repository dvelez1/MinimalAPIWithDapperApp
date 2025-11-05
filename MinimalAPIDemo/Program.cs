using DataAccess.DbAccess;
using MinimalAPIDemo.BackgroungServices;
using MinimalAPIDemo.Services;
using Reporting.DbAccess;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<IUserData, UserData>();
builder.Services.AddScoped<IEntityInfo, EntityInfo>();
builder.Services.AddScoped<IStoredProceduresInfo, StoredProceduresInfo>();


// Register Reporting Related Services
builder.Services.AddSingleton<IDbAccess>(sp =>
    new DbAccess(builder.Configuration.GetConnectionString("SchoolDB")));

builder.Services.AddSingleton<IReportExporter, ReportExporter>();
builder.Services.AddSingleton<ReportJobRunner>();


builder.Services.AddSwaggerGen(c => c.EnableAnnotations()); // Added for data annotation


// Register the background service
builder.Services.AddHostedService<MinuteJobService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/*
app.ConfigureApi();
app.ConfigurepProductApi();
app.DatabaseAdminToolsApi();
app.ApplicationMonitoringToolApi();
*/
app.EntityInfoApi();
app.MapStoredProcedureInfoEndpoints();
app.ReportingEndPoints();

app.Run();
