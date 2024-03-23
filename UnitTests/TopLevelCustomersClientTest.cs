using BIO_API_DATA.API_Client;
using BIO_API_DATA.Model;
using Castle.Core.Resource;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using static BIO_API_DATA.API_Client.TopLevelCustomersClientList;

namespace UnitTests
{
    public class TopLevelCustomersClientTest
    {

        private Mock<IRestClient> _restClient;
        private readonly string BaseUrl = "https://bioapi";
        private Mock<ILogger> _ILogger;
        private Mock<IRestClientFactory> _RestClientFactory;
        private IConfiguration configuration;


		public TopLevelCustomersClientTest()
        {
            _RestClientFactory = new Mock<IRestClientFactory>();
            _restClient = new Mock<IRestClient>();
            _ILogger = new Mock<ILogger>();
			
            var inMemorySettings = new Dictionary<string, string> {
			{"APITESTKEY", "test"}};

			configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(inMemorySettings)
			.Build();

		}
        [Fact]
        public async Task GetAllCustomers_Success_ReturnsCustomerIds()
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



            var client = new TopLevelCustomersClientList(configuration, _ILogger.Object, _RestClientFactory.Object, _restClient.Object);

            //Act
            
            var result = await client.GetAllCustomers();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.True(customerResponse.SequenceEqual(result));

        }

        [Fact]
        public async Task GetAllCustomers_Success_ReturnsCustomerIdsPaginated()
        {

            //Arrange
            var clientResult1 = new RestResponse
            {
                Content = $"{{\"TopLevelCustomerIds\":[\"customer1\",\"customer2\",\"customer3\"],\"Next\":\"https://api.example.com/customers?page=2\",\"Prev\":\"https://api.example.com/customers?page=1\"}}",
                StatusCode = System.Net.HttpStatusCode.OK,
                StatusDescription = "OK",
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed
            };
            var clientResult2 = new RestResponse
            {
                Content = $"{{\"TopLevelCustomerIds\":[\"customer4\",\"customer5\",\"customer6\"],\"Next\":\"https://api.example.com/customers?page=3\",\"Prev\":\"https://api.example.com/customers?page=2\"}}",
                StatusCode = System.Net.HttpStatusCode.OK,
                StatusDescription = "OK",
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed
            };

            var clientResult3 = new RestResponse
            {
                Content = $"{{\"TopLevelCustomerIds\":[\"customer7\",\"customer8\",\"customer9\"],\"Next\":\"\",\"Prev\":\"https://api.example.com/customers?page=3\"}}",
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
                    { "customer4"},
                    { "customer5"},
                    { "customer6"},
                    { "customer7"},
                    { "customer8"},
                    { "customer9"}
                };


            _restClient.SetupSequence(s => s.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(clientResult1)
                       .ReturnsAsync(clientResult2)
                       .ReturnsAsync(clientResult3);



            var client = new TopLevelCustomersClientList(configuration, _ILogger.Object, _RestClientFactory.Object, _restClient.Object);

			//Act
			
			var result = await client.GetAllCustomers();


			Assert.NotNull(result);
            Assert.Equal(9, result.Count);
            Assert.True(customerResponse.SequenceEqual(result));

        }


        [Fact]
        public async Task GetAllCustomers_Error_ReturnsEmptyList()
        {
            // Arrange

            _restClient.Setup(s => s.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new RestResponse { IsSuccessStatusCode = false, StatusDescription = "Internal Server Error" });

            var client = new TopLevelCustomersClientList(configuration, _ILogger.Object, _RestClientFactory.Object, _restClient.Object);

			//Act
            // Assert
            await Assert.ThrowsAsync<Exception>(client.GetAllCustomers);
         
        }



    }
}