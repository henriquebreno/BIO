using BIO_API_DATA.API_Client;
using Microsoft.Extensions.Configuration;
using Moq;
using RestSharp;
using Serilog;
using BIO_API_DATA.Data;
using BIO_API_DATA.Model.GasMeterpointToCustomerModel;
using Newtonsoft.Json;
using BIO_API_DATA.API_Client.ApplicationLogic;
using BIO_API_DATA.API_Client.Interfaces.Database;
using BIO_API_DATA.API_Client.Interfaces;
using BIO_API_DATA.API_Client.Database;
using BIO_API_DATA.Model;
using BIO_API_DATA.Model.CompositObjectModel;
using BIO_API_DATA.Model.TimeSeriesModel;

namespace UnitTests
{
	public class TimeSeriesClientTestcs
	{
		private readonly Mock<IRestClient> _restClient;
		private readonly Mock<IGasMeteringPointCustomerClient> _gasMeteringPointCustomerClient;
		private readonly Mock<BioDataContext> _dbContext;
		private readonly Mock<ITopLevelCustomersClientList> _topLevelCustomersClient;
		private readonly IConfiguration _configuration;
        private readonly Mock<ITimeSeriesClient> _timeSeriesClient;
        private readonly Mock<IGasMeteringPointCustomerClientList> _gasMeteringPointCustomerClientList;
        private readonly Mock<ICustomerRepository> _customerRepository;
        private readonly Mock<IGasMeterCustomerRelationRepository> _gasMeterCustomerRelationRepository;
        private readonly Mock<IGasMeterMeasurementRepository> _gasMeterMeasurementRepository;
        private readonly Mock<IObservationRepository> _observationRepository;
        private readonly Mock<IGasMeteringPointRepository> _gasMeteringPointRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;


        public TimeSeriesClientTestcs()
		{
			_timeSeriesClient = new Mock<ITimeSeriesClient>();
            _topLevelCustomersClient = new Mock<ITopLevelCustomersClientList>();
			_gasMeteringPointCustomerClientList = new Mock<IGasMeteringPointCustomerClientList>();
			_gasMeterCustomerRelationRepository = new Mock<IGasMeterCustomerRelationRepository>();
			_gasMeterMeasurementRepository = new Mock<IGasMeterMeasurementRepository>();
			_observationRepository = new Mock<IObservationRepository>();
			_gasMeteringPointRepository = new Mock<IGasMeteringPointRepository>();
            _customerRepository = new Mock<ICustomerRepository>();
			_unitOfWork = new Mock<IUnitOfWork>();
            _restClient = new Mock<IRestClient>();
			_gasMeteringPointCustomerClient = new Mock<IGasMeteringPointCustomerClient>();
			_dbContext = new Mock<BioDataContext>();
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
			const int quantityAdded = 3;
			var applicationBuildModel = new ModelBuild();
		
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

			var timeSeriesClient = new List<CompositModel>()
			{
				{
                     new CompositModel(){
                        GasMeteringCustomerObjectModel = JsonConvert.DeserializeObject<GasMeteringCustomerObjectModel>($"{{\r\n  \"id\": \"3\",\r\n  \"countryCode\": \"string\",\r\n  \"customer\": {{\r\n    \"name\": \"string\",\r\n    \"id\": \"386\",\r\n    \"link\": \"string\",\r\n    \"start\": \"2024-03-10T20:34:28.834Z\",\r\n    \"end\": \"2024-03-10T20:34:28.834Z\"\r\n  }},\r\n  \"installationAddress\": {{\r\n    \"streetCode\": \"string\",\r\n    \"streetName\": \"string\",\r\n    \"buildingNumber\": \"string\",\r\n    \"buildingFloor\": \"string\",\r\n    \"roomIdentification\": \"string\",\r\n    \"citySubDivisionName\": \"string\",\r\n    \"postalCode\": \"string\",\r\n    \"city\": \"string\",\r\n    \"municipalityCode\": \"string\",\r\n    \"countryCode\": \"string\"\r\n  }},\r\n  \"identifiers\": {{\r\n    \"meteringPointIdentification\": \"50\",\r\n    \"installationNumber\": \"string\",\r\n    \"externalIdentifier\": \"string\"\r\n  }},\r\n  \"deliveryStatus\": {{\r\n    \"inDelivery\": true,\r\n    \"end\": \"2024-03-10T20:34:28.834Z\",\r\n    \"start\": \"2024-03-10T20:34:28.834Z\"\r\n  }},\r\n  \"gridAreaDto\": {{\r\n    \"name\": \"string\"\r\n  }},\r\n  \"marketBalanceArea\": {{\r\n    \"name\": \"string\"\r\n  }},\r\n  \"calorificValueArea\": {{\r\n    \"name\": \"string\",\r\n    \"identifier\": \"string\",\r\n    \"countryCode\": \"string\"\r\n  }}\r\n}}"),
                        Customer =JsonConvert.DeserializeObject<BIO_API_DATA.Model.CustomerModel.Customer>($"{{\r\n  \"id\": \"1\",\r\n  \"countryCode\": \"string\",\r\n  \"identifiers\": {{\r\n    \"customerNumber\": \"string\",\r\n    \"externalIdentifier\": \"string\",\r\n    \"vatIdentification\": \"string\"\r\n  }},\r\n  \"contactInformation\": {{\r\n    \"address\": {{\r\n      \"streetCode\": \"string\",\r\n      \"streetName\": \"string\",\r\n      \"buildingNumber\": \"string\",\r\n      \"buildingFloor\": \"string\",\r\n      \"roomIdentification\": \"string\",\r\n      \"citySubDivisionName\": \"string\",\r\n      \"postalCode\": \"string\",\r\n      \"city\": \"string\",\r\n      \"municipalityCode\": \"string\",\r\n      \"countryCode\": \"string\"\r\n    }},\r\n    \"customerName\": {{\r\n      \"fullName\": \"string\"\r\n    }}\r\n  }},\r\n  \"parent\": {{\r\n    \"name\": \"string\",\r\n    \"id\": \"1\",\r\n    \"link\": \"string\",\r\n    \"start\": \"2024-03-10T20:37:35.580Z\",\r\n    \"end\": \"2024-03-10T20:37:35.580Z\"\r\n  }}\r\n}}"),
                        TimeSeries = JsonConvert.DeserializeObject<TimeSeries>($"{{\r\n  \"readings\": [\r\n    {{\r\n      \"start\": \"2024-03-10T20:36:28.906Z\",\r\n      \"end\": \"2024-03-10T20:36:28.906Z\",\r\n      \"resolution\": \"string\",\r\n      \"unit\": \"string\",\r\n      \"observations\": [\r\n        {{\r\n          \"quality\": \"string\",\r\n          \"value\": 0,\r\n          \"position\": 0\r\n        }}\r\n      ]\r\n    }}\r\n  ]\r\n}}")
                    }
                },
                {
                    new CompositModel(){
                        GasMeteringCustomerObjectModel = JsonConvert.DeserializeObject<GasMeteringCustomerObjectModel>($"{{\r\n  \"id\": \"3\",\r\n  \"countryCode\": \"string\",\r\n  \"customer\": {{\r\n    \"name\": \"string\",\r\n    \"id\": \"386\",\r\n    \"link\": \"string\",\r\n    \"start\": \"2024-03-10T20:34:28.834Z\",\r\n    \"end\": \"2024-03-10T20:34:28.834Z\"\r\n  }},\r\n  \"installationAddress\": {{\r\n    \"streetCode\": \"string\",\r\n    \"streetName\": \"string\",\r\n    \"buildingNumber\": \"string\",\r\n    \"buildingFloor\": \"string\",\r\n    \"roomIdentification\": \"string\",\r\n    \"citySubDivisionName\": \"string\",\r\n    \"postalCode\": \"string\",\r\n    \"city\": \"string\",\r\n    \"municipalityCode\": \"string\",\r\n    \"countryCode\": \"string\"\r\n  }},\r\n  \"identifiers\": {{\r\n    \"meteringPointIdentification\": \"50\",\r\n    \"installationNumber\": \"string\",\r\n    \"externalIdentifier\": \"string\"\r\n  }},\r\n  \"deliveryStatus\": {{\r\n    \"inDelivery\": true,\r\n    \"end\": \"2024-03-10T20:34:28.834Z\",\r\n    \"start\": \"2024-03-10T20:34:28.834Z\"\r\n  }},\r\n  \"gridAreaDto\": {{\r\n    \"name\": \"string\"\r\n  }},\r\n  \"marketBalanceArea\": {{\r\n    \"name\": \"string\"\r\n  }},\r\n  \"calorificValueArea\": {{\r\n    \"name\": \"string\",\r\n    \"identifier\": \"string\",\r\n    \"countryCode\": \"string\"\r\n  }}\r\n}}"),
                        Customer =JsonConvert.DeserializeObject<BIO_API_DATA.Model.CustomerModel.Customer>($"{{\r\n  \"id\": \"1\",\r\n  \"countryCode\": \"string\",\r\n  \"identifiers\": {{\r\n    \"customerNumber\": \"string\",\r\n    \"externalIdentifier\": \"string\",\r\n    \"vatIdentification\": \"string\"\r\n  }},\r\n  \"contactInformation\": {{\r\n    \"address\": {{\r\n      \"streetCode\": \"string\",\r\n      \"streetName\": \"string\",\r\n      \"buildingNumber\": \"string\",\r\n      \"buildingFloor\": \"string\",\r\n      \"roomIdentification\": \"string\",\r\n      \"citySubDivisionName\": \"string\",\r\n      \"postalCode\": \"string\",\r\n      \"city\": \"string\",\r\n      \"municipalityCode\": \"string\",\r\n      \"countryCode\": \"string\"\r\n    }},\r\n    \"customerName\": {{\r\n      \"fullName\": \"string\"\r\n    }}\r\n  }},\r\n  \"parent\": {{\r\n    \"name\": \"string\",\r\n    \"id\": \"1\",\r\n    \"link\": \"string\",\r\n    \"start\": \"2024-03-10T20:37:35.580Z\",\r\n    \"end\": \"2024-03-10T20:37:35.580Z\"\r\n  }}\r\n}}"),
                        TimeSeries = JsonConvert.DeserializeObject<TimeSeries>($"{{\r\n  \"readings\": [\r\n    {{\r\n      \"start\": \"2024-03-10T20:36:28.906Z\",\r\n      \"end\": \"2024-03-10T20:36:28.906Z\",\r\n      \"resolution\": \"string\",\r\n      \"unit\": \"string\",\r\n      \"observations\": [\r\n        {{\r\n          \"quality\": \"string\",\r\n          \"value\": 0,\r\n          \"position\": 0\r\n        }}\r\n      ]\r\n    }}\r\n  ]\r\n}}")
                    }
                },
                {
                     new CompositModel(){
                        GasMeteringCustomerObjectModel = JsonConvert.DeserializeObject<GasMeteringCustomerObjectModel>($"{{\r\n  \"id\": \"3\",\r\n  \"countryCode\": \"string\",\r\n  \"customer\": {{\r\n    \"name\": \"string\",\r\n    \"id\": \"386\",\r\n    \"link\": \"string\",\r\n    \"start\": \"2024-03-10T20:34:28.834Z\",\r\n    \"end\": \"2024-03-10T20:34:28.834Z\"\r\n  }},\r\n  \"installationAddress\": {{\r\n    \"streetCode\": \"string\",\r\n    \"streetName\": \"string\",\r\n    \"buildingNumber\": \"string\",\r\n    \"buildingFloor\": \"string\",\r\n    \"roomIdentification\": \"string\",\r\n    \"citySubDivisionName\": \"string\",\r\n    \"postalCode\": \"string\",\r\n    \"city\": \"string\",\r\n    \"municipalityCode\": \"string\",\r\n    \"countryCode\": \"string\"\r\n  }},\r\n  \"identifiers\": {{\r\n    \"meteringPointIdentification\": \"50\",\r\n    \"installationNumber\": \"string\",\r\n    \"externalIdentifier\": \"string\"\r\n  }},\r\n  \"deliveryStatus\": {{\r\n    \"inDelivery\": true,\r\n    \"end\": \"2024-03-10T20:34:28.834Z\",\r\n    \"start\": \"2024-03-10T20:34:28.834Z\"\r\n  }},\r\n  \"gridAreaDto\": {{\r\n    \"name\": \"string\"\r\n  }},\r\n  \"marketBalanceArea\": {{\r\n    \"name\": \"string\"\r\n  }},\r\n  \"calorificValueArea\": {{\r\n    \"name\": \"string\",\r\n    \"identifier\": \"string\",\r\n    \"countryCode\": \"string\"\r\n  }}\r\n}}"),
                        Customer =JsonConvert.DeserializeObject<BIO_API_DATA.Model.CustomerModel.Customer>($"{{\r\n  \"id\": \"1\",\r\n  \"countryCode\": \"string\",\r\n  \"identifiers\": {{\r\n    \"customerNumber\": \"string\",\r\n    \"externalIdentifier\": \"string\",\r\n    \"vatIdentification\": \"string\"\r\n  }},\r\n  \"contactInformation\": {{\r\n    \"address\": {{\r\n      \"streetCode\": \"string\",\r\n      \"streetName\": \"string\",\r\n      \"buildingNumber\": \"string\",\r\n      \"buildingFloor\": \"string\",\r\n      \"roomIdentification\": \"string\",\r\n      \"citySubDivisionName\": \"string\",\r\n      \"postalCode\": \"string\",\r\n      \"city\": \"string\",\r\n      \"municipalityCode\": \"string\",\r\n      \"countryCode\": \"string\"\r\n    }},\r\n    \"customerName\": {{\r\n      \"fullName\": \"string\"\r\n    }}\r\n  }},\r\n  \"parent\": {{\r\n    \"name\": \"string\",\r\n    \"id\": \"1\",\r\n    \"link\": \"string\",\r\n    \"start\": \"2024-03-10T20:37:35.580Z\",\r\n    \"end\": \"2024-03-10T20:37:35.580Z\"\r\n  }}\r\n}}"),
                        TimeSeries = JsonConvert.DeserializeObject<TimeSeries>($"{{\r\n  \"readings\": [\r\n    {{\r\n      \"start\": \"2024-03-10T20:36:28.906Z\",\r\n      \"end\": \"2024-03-10T20:36:28.906Z\",\r\n      \"resolution\": \"string\",\r\n      \"unit\": \"string\",\r\n      \"observations\": [\r\n        {{\r\n          \"quality\": \"string\",\r\n          \"value\": 0,\r\n          \"position\": 0\r\n        }}\r\n      ]\r\n    }}\r\n  ]\r\n}}")
                    }
                }
            };
           
			_gasMeteringPointCustomerClient.Setup(c => c.GetGasCustomer(It.IsAny<List<GasMeterPointCustomerModel>>())).ReturnsAsync(gasContentList);
            _topLevelCustomersClient.Setup(c => c.GetAllCustomers()).ReturnsAsync(customerResponse);
			_timeSeriesClient.Setup(x => x.GetTimeSeries(It.IsAny<List<string>>(), It.IsAny<List<GasMeteringCustomerObjectModel>>())).ReturnsAsync(timeSeriesClient);
			_gasMeteringPointRepository.Setup(x => x.GetAll()).Returns(applicationBuildModel.GetGasMeteringPoints());
			_gasMeteringPointRepository.Setup(x => x.Add(It.IsAny<BIO_API_DATA.Data.GasMeteringPoint>()));
			_customerRepository.Setup(x => x.Add(It.IsAny<BIO_API_DATA.Data.Customer>()));
			_gasMeterCustomerRelationRepository.Setup(x => x.Add(It.IsAny<BIO_API_DATA.Data.GasMeterCustomerRelation>()));
			_observationRepository.Setup(x => x.Add(It.IsAny<BIO_API_DATA.Data.Observation>()));
			_gasMeterMeasurementRepository.Setup(x => x.Add(It.IsAny<BIO_API_DATA.Data.GasMeterMeasurement>()));

            var client = new TimeSeriesLogic(_timeSeriesClient.Object, _topLevelCustomersClient.Object, _gasMeteringPointCustomerClientList.Object, _gasMeteringPointCustomerClient.Object, _customerRepository.Object, _gasMeterCustomerRelationRepository.Object, _gasMeterMeasurementRepository.Object, _observationRepository.Object, _gasMeteringPointRepository.Object,_unitOfWork.Object);

			//Act
			await client.AddTimeSeriesAsync();

            //Assert
            _gasMeteringPointRepository.Verify(x => x.Add(It.IsAny<BIO_API_DATA.Data.GasMeteringPoint>()), Times.Exactly(quantityAdded));
            _customerRepository.Verify(x => x.Add(It.IsAny<BIO_API_DATA.Data.Customer>()), Times.Exactly(quantityAdded));
            _gasMeterCustomerRelationRepository.Verify(x => x.Add(It.IsAny<BIO_API_DATA.Data.GasMeterCustomerRelation>()), Times.Exactly(quantityAdded));
            _observationRepository.Verify(x => x.Add(It.IsAny<BIO_API_DATA.Data.Observation>()), Times.Exactly(quantityAdded));
            _gasMeterMeasurementRepository.Verify(x => x.Add(It.IsAny<BIO_API_DATA.Data.GasMeterMeasurement>()), Times.Exactly(quantityAdded));
        }

	}
}