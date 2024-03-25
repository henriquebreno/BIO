using BIO_API_DATA.API_Client.Interfaces;
using BIO_API_DATA.Data;
using BIO_API_DATA.Model;
using BIO_API_DATA.Model.CompositObjectModel;
using BIO_API_DATA.Model.CustomerModel;
using BIO_API_DATA.Model.GasMeterpointToCustomerModel;
using BIO_API_DATA.Model.TimeSeriesModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using RestSharp;
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

        private readonly BioDataContext _dbContext;


		public TimeSeriesClient(BioDataContext bioDataContext, IConfiguration configuration, ILogger<TimeSeriesClient> logger, IRestClient iRestClient)
		{
			_baseUrl = configuration.GetValue<string>("ApiSettings:TimeSeriesClient");
			_baseUrl2 = configuration.GetValue<string>("ApiSettings:CustomerClient");
			_logger = logger;
			_restClient = iRestClient;
			_dbContext = bioDataContext;
        }

		public async Task<List<CompositModel>> GetTimeSeries(List<string> customerIds,List<GasMeteringCustomerObjectModel> customerGasRelations)
		{
			List<CompositModel> compositModel = new List<CompositModel>();
			string url = _baseUrl;

			foreach (var topLevelCustomerId in customerIds)
			{
				foreach (var client in customerGasRelations)
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

			return compositModel;
			
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

		
	}
}
