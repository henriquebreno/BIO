using BIO_API_DATA.API_Client;
using Moq.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BIO_API_DATA.API_Client.TopLevelCustomersClientList;
using BIO_API_DATA.Data;

namespace UnitTests
{
	public class TimeSeriesClientTestcs
	{
		private Mock<IRestClient> _restClient;
		private readonly string BaseUrl = "https://bioapi";
		private Mock<ILogger> _ILogger;
		private IConfiguration configuration;
		private readonly Mock<IGasMeteringPointCustomerClient> _gasMeteringPointCustomerClient;
		private readonly Mock<BioDataContext> _dbContext;

		public TimeSeriesClientTestcs()
		{
			_restClient = new Mock<IRestClient>();
			_ILogger = new Mock<ILogger>();
			_gasMeteringPointCustomerClient = new Mock<IGasMeteringPointCustomerClient>();
			_dbContext = new Mock<BioDataContext>();

			var inMemorySettings = new Dictionary<string, string> {
			{"APITESTKEY", "test"}};

			configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(inMemorySettings)
			.Build();

		}

		[Fact]
		public async Task GetTimeSeries_Success()
		{

		}
	}
}
