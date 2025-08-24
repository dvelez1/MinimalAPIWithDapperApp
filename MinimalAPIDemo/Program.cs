using DataAccess.DbAccess;
using MinimalAPIDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<IUserData, UserData>();
builder.Services.AddScoped<IEntityInfo, EntityInfo>();


builder.Services.AddSwaggerGen(c => c.EnableAnnotations()); // Added for data annotation

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
*/
app.EntityInfoApi();
app.DatabaseAdminToolsApi();
app.ApplicationMonitoringToolApi();

app.Run();
