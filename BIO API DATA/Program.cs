using BIO_API_DATA.API_Client;
using BIO_API_DATA.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;
using Serilog;
using static BIO_API_DATA.API_Client.TopLevelCustomersClient;


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
		services.AddTransient<IRestClientFactory, MyRestClientFactory>();
		services.AddTransient<IRestClient, RestClient>();
		services.AddTransient<ITopLevelCustomersClient, TopLevelCustomersClient>();
		services.AddTransient<IGasMeteringPointClient, GasMeteringPointClient>();
		services.AddTransient<ILogger>(provider => Log.Logger);


	}).UseSerilog()
	.Build();

var svc = ActivatorUtilities.CreateInstance<GasMeteringPointClient>(host.Services);

svc.GetGasmetringPointCustomerassociation();