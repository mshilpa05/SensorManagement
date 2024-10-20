using Application.Interface;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Application.Mappings;
using Infrastructure;
using Infrastructure.Data;
using Api.Models;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

builder.Services.Configure<PlatformApiConfig>(builder.Configuration.GetSection("ExternalApiSettings"));

builder.Configuration.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "src/Api"));
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<SensorDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddHttpClient<IPlatformService, PlatformService>();

builder.Services.AddAutoMapper(typeof(SensorMappingProfile).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
