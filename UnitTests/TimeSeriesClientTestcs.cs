using BIO_API_DATA.API_Client;
using Microsoft.Extensions.Configuration;
using Moq;
using RestSharp;
using Serilog;
using BIO_API_DATA.Data;
using BIO_API_DATA.Model.GasMeterpointToCustomerModel;
using Newtonsoft.Json;

namespace UnitTests
{
    public class TimeSeriesClientTestcs
	{
		private readonly Mock<IRestClient> _restClient;
		private readonly string BaseUrl = "https://bioapi";
		private readonly Mock<ILogger> _ILogger;
		private readonly Mock<IGasMeteringPointCustomerClient> _gasMeteringPointCustomerClient;  
        private readonly Mock<BioDataContext> _dbContext;
        private readonly Mock<ITopLevelCustomersClientList> _topLevelCustomersClientList;
        private readonly IConfiguration _configuration;



        public TimeSeriesClientTestcs()
		{
			_restClient = new Mock<IRestClient>();
			_ILogger = new Mock<ILogger>();
			_gasMeteringPointCustomerClient = new Mock<IGasMeteringPointCustomerClient>();
			_dbContext = new Mock<BioDataContext>();
            _topLevelCustomersClientList = new Mock<ITopLevelCustomersClientList>();
            Dictionary<string, string> inMemorySettings =
            new Dictionary<string, string> {
                {"ApiSettings:TimeSeriesClient", "https://your-api-url-for-class-1"},
                {"ApiSettings:CustomerClient", "https://your-api-url-for-class-2"},
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

		[Fact]
		public async Task GetTimeSeries_Success()
		{
            //Arrange
            const int minimumQuantityAdded = 1;
            var applicationBuildModel = new ModelBuild();
           
            var TimeSeriesResponse = new RestResponse
            {
                Content = $"{{\r\n  \"readings\": [\r\n    {{\r\n      \"start\": \"2024-03-10T20:36:28.906Z\",\r\n      \"end\": \"2024-03-10T20:36:28.906Z\",\r\n      \"resolution\": \"string\",\r\n      \"unit\": \"string\",\r\n      \"observations\": [\r\n        {{\r\n          \"quality\": \"string\",\r\n          \"value\": 0,\r\n          \"position\": 0\r\n        }}\r\n      ]\r\n    }}\r\n  ]\r\n}}",
                StatusCode = System.Net.HttpStatusCode.OK,
                StatusDescription = "OK",
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed
            };

            var CustomerInfo = new RestResponse
            {
                Content = $"{{\r\n  \"id\": \"1\",\r\n  \"countryCode\": \"string\",\r\n  \"identifiers\": {{\r\n    \"customerNumber\": \"string\",\r\n    \"externalIdentifier\": \"string\",\r\n    \"vatIdentification\": \"string\"\r\n  }},\r\n  \"contactInformation\": {{\r\n    \"address\": {{\r\n      \"streetCode\": \"string\",\r\n      \"streetName\": \"string\",\r\n      \"buildingNumber\": \"string\",\r\n      \"buildingFloor\": \"string\",\r\n      \"roomIdentification\": \"string\",\r\n      \"citySubDivisionName\": \"string\",\r\n      \"postalCode\": \"string\",\r\n      \"city\": \"string\",\r\n      \"municipalityCode\": \"string\",\r\n      \"countryCode\": \"string\"\r\n    }},\r\n    \"customerName\": {{\r\n      \"fullName\": \"string\"\r\n    }}\r\n  }},\r\n  \"parent\": {{\r\n    \"name\": \"string\",\r\n    \"id\": \"1\",\r\n    \"link\": \"string\",\r\n    \"start\": \"2024-03-10T20:37:35.580Z\",\r\n    \"end\": \"2024-03-10T20:37:35.580Z\"\r\n  }}\r\n}}",
                StatusCode = System.Net.HttpStatusCode.OK,
                StatusDescription = "OK",
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed
            };

            var gasContentList = new List<GasMeteringCustomerObjectModel>() {
                { 
                    JsonConvert.DeserializeObject<GasMeteringCustomerObjectModel>($"{{\r\n  \"id\": \"3\",\r\n  \"countryCode\": \"string\",\r\n  \"customer\": {{\r\n    \"name\": \"string\",\r\n    \"id\": \"386\",\r\n    \"link\": \"string\",\r\n    \"start\": \"2024-03-10T20:34:28.834Z\",\r\n    \"end\": \"2024-03-10T20:34:28.834Z\"\r\n  }},\r\n  \"installationAddress\": {{\r\n    \"streetCode\": \"string\",\r\n    \"streetName\": \"string\",\r\n    \"buildingNumber\": \"string\",\r\n    \"buildingFloor\": \"string\",\r\n    \"roomIdentification\": \"string\",\r\n    \"citySubDivisionName\": \"string\",\r\n    \"postalCode\": \"string\",\r\n    \"city\": \"string\",\r\n    \"municipalityCode\": \"string\",\r\n    \"countryCode\": \"string\"\r\n  }},\r\n  \"identifiers\": {{\r\n    \"meteringPointIdentification\": \"50\",\r\n    \"installationNumber\": \"string\",\r\n    \"externalIdentifier\": \"string\"\r\n  }},\r\n  \"deliveryStatus\": {{\r\n    \"inDelivery\": true,\r\n    \"end\": \"2024-03-10T20:34:28.834Z\",\r\n    \"start\": \"2024-03-10T20:34:28.834Z\"\r\n  }},\r\n  \"gridAreaDto\": {{\r\n    \"name\": \"string\"\r\n  }},\r\n  \"marketBalanceArea\": {{\r\n    \"name\": \"string\"\r\n  }},\r\n  \"calorificValueArea\": {{\r\n    \"name\": \"string\",\r\n    \"identifier\": \"string\",\r\n    \"countryCode\": \"string\"\r\n  }}\r\n}}")
                },
                {
                    JsonConvert.DeserializeObject<GasMeteringCustomerObjectModel>($"{{\r\n  \"id\": \"34\",\r\n  \"countryCode\": \"string\",\r\n  \"customer\": {{\r\n    \"name\": \"string\",\r\n    \"id\": \"36\",\r\n    \"link\": \"string\",\r\n    \"start\": \"2024-03-10T20:34:28.834Z\",\r\n    \"end\": \"2024-03-10T20:34:28.834Z\"\r\n  }},\r\n  \"installationAddress\": {{\r\n    \"streetCode\": \"string\",\r\n    \"streetName\": \"string\",\r\n    \"buildingNumber\": \"string\",\r\n    \"buildingFloor\": \"string\",\r\n    \"roomIdentification\": \"string\",\r\n    \"citySubDivisionName\": \"string\",\r\n    \"postalCode\": \"string\",\r\n    \"city\": \"string\",\r\n    \"municipalityCode\": \"string\",\r\n    \"countryCode\": \"string\"\r\n  }},\r\n  \"identifiers\": {{\r\n    \"meteringPointIdentification\": \"90\",\r\n    \"installationNumber\": \"string\",\r\n    \"externalIdentifier\": \"string\"\r\n  }},\r\n  \"deliveryStatus\": {{\r\n    \"inDelivery\": true,\r\n    \"end\": \"2024-03-10T20:34:28.834Z\",\r\n    \"start\": \"2024-03-10T20:34:28.834Z\"\r\n  }},\r\n  \"gridAreaDto\": {{\r\n    \"name\": \"string\"\r\n  }},\r\n  \"marketBalanceArea\": {{\r\n    \"name\": \"string\"\r\n  }},\r\n  \"calorificValueArea\": {{\r\n    \"name\": \"string\",\r\n    \"identifier\": \"string\",\r\n    \"countryCode\": \"string\"\r\n  }}\r\n}}")
                }
            };

            var customerInfo = JsonConvert.DeserializeObject<BIO_API_DATA.Model.CustomerModel.Customer>($"{{\r\n  \"id\": \"1\",\r\n  \"countryCode\": \"string\",\r\n  \"identifiers\": {{\r\n    \"customerNumber\": \"string\",\r\n    \"externalIdentifier\": \"string\",\r\n    \"vatIdentification\": \"string\"\r\n  }},\r\n  \"contactInformation\": {{\r\n    \"address\": {{\r\n      \"streetCode\": \"string\",\r\n      \"streetName\": \"string\",\r\n      \"buildingNumber\": \"string\",\r\n      \"buildingFloor\": \"string\",\r\n      \"roomIdentification\": \"string\",\r\n      \"citySubDivisionName\": \"string\",\r\n      \"postalCode\": \"string\",\r\n      \"city\": \"string\",\r\n      \"municipalityCode\": \"string\",\r\n      \"countryCode\": \"string\"\r\n    }},\r\n    \"customerName\": {{\r\n      \"fullName\": \"string\"\r\n    }}\r\n  }},\r\n  \"parent\": {{\r\n    \"name\": \"string\",\r\n    \"id\": \"30279\",\r\n    \"link\": \"string\",\r\n    \"start\": \"2024-03-10T20:37:35.580Z\",\r\n    \"end\": \"2024-03-10T20:37:35.580Z\"\r\n  }}\r\n}}");            
            var customerResponse = new List<string>
            {
                { "customer1"},
                { "customer2"},
                { "customer3"},
            };


            _restClient.Setup(s => s.ExecuteAsync(It.Is<RestRequest>(r => r.Resource.StartsWith(_configuration.GetValue<string>("ApiSettings:TimeSeriesClient"))), It.IsAny<CancellationToken>())).ReturnsAsync(TimeSeriesResponse);
            _restClient.Setup(s => s.ExecuteAsync(It.Is<RestRequest>(r => r.Resource.StartsWith(_configuration.GetValue<string>("ApiSettings:CustomerClient"))), It.IsAny<CancellationToken>())).ReturnsAsync(CustomerInfo);
            _gasMeteringPointCustomerClient.Setup(c => c.GetGasCustomer()).ReturnsAsync(gasContentList);
            _topLevelCustomersClientList.Setup(c => c.GetAllCustomers()).ReturnsAsync(customerResponse);
            _dbContext.Setup(x => x.GasMeterMeasurements).Returns(DbContextHelper.GetMockDbSet(applicationBuildModel.GetGasMeterCustomerRelation()).Object);
            _dbContext.Setup(x => x.Customers).Returns(DbContextHelper.GetMockDbSet(applicationBuildModel.GetCustomers()).Object);
            _dbContext.Setup(x => x.GasMeterCustomerRelations).Returns(DbContextHelper.GetMockDbSet(applicationBuildModel.GetGasMeterCustomerRelations()).Object);
            _dbContext.Setup(x => x.Observations).Returns(DbContextHelper.GetMockDbSet(applicationBuildModel.GetObservations()).Object);
            _dbContext.Setup(x => x.GasMeteringPoints).Returns(DbContextHelper.GetMockDbSet(applicationBuildModel.GetGasMeteringPoints()).Object);

            var client = new TimeSeriesClient(_dbContext.Object, _configuration, _ILogger.Object, _restClient.Object, _gasMeteringPointCustomerClient.Object, _topLevelCustomersClientList.Object);
            
            //Act
            await client.GetTimeSeries();

            //Assert
            Assert.True(_dbContext.Object.Customers.Count() > minimumQuantityAdded);
            Assert.True(_dbContext.Object.GasMeterMeasurements.Count() > minimumQuantityAdded);
            Assert.True(_dbContext.Object.GasMeterCustomerRelations.Count() > minimumQuantityAdded);
            Assert.True(_dbContext.Object.Observations.Count() > minimumQuantityAdded);
            Assert.True(_dbContext.Object.GasMeteringPoints.Count() > minimumQuantityAdded);
        }

	}
}
