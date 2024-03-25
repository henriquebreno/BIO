using BIO_API_DATA.API_Client;
using BIO_API_DATA.API_Client.ApplicationLogic;
using BIO_API_DATA.API_Client.Database;
using BIO_API_DATA.API_Client.Interfaces;
using BIO_API_DATA.API_Client.Interfaces.Database;
using BIO_API_DATA.Data;
using BIO_API_DATA.WebApi.Controllers;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestSharp;
using Serilog;
using Serilog.Core;
using System.Globalization;
using System.IO;
using static BIO_API_DATA.API_Client.TopLevelCustomersClientList;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddDebug();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IGasMeteringPointRepository, GasMeteringPointRepository>();
builder.Services.AddScoped<IGasMeterCustomerRelationRepository, GasMeterCustomerRelationRepository>();
builder.Services.AddScoped<IObservationRepository, ObservationRepository>();
builder.Services.AddScoped<IGasMeterMeasurementRepository, GasMeterMeasurementRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITimeSeriesLogic, TimeSeriesLogic>();
builder.Services.AddScoped<ITimeSeriesClient, TimeSeriesClient>();
builder.Services.AddSingleton<IRestClient, RestClient>();
builder.Services.AddSingleton<ITopLevelCustomersClientList, TopLevelCustomersClientList>();
builder.Services.AddSingleton<IGasMeteringPointCustomerClientList, GasMeteringPointCustomerClientList>();
builder.Services.AddScoped<IRestClientFactory, MyRestClientFactory>();
builder.Services.AddScoped<IGasMeteringPointCustomerClient, GasMeteringPointCustomerClient>();
builder.Services.AddDbContext<BioDataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddHangfire(configuration => configuration
       .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
       .UseSimpleAssemblyNameTypeSerializer()
       .UseRecommendedSerializerSettings()
       .UseMemoryStorage());



var app = builder.Build();
var culture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
app.UseHangfireServer();

var jobScheduleHours = builder.Configuration.GetValue<int>("ScheduledJobConfig:IntervalInHours");
var jobSchedule = $"0 0 */{jobScheduleHours} * *";
RecurringJob.AddOrUpdate<TopLevelCustomerController>("ImportTopCustomerJob",
       controller => controller.ImportTopCustomerJob(), jobSchedule);

RecurringJob.Trigger("ImportTopCustomerJob");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHangfireDashboard();
app.Run();



