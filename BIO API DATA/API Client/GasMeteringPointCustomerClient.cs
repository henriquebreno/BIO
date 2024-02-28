using BIO_API_DATA.Model;
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
	
	public class GasMeteringPointCustomerClient
	{
		private readonly ILogger _logger;
		private readonly string _baseUrl;
		private readonly IRestClient _restClient;
		private readonly GasMeteringPointCustomerListClient _customerGas;
		public GasMeteringPointCustomerClient(IConfiguration configuration, ILogger logger, IRestClient iRestClient, GasMeteringPointCustomerListClient customersClient)
        {
			_baseUrl = configuration.GetValue<string>("ApiSettings:GasMeteringPointCustomerClient");
			_logger = logger;
			_restClient = iRestClient;
			_customerGas = customersClient;
		}

		public async void GetGasCustomer()
		{
			var customerGasrelations = _customerGas.GetGasmetringPointCustomerassociation();
			string url = _baseUrl;  

			foreach (var customer in customerGasrelations.Result)
			{
				foreach(var gas in customer.GasMeteringPoints)
				{
					url += $"/api/v1/topLevelCustomers/{customer.CustomerId}/gasMeteringPoints/{gas.Id}";
					var request = new RestRequest(url);
					var response = await _restClient.GetAsync(request);

					if (!response.IsSuccessful)
					{
						throw new Exception($"Error getting gasmetringpointsCustumerRelation: {response.StatusDescription}");
					}

					_logger.Information("Response status: {StatusDescription}", response.StatusDescription);

					var content = response.Content;

					var responseData = JsonConvert.DeserializeObject<GasMeteringCustomerObjectModel>(content);

				}
				
				
			}
		}

		public void SaveGasCustomer()
		{

		}
	}

	
}
