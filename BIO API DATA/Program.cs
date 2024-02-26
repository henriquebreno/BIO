using BIO_API_DATA.API_Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using static BIO_API_DATA.API_Client.TopLevelCustomersClient;

var log = new LoggerConfiguration()
	.MinimumLevel.Information() // Optional: Set minimum log level
	.WriteTo.Console() // Write logs to console
	.CreateLogger();

try
{
	Log.Information("Starting application...");

	// Your application code here
	// ...

	Log.Information("Application finished successfully.");
}
catch (Exception ex)
{
	Log.Error(ex, "An error occurred:");
}
finally
{
	Log.CloseAndFlush(); // Flush and dispose of the logger
}

await Host.CreateDefaultBuilder(args)
	.UseSerilog(log) // Use the configured logger
	.Build()
	.RunAsync();

var builder = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); // optional for development

var configuration = builder.Build();

var host = Host.CreateDefaultBuilder()
	.ConfigureServices((context, services) =>
	{
		services.AddTransient<IRestClientFactory, MyRestClientFactory>();

	}).UseSerilog()
	.Build();