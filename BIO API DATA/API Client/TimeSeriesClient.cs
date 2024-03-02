using BIO_API_DATA.Model.GasMeterpointToCustomerModel;
using Microsoft.Extensions.Configuration;
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
	public class TimeSeriesClient 
	{ 
		private readonly ILogger _logger;
		private readonly string _baseUrl;
		private readonly IRestClient _restClient;
		private readonly GasMeteringPointCustomerClient _gasMeteringPointCustomerClient;

	public TimeSeriesClient(IConfiguration configuration, ILogger logger, IRestClient iRestClient, GasMeteringPointCustomerClient customerClient)
		{
			_baseUrl = configuration.GetValue<string>("ApiSettings:TimeSeriesClient");
			_logger = logger;
			_restClient = iRestClient;
			_gasMeteringPointCustomerClient = customerClient;
		}
		public async void GetTimeSeries()
		{
			string url = _baseUrl;
			var _gasMeteringPointCustomerClientList = _gasMeteringPointCustomerClient.GetGasCustomer();


			foreach (var topLevelCustomerId in TopLevelCustomersClientList._allCustomerIds)
			{
				foreach (var client in _gasMeteringPointCustomerClientList.Result)
				{

					//need to be implemented Find out if a gasmeter is curently asociated with th customer

					url += $"/api/v1/topLevelCustomers/{topLevelCustomerId}/gasMeteringPoints/{client.Identifiers.MeteringPointIdentification}/timeSeries/invoiceRelevant?Start={client.DeliveryStatus.Start}&End={client.DeliveryStatus.End}";

					var request = new RestRequest(url);
					var response = await _restClient.GetAsync(request);

					if (!response.IsSuccessful)
					{
						throw new Exception($"Error getting gasmetringpointsCustumerRelation: {response.StatusDescription}");
					}

					var content = response.Content;

					var gasMeteringCustomerObjectModel = JsonConvert.DeserializeObject<GasMeteringCustomerObjectModel>(content);
				}
			}

			
		}
	}
}
