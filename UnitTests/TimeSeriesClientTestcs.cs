using BIO_API_DATA.API_Client;
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
        private Mock<IRestClientFactory> _restClientFactory;

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
            _restClientFactory = new Mock<IRestClientFactory>();


        }

		[Fact]
		public async Task GetTimeSeries_Success()
		{
            //Arrange
            var clientResult = new RestResponse
            {
                Content = $"{{\"TopLevelCustomerIds\":[\"customer1\",\"customer2\",\"customer3\"],\"Next\":\"\",\"Prev\":\"\"}}",
                StatusCode = System.Net.HttpStatusCode.OK,
                StatusDescription = "OK",
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed
            };


            var customerResponse = new List<string>
            {
                { "customer1"},
                { "customer2"},
                { "customer3"},
            };


            _restClient.Setup(s => s.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(clientResult);



            var client = new TopLevelCustomersClientList(configuration, _ILogger.Object, _restClientFactory.Object, _restClient.Object);

            //Act
            
            var result = await client.GetAllCustomers();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.True(customerResponse.SequenceEqual(result));
        }
	}
}
