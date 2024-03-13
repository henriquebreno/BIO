using BIO_API_DATA.API_Client.Interfaces;
using BIO_API_DATA.Data;
using BIO_API_DATA.Model;
using BIO_API_DATA.Model.CompositObjectModel;
using BIO_API_DATA.Model.CustomerModel;
using BIO_API_DATA.Model.GasMeterpointToCustomerModel;
using BIO_API_DATA.Model.TimeSeriesModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.API_Client
{
	public class TimeSeriesClient : ITimeSeriesClient
	{ 
		private readonly ILogger _logger;
		private readonly string _baseUrl;
		private readonly string _baseUrl2;
		private readonly IRestClient _restClient;
		private readonly IGasMeteringPointCustomerClient _gasMeteringPointCustomerClient;
		private readonly BioDataContext _dbContext;
		private readonly ITopLevelCustomersClientList _topLevelCustomersClient;


        public TimeSeriesClient(BioDataContext bioDataContext, IConfiguration configuration, ILogger logger, IRestClient iRestClient, IGasMeteringPointCustomerClient customerClient, ITopLevelCustomersClientList topLevelCustomersClient)
		{
			_baseUrl = configuration.GetValue<string>("ApiSettings:TimeSeriesClient");
			_baseUrl2 = configuration.GetValue<string>("ApiSettings:CustomerClient");
			_logger = logger;
			_restClient = iRestClient;
			_gasMeteringPointCustomerClient = customerClient;
			_dbContext = bioDataContext;
			_topLevelCustomersClient = topLevelCustomersClient;

        }
		public async Task GetTimeSeries()
		{
			List<CompositModel> compositModel = new List<CompositModel>();
			string url = _baseUrl;
			var gasMeteringPointCustomerClientList = await _gasMeteringPointCustomerClient.GetGasCustomer();
			var allCustomerIds = await _topLevelCustomersClient.GetAllCustomers();


            foreach (var topLevelCustomerId in allCustomerIds)
			{
				foreach (var client in gasMeteringPointCustomerClientList)
				{
					//If customer is not associated with the gasmeter anymore ignore
					if (client.Customer.End.Date < DateTime.Now.Date)
					{
						continue;
					}

					url += $"/api/v1/topLevelCustomers/{topLevelCustomerId}/gasMeteringPoints/{client.Identifiers.MeteringPointIdentification}/timeSeries/invoiceRelevant?Start={client.DeliveryStatus.Start.ToString("yyyy-MM-ddTHH:00:00.00Z")}&End={client.DeliveryStatus.End.ToString("yyyy-MM-ddTHH:00:00.00Z")}";

					var request = new RestRequest(url);
					var response = await _restClient.GetAsync(request);

					if (!response.IsSuccessful)
					{
						throw new Exception($"Error getting gasmetringpointsCustumerRelation: {response.StatusDescription}");
					}

					var content = response.Content;

					var timeseries = JsonConvert.DeserializeObject<TimeSeries>(content);

					var customerDetailedInfo = await GetCustomerInfo(topLevelCustomerId, client.Customer.Id);

					compositModel.Add(new CompositModel
					{
						GasMeteringCustomerObjectModel = client,
						Customer = customerDetailedInfo,
						TimeSeries = timeseries

					});

				}
			}

			MapData(compositModel);
			
		}

		public async Task<Model.CustomerModel.Customer> GetCustomerInfo(string topLevelCustomerId, string customerId)
		{
			string url = _baseUrl2;

			url += $"/api/v1/topLevelCustomers/{topLevelCustomerId}/customers/{customerId}";

			var request = new RestRequest(url);
			var response = await _restClient.GetAsync(request);

			if (!response.IsSuccessful)
			{
				throw new Exception($"Error getting gasmetringpointsCustumerRelation: {response.StatusDescription}");
			}

			var content = response.Content;

			var customerDetailedInfo = JsonConvert.DeserializeObject<Model.CustomerModel.Customer>(content);

			return customerDetailedInfo;
		}

		public void MapData(List<CompositModel> compositModel)
		{
			

			foreach(var item in compositModel)
			{
				var gasMeteringPointEntity = new Data.GasMeteringPoint();
				var gasMeterCustomerRelationEntity = new GasMeterCustomerRelation();
				var customerEntity = new Data.Customer();
				var gasMeterMeasurementEntity = new GasMeterMeasurement();
				var observationEntity = new Data.Observation();


				// Create a customer
				var customer = new Data.Customer()
				{
					Id = int.Parse(item.Customer.id),
					CustomerNumber = item.Customer.identifiers.customerNumber,
					EffectiveStartTimeUtc = item.Customer.parent.start,
					EffectiveEndTimeUtc = item.Customer.parent.end,
					VatIdentification = item.Customer.identifiers.vatIdentification,
					CustomerName = item.Customer.contactInformation.customerName.fullName,
					Country = item.Customer.contactInformation.address.countryCode,
					MunicipalityCode = item.Customer.contactInformation.address.municipalityCode,
					PostalCode = item.Customer.contactInformation.address.postalCode,
					City = item.Customer.contactInformation.address.city,
					StreetName = item.Customer.contactInformation.address.streetName,
					BuildingNumber = item.Customer.contactInformation.address.buildingNumber,
					BuildingFloor = item.Customer.contactInformation.address.buildingFloor,
					RoomIdentification = item.Customer.contactInformation.address.roomIdentification,
					Source = "Web"
				};

				// Create a gas metering point
				var gasMeteringPoint = new Data.GasMeteringPoint
                {
					Id = int.Parse(item.GasMeteringCustomerObjectModel.Id),
					MeterId = int.Parse(item.GasMeteringCustomerObjectModel.Identifiers.MeteringPointIdentification),
					EffectiveStartTimeUtc = item.GasMeteringCustomerObjectModel.DeliveryStatus.Start,
					EffectiveEndTimeUtc = item.GasMeteringCustomerObjectModel.DeliveryStatus.End,
					InDelivery = item.GasMeteringCustomerObjectModel.DeliveryStatus.InDelivery,
					PriceAreaCode = "",
					InstallationCountry = item.GasMeteringCustomerObjectModel.InstallationAddress.CountryCode,
					InstallationMunicipalityCode = item.GasMeteringCustomerObjectModel.InstallationAddress.MunicipalityCode,
					InstalationPostalCode = item.GasMeteringCustomerObjectModel.InstallationAddress.PostalCode,
					InstallationCity = item.GasMeteringCustomerObjectModel.InstallationAddress.City,
					InstallationStreetName = item.GasMeteringCustomerObjectModel.InstallationAddress.StreetName,
					InstallationBuildingNumber = item.GasMeteringCustomerObjectModel.InstallationAddress.BuildingNumber,
					InstallationBuildingFloor = item.GasMeteringCustomerObjectModel.InstallationAddress.BuildingFloor,
					InstallationRoomIdentification = item.GasMeteringCustomerObjectModel.InstallationAddress.RoomIdentification,
					Source = "Web"
				};

				// Create a gas meter customer relation
				var gasMeterCustomerRelation = new GasMeterCustomerRelation
				{
					EffectiveStartTimeUtc = item.GasMeteringCustomerObjectModel.Customer.Start,
					EffectiveEndTimeUtc = item.GasMeteringCustomerObjectModel.Customer.End,
					Customer = customer,
					GasMeteringPoint = gasMeteringPoint,
					Source = "Web"
				};

				GasMeterMeasurement gasMeterMeasurement = new GasMeterMeasurement();

				foreach (var measurement in item.TimeSeries.Readings)
				{
					// Create a gas meter measurement
					gasMeterMeasurement = new GasMeterMeasurement
					{
						Start = measurement.Start,
						End = measurement.End,
						Resolution = measurement.Resolution,
						Unit = measurement.Unit,
						MeteringPointIdentificationNavigation = gasMeteringPoint
					};
				}


				//Check if correction
				bool exist = false;
				GasMeterMeasurement check;
				foreach (var measurement in item.TimeSeries.Readings)
				{
					check = _dbContext.GasMeterMeasurements.FirstOrDefault(e => e.Start == measurement.Start && e.End == measurement.End);
					if (check != null)
					{
						exist = true;
					}
				}
				var observation = new Data.Observation();

				foreach(var read in item.TimeSeries.Readings)
				{
					foreach(var obs in read.Observations)
					{
						observation = new Data.Observation
						{
							Quality = obs.Quality,
							Value = obs.Value,
							Position = obs.Position,
							Correction = exist,
							GasMeterMeasurement = gasMeterMeasurement
						};
					}
				}

				// Add entities to context
				_dbContext.Customers.Add(customer);
				_dbContext.GasMeteringPoints.Add(gasMeteringPoint);
				_dbContext.GasMeterCustomerRelations.Add(gasMeterCustomerRelation);
				_dbContext.GasMeterMeasurements.Add(gasMeterMeasurement);
				_dbContext.Observations.Add(observation);
			}

			

			// Save changes to the database
			_dbContext.SaveChanges();

		}
	}
}
