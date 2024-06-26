﻿using BIO_API_DATA.API_Client;
using BIO_API_DATA.API_Client.ApplicationLogic;
using BIO_API_DATA.API_Client.Database;
using BIO_API_DATA.API_Client.Interfaces;
using BIO_API_DATA.API_Client.Interfaces.Database;
using BIO_API_DATA.Data;
using BIO_API_DATA.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;
using Serilog;
using static BIO_API_DATA.API_Client.TopLevelCustomersClientList;


var builder = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); // optional for development

var log = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Build())
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.CreateLogger();

log.Information("Starting the application");

var configuration = builder.Build();

var host = Host.CreateDefaultBuilder()
	.ConfigureServices((context, services) =>
	{
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IGasMeteringPointRepository, GasMeteringPointRepository>();
        services.AddScoped<IGasMeterCustomerRelationRepository, GasMeterCustomerRelationRepository>();
        services.AddScoped<IObservationRepository, ObservationRepository>();
        services.AddScoped<IGasMeterMeasurementRepository, GasMeterMeasurementRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITimeSeriesLogic, TimeSeriesLogic>();
        services.AddScoped<ITimeSeriesClient, TimeSeriesClient>();
        services.AddScoped<IRestClient, RestClient>();
		services.AddSingleton<ITopLevelCustomersClientList, TopLevelCustomersClientList>();
		services.AddScoped<IGasMeteringPointCustomerClientList, GasMeteringPointCustomerClientList>();
        services.AddScoped<IRestClientFactory, MyRestClientFactory>();      
        services.AddScoped<ILogger>(provider => Log.Logger);
		services.AddScoped<IGasMeteringPointCustomerClient, GasMeteringPointCustomerClient>();
		services.AddDbContext<BioDataContext>(options =>
		{
			options.UseSqlServer(configuration.GetConnectionString("Default"));
		});


	}).UseSerilog()
	.Build();

var svc = ActivatorUtilities.CreateInstance<Start>(host.Services);

await svc.Run();